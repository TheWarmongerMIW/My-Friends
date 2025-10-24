using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class MainCameraControl : MonoBehaviour
{
    public static MainCameraControl instance;

    [Header("Movement Options")]
    [SerializeField] private bool moveWithKeyboard;
    [SerializeField] private bool moveWithMouseDrag;
    [SerializeField] private bool moveWithEdgeScrolling;
    [SerializeField] private bool isDragging;
    public bool isEdgeScrolling;

    [Header("General Components")]
    [SerializeField] private Transform cameraRigTransform;
    [SerializeField] private Transform cameraTransform;
    public Transform followTransform;
    [SerializeField] private float rightOffsetAmount;

    [Header("Movement Components")]
    [SerializeField] private float defaultMovementSpeed;
    [SerializeField] private float defaultMovementSmoothness;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSmoothness;
    [SerializeField] private float dragDelay;
    [SerializeField] private float edgeDelay;

    [Header("Rotation Components")]
    [SerializeField] private float defaultRotationAngle;
    [SerializeField] private float defaultRotationSpeed;
    [SerializeField] private float defaultRotationSmoothness;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSmoothness;

    [Header("Tilt Components")]
    [SerializeField] private float defaultTiltAngle;
    [SerializeField] private float defaultTiltSpeed;
    [SerializeField] private float defaultTiltSmoothness;
    [SerializeField] private float tiltSpeed;
    [SerializeField] private float tiltSmoothness;
    [SerializeField] private float minTilt;
    [SerializeField] private float maxTilt;

    [Header("Zoom Components")]
    [SerializeField] private float defaultZoomSpeed;
    [SerializeField] private float defaultZoomSmoothness;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomSmoothness;
    [SerializeField] private float zoomDelay;
    [SerializeField] private Vector3 minZoom;
    [SerializeField] private Vector3 maxZoom;

    private InputActionMaps inputActionMaps;

    private Coroutine currentlyDragging;
    private Coroutine currentlyEdgeScrolling;
    private Coroutine currentlyZooming;

    private float currentX;
    private float currentY;
    private float currentZ;

    private Vector3 startDragPos;
    private Vector3 currentDragPos;
    private Vector3 newPos;
    private Vector3 newZoom;
    private Quaternion newRotation;
    private Quaternion newTilt;
    private float edgeSize = 50f;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {
        inputActionMaps = new InputActionMaps();
        inputActionMaps.Camera.Enable();
        inputActionMaps.Cursor.Enable();

        movementSpeed = defaultMovementSpeed;
        movementSmoothness = defaultMovementSmoothness; 

        rotationSpeed = defaultRotationSpeed;
        rotationSmoothness = defaultRotationSmoothness;

        tiltSpeed = defaultTiltSpeed;
        tiltSmoothness = defaultTiltSmoothness;     
        
        zoomSpeed = defaultZoomSpeed;
        zoomSmoothness = defaultZoomSmoothness;

        currentX = cameraRigTransform.localEulerAngles.x;
        currentY = cameraRigTransform.localEulerAngles.y;
        currentZ = cameraRigTransform.localEulerAngles.z;

        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
        newTilt = cameraTransform.localRotation;    
    }
    void LateUpdate()
    {
        HandleMovement();
        RotateAndTiltCamera();
        ZoomCamera();
    }

    private void HandleMovement()
    {
        if (followTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, followTransform.position, movementSmoothness * Time.deltaTime);
        }
        else
        {
            if (moveWithKeyboard) KeyboardMovement();
            if (moveWithMouseDrag) MouseDragMovement();
            if (moveWithEdgeScrolling) EdgeScrollingMovement();
        }
    }
    private void KeyboardMovement()
    {
        Vector2 inputVector = inputActionMaps.Camera.Movement.ReadValue<Vector2>();
        Vector3 inputDir = new Vector3(inputVector.x, 0, inputVector.y);

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 moveDir = (right * inputDir.x + forward * inputDir.z).normalized;

        newPos += moveDir * movementSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, newPos, movementSmoothness * Time.deltaTime);
    }
    private void MouseDragMovement()
    {
        if (inputActionMaps.Cursor.Drag.IsPressed())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                if (!isDragging)
                {
                    startDragPos = hit.point;
                    isDragging = true;

                    CursorManager.instance.SetCursor("Hand Closed");
                }
                else
                {
                    currentDragPos = hit.point;
                    newPos = transform.position + startDragPos - currentDragPos;
                }
            }
        }
        else
        {
            if (isDragging)
            {
                isDragging = false;
                CursorManager.instance.SetCursor("Hand Open");

                if (currentlyDragging != null) StopCoroutine(currentlyDragging);
                currentlyDragging = StartCoroutine(CursorManager.instance.ResetCursorToArrow(dragDelay));
            }
        }

        #region No Open Hand
        //if (inputActionMaps.Cursor.Drag.IsPressed())
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        //    {
        //        if (!isDragging)
        //        {
        //            startDragPos = hit.point;
        //            isDragging = true;

        //            CursorManager.instance.SetCursor("Hand Closed");
        //        }
        //        else
        //        {
        //            currentDragPos = hit.point;
        //            newPos = transform.position + startDragPos - currentDragPos;
        //        }
        //    }
        //}
        //else
        //{
        //    if (isDragging)
        //    {
        //        isDragging = false;
        //        CursorManager.instance.SetCursor("Arrow");

        //    }
        //}
        #endregion
    }
    private void EdgeScrollingMovement()
    {
        Vector3 moveDir = Vector3.zero;
        Vector3 mousePos = UnityEngine.Input.mousePosition;
        //string currentNav = " ";

        bool onRight = mousePos.x > Screen.width - edgeSize;
        bool onLeft = mousePos.x < edgeSize;
        bool onTop = mousePos.y > Screen.height - edgeSize;
        bool onBottom = mousePos.y < edgeSize;

        // Get camera-relative directions
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Diagonal directions
        if (onTop && onRight) moveDir = (forward + right).normalized;
        else if (onTop && onLeft) moveDir = (forward - right).normalized;
        else if (onBottom && onRight) moveDir = (-forward + right).normalized;
        else if (onBottom && onLeft) moveDir = (-forward - right).normalized;

        // Cardinal directions
        else if (onTop) moveDir = forward;
        else if (onBottom) moveDir = -forward;
        else if (onRight) moveDir = right;
        else if (onLeft) moveDir = -right;

        newPos += moveDir * movementSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, newPos, movementSmoothness * Time.deltaTime);
        //Debug.Log($"{moveDir}");

        //if (moveDir.x > 0 && moveDir.z < 0) currentNav = "Nav East";
        //else if (moveDir.x < 0 && moveDir.z > 0) currentNav = "Nav West";
        //else if (moveDir.x > 0 && moveDir.z > 0) currentNav = "Nav North";
        //else if (moveDir.x < 0 && moveDir.z < 0) currentNav = "Nav South";
        //if (moveDir.x == 1f) currentNav = "Nav North East";

        //if (currentNav != " ") CursorManager.instance.SetCursor(currentNav);

        //if (newPos != Vector3.zero) isEdgeScrolling = true;
        //else isEdgeScrolling = false;

        //if (!isEdgeScrolling)
        //{
        //    if (currentlyEdgeScrolling != null) StopCoroutine(currentlyEdgeScrolling);
        //    currentlyEdgeScrolling = StartCoroutine(ReturnToArrowAfterEdgeDelay(edgeDelay));
        //}
    }
    private IEnumerator ReturnToArrowAfterEdgeDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CursorManager.instance.SetCursor("Arrow");
    }
    private void RotateAndTiltCamera()
    {
        float rotateDir = 0;
        if (inputActionMaps.Camera.RotateLeft.IsPressed()) rotateDir = 1;
        if (inputActionMaps.Camera.RotateRight.IsPressed()) rotateDir = -1;
        if (rotateDir != 0) currentY += rotateDir * rotationSpeed * Time.deltaTime;
        else if (inputActionMaps.Camera.ResetRotation.WasPressedThisFrame()) currentY = defaultRotationAngle;

        float tiltDir = 0;
        if (inputActionMaps.Camera.TiltUp.IsPressed()) tiltDir = 1;
        if (inputActionMaps.Camera.TiltDown.IsPressed()) tiltDir = -1;
        if (tiltDir != 0) currentX = Mathf.Clamp(currentX + tiltDir * tiltSpeed * Time.deltaTime, minTilt, maxTilt);
        else if (inputActionMaps.Camera.ResetTilt.WasPressedThisFrame()) currentX = defaultTiltAngle;

        Quaternion targetRot = Quaternion.Euler(currentX, currentY, currentZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSmoothness * Time.deltaTime);
    }
    private void ZoomCamera()
    {
        Vector2 zoomDelta = inputActionMaps.Cursor.Scroll.ReadValue<Vector2>();
        Vector3 zoomDir = new Vector3(0, -zoomDelta.y, +zoomDelta.y) * zoomSpeed * Time.deltaTime;

        if (zoomDir == Vector3.zero) return;

        if (zoomDelta.y > 0) CursorManager.instance.SetCursor("Zoom In");
        else if (zoomDelta.y < 0) CursorManager.instance.SetCursor("Zoom Out");

        if (currentlyZooming != null) StopCoroutine(currentlyZooming);
        currentlyZooming = StartCoroutine(CursorManager.instance.ResetCursorToArrow(zoomDelay));

        newZoom += zoomDir;
        newZoom.y = Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y);
        newZoom.z = Mathf.Clamp(newZoom.z, maxZoom.z, minZoom.z);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, zoomSmoothness * Time.deltaTime);
    }
    public void StopFollowing()
    {
        Vector3 previousFollow = followTransform.position;
        Vector3 rightOffset = Camera.main.transform.right * rightOffsetAmount;
        newPos = previousFollow + rightOffset;
        followTransform = null;
    }

    #region Legacy
    private void RotateCamera1()
    {
        Vector3 rotateDir = Vector3.zero;

        if (inputActionMaps.Camera.RotateLeft.IsPressed()) rotateDir = Vector3.up;
        if (inputActionMaps.Camera.RotateRight.IsPressed()) rotateDir = Vector3.down;

        if (rotateDir != Vector3.zero) newRotation *= Quaternion.Euler(rotateDir * rotationSpeed * Time.deltaTime);
        else if (inputActionMaps.Camera.ResetRotation.WasPressedThisFrame()) newRotation = Quaternion.Euler(0, 45, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSmoothness * Time.deltaTime);
    }
    private void RotateCamera()
    {
        float rotateDir = 0;

        if (inputActionMaps.Camera.RotateLeft.IsPressed()) rotateDir = 1;
        if (inputActionMaps.Camera.RotateRight.IsPressed()) rotateDir = -1;

        if (rotateDir != 0) currentY += rotateDir * rotationSpeed * Time.deltaTime;
        else if (inputActionMaps.Camera.ResetRotation.WasPressedThisFrame()) currentY = defaultRotationAngle;

        newRotation = Quaternion.Euler(currentX, currentY, currentZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSmoothness * Time.deltaTime);
    }
    private void TiltCamera()
    {
        float tiltDir = 0;

        if (inputActionMaps.Camera.TiltUp.IsPressed()) tiltDir = 1;
        if (inputActionMaps.Camera.TiltDown.IsPressed()) tiltDir = -1;

        if (tiltDir != 0)
        {
            currentX += tiltDir * tiltSpeed * Time.deltaTime;
            currentX = Mathf.Clamp(currentX, minTilt, maxTilt);
        }
        else if (inputActionMaps.Camera.ResetTilt.WasPressedThisFrame()) currentX = defaultTiltAngle;

        newTilt = Quaternion.Euler(currentX, currentY, currentZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, newTilt, tiltSmoothness * Time.deltaTime);
    }
    #endregion
}
