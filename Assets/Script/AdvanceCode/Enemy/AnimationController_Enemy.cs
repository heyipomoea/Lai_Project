//using BasicCode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public class AnimationController_Enemy : AnimationControllerBase
    {
        protected CharacterMovement_Enemy characterMovement_Enemy;

        protected override void Start()
        {
            base.Start();
            characterMovement_Enemy = (CharacterMovement_Enemy)characterMovement;
        }

        protected override void OnHit(object sender, EventArgs e)
        {
            if(!characterMovement_Enemy.IsAttacking) base.OnHit(sender, e);
        }

        protected override void OnMove()
        {
            animator.SetFloat(velYHash, characterMovement_Enemy.DeltaPos.magnitude);
        }
    }
}

