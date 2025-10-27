using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;


public class ClickManager : MonoBehaviour
{
    public static ClickManager instance;

    [Header("General Components")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float holdTimer;
    [SerializeField] private float holdDuration;
    [SerializeField] private float loadingCursorDelay;
    public bool cursorIsLoading;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask friendLayer;
    [SerializeField] private LayerMask combinedLayer;

    private InputActionMaps inputActionMaps;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;   

        inputActionMaps = new InputActionMaps();
        inputActionMaps.Cursor.Enable();
        inputActionMaps.Friend.Enable();    
    }
    void Update()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (inputActionMaps.Cursor.Select.IsPressed())
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayer)) return;

            if (((1 << hit.transform.gameObject.layer) & friendLayer) != 0)
            {
                if (!hit.collider.gameObject.GetComponent<FriendHoldable>().hasMenuActivated)
                {
                    holdTimer += Time.deltaTime;

                    if (holdTimer >= loadingCursorDelay && holdTimer < holdDuration)
                    {
                        cursorIsLoading = true;
                        float progress = Mathf.Clamp01(holdTimer / holdDuration);
                        CursorManager.instance.SetCursorProgress("Loading", progress);
                    }

                    if (holdTimer >= holdDuration)
                    {
                        cursorIsLoading = false;
                        holdTimer = 0;

                        CursorManager.instance.SetCursor("Arrow");
                        hit.collider.gameObject.GetComponent<FriendHoldable>().OnHold();
                    }
                }

                if (inputActionMaps.Friend.MultiSelect.IsPressed()) 
                    FriendSelectionManager.instance.MultiSelect(hit.collider.gameObject);
                else 
                    FriendSelectionManager.instance.SelectFriend(hit.collider.gameObject);
            }
            else if (((1 << hit.transform.gameObject.layer) & groundLayer) != 0)
            {
                MovementManager.instance.MoveFriend(hit.point);
            }
        }
        else
        {
            holdTimer = 0;
        }

        if (inputActionMaps.Cursor.Unselect.WasPressedThisFrame())
        {
            FriendSelectionManager.instance.UnselectAllFriends();   
        }
    }
}
