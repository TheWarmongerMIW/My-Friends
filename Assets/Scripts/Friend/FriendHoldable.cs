using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendHoldable : MonoBehaviour, IHoldable
{
    [SerializeField] private GameObject friendMenu;
    public bool hasMenuActivated;

    void Start()
    {
        
    }
    void Update()
    {
        hasMenuActivated = friendMenu.activeSelf;
    }

    public void OnHold()
    {
        friendMenu.SetActive(true);
    }
}
