using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSelectionManager : MonoBehaviour
{
    public static FriendSelectionManager instance;

    [Header("General Components")]
    public List<GameObject> currentFriends = new List<GameObject>();
    public List<GameObject> selectedFriends = new List<GameObject>(); 

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {    
    }
    void Update()
    {
    }

    public void SelectFriend(GameObject friend)
    {
        UnselectAllFriends();
        EnableFriend(friend);
    }
    public void MultiSelect(GameObject friend)
    {
        if (!selectedFriends.Contains(friend))
            EnableFriend(friend);
    }
    public void UnselectAllFriends()
    {
        foreach (var selectedFriend in new List<GameObject>(selectedFriends))
            selectedFriend.GetComponent<FriendSelectable>().OnDeselect();   

        selectedFriends.Clear();    
    }
    public void AddToSelectedFriendsList(GameObject friend)
    {
        if (!selectedFriends.Contains(friend))
        {
            selectedFriends.Add(friend);
            MovementManager.instance.friendMovements.Add(friend.GetComponent<FriendMovement>());
        }
    }
    public void RemoveFromSelectedFriendsList(GameObject friend)
    {
        if (selectedFriends.Contains(friend))
        {
            selectedFriends.Remove(friend);
            MovementManager.instance.friendMovements.Remove(friend.GetComponent<FriendMovement>());
        }
    }
    private void EnableFriend(GameObject friend)
    {
        friend.GetComponent<FriendSelectable>().OnSelect();
    }
}
