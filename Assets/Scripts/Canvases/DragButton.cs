using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragButton: MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Button Components")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas buttonCanvas;

    [Header("General Components")]
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject[] friends;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float distanceToCamera;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(rectTransform.position);
        distanceToCamera = Vector3.Dot(
            house.transform.position - mainCamera.transform.position,
            mainCamera.transform.forward);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CursorManager.instance.SetCursor("Hand Closed");
        ControlFriendsNav(false);
    }
    public void OnDrag(PointerEventData eventData)
    {
        //Drag button
        rectTransform.anchoredPosition += eventData.delta / buttonCanvas.scaleFactor;
        CursorManager.instance.SetCursor("Hand Closed");

        //Calcualte house drag distance
        Vector3 worldDelta = mainCamera.ScreenToWorldPoint(
            new Vector3(eventData.delta.x, eventData.delta.y, distanceToCamera)) 
            - mainCamera.ScreenToWorldPoint(new Vector3(0, 0, distanceToCamera));

        //Drag house
        ControlFriendsNav(false);
        MovementIndicatorManager.instance.DestroyIndicator();
        house.transform.position += worldDelta;    
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        CursorManager.instance.SetCursor("Hand Open");
        ControlFriendsNav(true);
    }
    private void ControlFriendsNav(bool value)
    {
        foreach (GameObject friend in FriendSelectionManager.instance.GetCurrentFriendsList())
            friend.GetComponent<NavMeshAgent>().enabled = value;
    }
}
