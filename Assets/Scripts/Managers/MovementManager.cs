using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;

    [Header("General Components")]
    public List<FriendMovement> friendMovements = new List<FriendMovement>();

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

    //public void AddToMovementList(FriendMovement friendMovement)
    //{
    //    if (!friendMovements.Contains(friendMovement)) friendMovements.Add(friendMovement);    
    //}
    //public void RemoveFromMovementList(FriendMovement friendMovement)
    //{
    //    if (friendMovements.Contains(friendMovement)) friendMovements.Remove(friendMovement); 
    //}
    public void MoveFriend(Vector3 des)
    {
        if (friendMovements.Count == 0) return;

        MovementIndicatorManager.instance.ShowIndicator(des);
        for (int i = 0; i < friendMovements.Count; i++)
            friendMovements[i].Move(des);
    }
    public void MoveToFurn(Vector3 des)
    {
        if (friendMovements.Count == 0) return;

        for (int i = 0; i < friendMovements.Count; i++)
        {
            friendMovements[i].Move(des);
            friendMovements[i].MoveToFurn();
        }
    }
    //public void MoveFriends(Vector3 des)
    //{
    //    if (friendMovements.Count == 0) return;

    //    MovementIndicatorManager.instance.ShowIndicator(des);
    //    for (int i = 0; i < friendMovements.Count; i++)
    //        friendMovements[i].Move(des);   
    //}
}
