
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SelectionBoxCanvas : MonoBehaviour
{
    public static SelectionBoxCanvas instance;
    private InputActionMaps inputActionMaps;

    [Header("General Components")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform boxVisual;
    [SerializeField] private float dragThreshold;
    [SerializeField] private bool isDragging;
    public bool selectingFriends;

    private Rect selectionBox;
    private Vector2 startPos;
    private Vector2 endPos;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;
        startPos = Vector2.zero;
        endPos = Vector2.zero;

        inputActionMaps = new InputActionMaps();
        inputActionMaps.Cursor.Enable();    

        DrawVisual();
    }
    void Update()
    {
        //Clicking
        if (inputActionMaps.Cursor.Select.WasPressedThisFrame())
        {
            startPos = Input.mousePosition;
            selectionBox = new Rect();
        }

        //Dragging
        if (inputActionMaps.Cursor.Select.IsPressed())
        {
            if (boxVisual.rect.width != 0 || boxVisual.rect.height != 0)
            {
                FriendSelectionManager.instance.UnselectAllFriends();
                //SelectFriends();
            }

            endPos = Input.mousePosition;

            DrawVisual();
            DrawSelection();
        }

        //Releasing
        if (inputActionMaps.Cursor.Select.WasReleasedThisFrame())
        {
            //SelectFriends();
            CursorModeManager.instance.OnMultiSelectDisable.Invoke();
            CursorModeManager.instance.enableMultiSelect = false;

            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawVisual();
        }
    }

    private void DrawVisual()
    {
        //Calculate the start and end point of the selection box
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        //Calculate the center of the selection box
        Vector2 boxCenter = (boxStart + boxEnd) / 2;

        //Set the position of the visual selection box based on its center
        boxVisual.position = boxCenter;

        //Calculate the size of the selection box in both width and height
        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }
    private void DrawSelection()
    {
        if (Input.mousePosition.x < startPos.x)
        {
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPos.x;
        }
        else
        {
            selectionBox.xMin = startPos.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPos.y)
        {
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPos.y;
        }
        else
        {
            selectionBox.yMin = startPos.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    #region Legacy
    //private void SelectFriends()
    //{
    //    foreach (var friend in FriendSelectionManager.instance.currentFriends)
    //    {
    //        if (selectionBox.Contains(mainCamera.WorldToScreenPoint(friend.transform.position)))
    //        {
    //            FriendSelectionManager.instance.MultiSelect(friend);
    //        }
    //    }
    //}
    #endregion
}
