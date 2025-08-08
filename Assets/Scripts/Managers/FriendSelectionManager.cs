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
    public void UnselectAllFriends()
    {
        foreach (var selectedFriend in selectedFriends)
            selectedFriend.GetComponent<FriendSelectable>().OnDeselect();   

        selectedFriends.Clear();    
    }
    private void EnableFriend(GameObject friend)
    {
        selectedFriends.Add(friend);
        friend.GetComponent<FriendSelectable>().OnSelect();
    }
}
