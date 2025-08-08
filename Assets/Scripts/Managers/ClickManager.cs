using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager instance;

    [Header("General Components")]
    [SerializeField] private Camera mainCamera;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask friendLayer;
    [SerializeField] private LayerMask combinedLayer;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;   
    }
    void Update()
    {
        CheckForInput();    
    }

    private void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, combinedLayer)) return;

            if (((1 << hit.transform.gameObject.layer) & friendLayer) != 0)
            {
                FriendSelectionManager.instance.SelectFriend(hit.collider.gameObject);
            }
            else if (((1 << hit.transform.gameObject.layer) & groundLayer) != 0)
            {
                MovementManager.instance.MoveFriend(hit.point);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            FriendSelectionManager.instance.UnselectAllFriends();   
        }
    }
}
