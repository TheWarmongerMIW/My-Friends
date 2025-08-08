using System;
using System.Collections;
using System.Collections.Generic;
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
        SetSpeed(0);    
    }

    // Update is called once per frame
    void Update()
    {
        WalkingAnim();
    }

    private void WalkingAnim()
    {
        OnSpeedChange?.Invoke(friendMovement.CalculateSpeed());
    }
    private void SetSpeed(float speed)
    {
        animator.SetFloat("Velocity", speed);
    }
}
