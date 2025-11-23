using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnHoverable : MonoBehaviour, IHoverable
{
    [Header("General Components")]
    [SerializeField] private Renderer[] renderers;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHoverEnter()
    {
        if (FriendSelectionManager.instance.GetSelectedFriendsList().Count != 0)
            foreach (var renderer in renderers)
                renderer.material.EnableKeyword("_EMISSION");
    }
    public void OnHoverStay()
    {
        if (FriendSelectionManager.instance.GetSelectedFriendsList().Count != 0)
            CursorManager.instance.SetCursor("Pointer");
    }
    public void OnHoverExit()
    {
        foreach (var renderer in renderers)
            renderer.material.DisableKeyword("_EMISSION");

        CursorManager.instance.SetCursor("Arrow");
    }
}
