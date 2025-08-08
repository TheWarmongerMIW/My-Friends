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
    public bool isDragging;

    [Header("General Components")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform followTransform;
    [SerializeField] private InputActionMaps inputActionMaps;

    [Header("Movement Components")]
    [SerializeField] private float defaultMovementSpeed;
    [SerializeField] private float defaultMovementSmoothness;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSmoothness;

    [Header("Rotation Components")]
    [SerializeField] private float defaultRotationSpeed;
    [SerializeField] private float defaultRotationSmoothness;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSmoothness;

    [Header("Zoom Components")]
    [SerializeField] private float defaultZoomSpeed;
    [SerializeField] private float defaultZoomSmoothness;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomSmoothness;
    [SerializeField] private Vector3 minZoom;
    [SerializeField] private Vector3 maxZoom;

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

        movementSpeed = defaultMovementSpeed;
        movementSmoothness = defaultMovementSmoothness; 

        rotationSpeed = defaultRotationSpeed;
        rotationSmoothness = defaultRotationSmoothness;
        
        zoomSpeed = defaultZoomSpeed;
        zoomSmoothness = defaultZoomSmoothness;

        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
        newTilt = cameraTransform.localRotation;    
    }

    // Update is called once per frame
    void LateUpdate()
    {
        HandleMovement();
        RotateCamera();
        ZoomCamera();
    }

    private void HandleMovement()
    {
        if (moveWithKeyboard) KeyboardMovement();
        if (moveWithMouseDrag) MouseDragMovement();
        if (moveWithEdgeScrolling) EdgeScrollingMovement();
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
        if (inputActionMaps.Camera.Drag.IsPressed())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                if (!isDragging)
                {
                    startDragPos = hit.point;
                    isDragging = true;
                }
                else
                {
                    currentDragPos = hit.point;
                    newPos = transform.position + startDragPos - currentDragPos;
                }
            }
        }
        else isDragging = false;
    }
    private void EdgeScrollingMovement()
    {
        Vector3 moveDir = Vector3.zero;
        Vector3 mousePos = UnityEngine.Input.mousePosition;

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
    }
    private void RotateCamera()
    {
        Vector3 rotateDir = Vector3.zero; 

        if (inputActionMaps.Camera.RotateLeft.IsPressed()) rotateDir = Vector3.up;
        if (inputActionMaps.Camera.RotateRight.IsPressed()) rotateDir = Vector3.down;

        if (rotateDir != Vector3.zero) newRotation *= Quaternion.Euler(rotateDir * rotationSpeed * Time.deltaTime);
        else if (inputActionMaps.Camera.ResetRotation.WasPressedThisFrame()) newRotation = Quaternion.Euler(0, 45, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSmoothness * Time.deltaTime);
    }
    private void ZoomCamera()
    {
        Vector2 zoomDelta = inputActionMaps.Camera.Scroll.ReadValue<Vector2>();
        Vector3 zoomDir = new Vector3(0, -zoomDelta.y, +zoomDelta.y) * zoomSpeed * Time.deltaTime;

        if (zoomDir == Vector3.zero) return;

        newZoom += zoomDir;
        newZoom.y = Mathf.Clamp(newZoom.y, minZoom.y, maxZoom.y);
        newZoom.z = Mathf.Clamp(newZoom.z, maxZoom.z, minZoom.z);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, zoomSmoothness * Time.deltaTime);
    }    
}
