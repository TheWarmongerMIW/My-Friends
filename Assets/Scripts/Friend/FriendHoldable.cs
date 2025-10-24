using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class FriendHoldable : MonoBehaviour, IHoldable
{
    [SerializeField] private GameObject friendMenu;
    public bool hasActivated;

    void Start()
    {
        
    }
    void Update()
    {
        hasActivated = friendMenu.activeSelf;
    }

    public void OnHold()
    {
        friendMenu.SetActive(true);
    }
}
