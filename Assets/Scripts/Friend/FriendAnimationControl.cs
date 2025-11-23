using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FriendAnimationControl: MonoBehaviour
{
    [Header("Script Components")]
    [SerializeField] private FriendMovement friendMovement;

    [Header("General Compoenents")]
    [SerializeField] private Animator animator;

    private event Action<float> OnSpeedChange;

    void Start()
    {
        friendMovement = GetComponent<FriendMovement>();    
        animator = GetComponent<Animator>();

        OnSpeedChange += SetSpeed;
        friendMovement.OnStartJump.AddListener(this.SetJump);
        SetSpeed(0);    
    }

    private void FriendAnimationControl_OnJump(TriggerEventUnit obj)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        WalkingAnim();
    }
    
    private void WalkingAnim()
    {
        OnSpeedChange?.Invoke(friendMovement.CalculateSpeed());
        friendMovement.MoveToFurn();
    }
    public void SetJump()
    {
        animator.SetTrigger("Jump");
    }
    public void SetSpeed(float speed)
    {
        animator.SetFloat("Velocity", speed);
    }
}
