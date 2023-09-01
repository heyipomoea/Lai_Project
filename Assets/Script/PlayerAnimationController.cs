using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerMovementController playerMovementController;
    private Animator animator;
    private int velXHash, velYHash;

    private int jumpTriggerHash, isGroundedHash;
    private int attack1Index = 0, attack1MaxIndex = 2;
    private int attack1Hash, attack1IndexHash;

    private EventHandler startAnimReceived, endAnimReceived;
    public event EventHandler StartAnimReceived
    {
        add { startAnimReceived += value; }
        remove { startAnimReceived -= value; }
    }
    public event EventHandler EndAnimReceived
    {
        add { endAnimReceived += value; }
        remove { endAnimReceived -= value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovementController = GetComponentInParent<PlayerMovementController>();

        playerMovementController.Fire1Received += OnFire1;
        attack1Hash = Animator.StringToHash("attack1Trigger");
        attack1IndexHash = Animator.StringToHash("attack1Index");

        playerMovementController.JumpReceived += OnJump;
        jumpTriggerHash = Animator.StringToHash("JumpTrigger");
        isGroundedHash = Animator.StringToHash("IsGrounded");

        animator = GetComponent<Animator>();
        velXHash = Animator.StringToHash("VelX");
        velYHash = Animator.StringToHash("VelY");

        
    }

    void StartAttackAnimation()
    {
        startAnimReceived?.Invoke(this, EventArgs.Empty);
    }

    void EndAttackAnimation()
    {
        endAnimReceived?.Invoke(this, EventArgs.Empty);
    }

    private void OnFire1(object sender, EventArgs e)
    {
        attack1Index++;
        if (attack1Index > attack1MaxIndex) attack1Index = 0;
        animator.SetTrigger(attack1Hash);
        animator.SetInteger(attack1IndexHash, attack1Index);
    }

    private void OnJump(object sender, EventArgs e)
    {
        animator.SetTrigger(jumpTriggerHash);
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        animator.SetBool(isGroundedHash, playerMovementController.IsGrounded());
    }

    private void OnMove()
    {
        Vector3 localMoveDirection = transform.InverseTransformDirection(playerMovementController.DeltaPos);
        Vector3 normalVel = Vector3.Normalize(localMoveDirection);
        Vector3 targetVel = Vector3.Lerp(localMoveDirection, normalVel, playerMovementController.DeltaInput.magnitude);

        animator.SetFloat(velXHash, targetVel.x);
        animator.SetFloat(velYHash, targetVel.z);

        //animator.SetFloat(velXHash, localMoveDirection.x * playerMovementController.MoveSpeed * 30f);
        //animator.SetFloat(velYHash, localMoveDirection.z * playerMovementController.MoveSpeed * 30f);
    }
}
