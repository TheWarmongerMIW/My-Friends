using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendMenuCanvas : MonoBehaviour
{
    [Header("General Components")]
    [SerializeField] private GameObject friend;
    [SerializeField] private GameObject friendMenu;
    [SerializeField] private GameObject selectionSpotlight;

    [Header("Canvas Components")]
    [SerializeField] private TextMeshProUGUI followText;
    [SerializeField] private Image followBackground;
    [SerializeField] private Sprite followSprite;   
    [SerializeField] private Sprite stopFollowSprite;   
    [SerializeField] private Button closeButton;
    [SerializeField] private Button followButton;

    [Header("Stat Components")]
    [SerializeField] private float rightOffsetAmount;
    [SerializeField] private float upOffsetAmount;

    void Start()
    {

    }
    void Update()
    {
        OffsetMenuPos();
    }

    public void CloseMenu()
    {
        friendMenu.SetActive(false);
    }
    private void FollowFriend()
    {
        MainCameraControl.instance.followTransform = friend.transform;

        selectionSpotlight.GetComponent<Light>().color = Color.yellow;
        followBackground.sprite = stopFollowSprite;
        followText.text = "Stop";

        CloseMenu();
    }
    private void StopFollowFriend()
    {
        MainCameraControl.instance.StopFollowing();

        selectionSpotlight.GetComponent<Light>().color = Color.green;
        followBackground.sprite = followSprite;
        followText.text = "Follow";

        CloseMenu();
    }
    private void ResetFollowFriend()
    {
        FriendMenuCanvas previousMenu = MainCameraControl.instance.followTransform.gameObject.GetComponentInChildren<FriendMenuCanvas>();

        previousMenu.selectionSpotlight.GetComponent<Light>().color = Color.green;
        previousMenu.followBackground.sprite = previousMenu.followSprite;
        previousMenu.followText.text = "Follow";
    }
    public void StopOrFollowFriend()
    {
        if (followText.text == "Follow")
        {
            if (MainCameraControl.instance.followTransform == null) FollowFriend();
            else
            {
                ResetFollowFriend();
                FollowFriend();
            }
        }
        else if (followText.text == "Stop") StopFollowFriend();
    }
    private void OffsetMenuPos()
    {
        transform.forward = Camera.main.transform.forward;

        Vector3 rightOffset = Camera.main.transform.right * rightOffsetAmount;
        transform.position = friend.transform.position + rightOffset + Vector3.up * upOffsetAmount;
    }
}
