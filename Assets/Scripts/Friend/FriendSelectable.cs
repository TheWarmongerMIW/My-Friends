using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSelectable : MonoBehaviour, ISelectable
{
    [Header("Script Components")]
    [SerializeField] private Friend friend;
    [SerializeField] private FriendHoverable friendHoverable;
    [SerializeField] private FriendMovement friendMovement;

    [Header("General Components")]
    [SerializeField] private GameObject selectSpotlight;
    void Start()
    {
        friend = GetComponent<Friend>();
        friendHoverable = GetComponent<FriendHoverable>();  
        friendMovement = GetComponent<FriendMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelect()
    {
        friend.isSelected = true;
        friendHoverable.OnHoverExit();
        MovementManager.instance.AddToMovementList(friendMovement);
        selectSpotlight.SetActive(true);
    }
    public void OnDeselect()
    {
        friend.isSelected = false;
        MovementManager.instance.RemoveFromMovementList(friendMovement);
        selectSpotlight.SetActive(false);   
    }
}
