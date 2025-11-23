using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendSelectionManager : MonoBehaviour
{
    public static FriendSelectionManager instance;

    [Header("General Components")]
    [SerializeField] private Transform friendsRoot;
    [SerializeField] private List<GameObject> currentFriends = new List<GameObject>();
    [SerializeField] private List<GameObject> selectedFriends = new List<GameObject>(); 

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {    
        GetCurrentFriends();    
    }
    void Update()
    {
    }

    public List<GameObject> GetCurrentFriendsList()
    {
        return currentFriends;  
    }
    public List<GameObject> GetSelectedFriendsList()
    {
        return selectedFriends;
    }
    public void GetCurrentFriends()
    {
        currentFriends.Clear();

        foreach (Transform friend in friendsRoot)
            currentFriends.Add(friend.gameObject);
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
