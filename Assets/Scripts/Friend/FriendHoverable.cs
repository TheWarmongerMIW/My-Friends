using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendHoverable : MonoBehaviour, IHoverable
{
    [Header("Script Components")]
    [SerializeField] private Friend friend;

    [Header("General Components")]
    [SerializeField] private Renderer[] renderers;

    void Start()
    {
        friend = GetComponent<Friend>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHoverEnter()
    {
        if (!friend.isSelected)
        {
            foreach (var renderer in renderers)
                renderer.material.EnableKeyword("_EMISSION");
        }
    }

    public void OnHoverExit()
    {
        foreach (var renderer in renderers)
            renderer.material.DisableKeyword("_EMISSION");
    }
}
