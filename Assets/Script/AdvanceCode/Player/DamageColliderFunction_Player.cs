using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public class DamageColliderFunction_Player : DamageColliderFunctionBase
    {
        private CharacterMovement_Player characterMovement_Player;
        protected override void Start()
        {
            base.Start();
            characterMovement_Player = (CharacterMovement_Player)characterMovementbase;
            characterMovement_Player.Fire2Received += OnFire2;
        }

        protected override void OnTakeDamage(Collider other)
        {
            float TotalDamage = fireEventArgs.FireDamage + (GameManager.Instance.Gems + 1) * 0.1f;

            HealthBase healthBase = other.GetComponent<HealthBase>();
            if (healthBase) healthBase.OnDamage(TotalDamage);
        }

        private void OnFire2(object sender, FireEventArgs e)
        {
            fireEventArgs = e;
        }
    }
}

