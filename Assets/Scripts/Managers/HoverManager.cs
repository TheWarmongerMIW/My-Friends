using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public static HoverManager instance;

    [SerializeField] private Camera mainCamera;

    private IHoverable lastHovered;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        CheckForHoverable();
    }

    private void CheckForHoverable()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        IHoverable currentHover = null;

        if (Physics.Raycast(ray, out hit)) currentHover = hit.collider.GetComponent<IHoverable>();
        if (currentHover != lastHovered)
        {
            if (lastHovered != null) lastHovered.OnHoverExit();
            if (currentHover != null) currentHover.OnHoverEnter();
            lastHovered = currentHover;
        }
        if (currentHover != null) currentHover.OnHoverStay();
    }
}
