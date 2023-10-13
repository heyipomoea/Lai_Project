//using BasicCode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public class AnimationController_Player : AnimationControllerBase
    {
        private CharacterMovement_Player characterMovementPlayer;
        private int attack2Hash;
        private int jumpTriggerHash, isGroundedHash;

        protected override void Start()
        {
            base.Start();

            characterMovementPlayer = (CharacterMovement_Player)characterMovement;
            characterMovementPlayer.Fire2Received += OnFire2;
            characterMovementPlayer.JumpReceived += OnJump;

            attack2Hash = Animator.StringToHash("Attack2Trigger");
            jumpTriggerHash = Animator.StringToHash("JumpTrigger");
            isGroundedHash = Animator.StringToHash("IsGrounded");
        }

        protected override void Update()
        {
            base.Update();
            animator.SetBool(isGroundedHash, characterMovementPlayer.IsGrounded());
        }
        protected override void OnMove()
        {
            Vector3 localMoveDirection = transform.InverseTransformDirection(characterMovementPlayer.DeltaPos);
            Vector3 normalVel = Vector3.Normalize(localMoveDirection);
            Vector3 targetVel = Vector3.Lerp(localMoveDirection, normalVel, characterMovementPlayer.DeltaInput.magnitude);

            animator.SetFloat(velXHash, targetVel.x);
            animator.SetFloat(velYHash, targetVel.z);
        }

        protected override void EndAttackAnimation()
        {
            base.EndAttackAnimation();
            attack1Index++;
            if (attack1Index > attack1MaxIndex) attack1Index = 0;
        }

        protected override void OnFire1(object sender, EventArgs e)
        {
            base.OnFire1(sender, e);
            animator.SetInteger(attack1IndexHash, attack1Index);
        }

        protected virtual void OnFire2(object sender, EventArgs e)
        {
            animator.SetTrigger(attack2Hash);
        }

        void OnJump(object sender, EventArgs e)
        {
            animator.SetTrigger(jumpTriggerHash);
        }
    }
}

