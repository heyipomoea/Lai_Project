using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public abstract class AnimationControllerBase : MonoBehaviour
    {
        protected CharacterMovementBase characterMovement;
        protected HealthBase healthBase;
        protected Animator animator;
        protected int attack1Index = 0, attack1MaxIndex = 2;

        protected int velXHash, velYHash;
        protected int attack1Hash, attack1IndexHash;

        protected int hitTriggetHash, deadTriggerHash, revivalTriggerHash;

        protected EventHandler startAnimReceived, endAnimReceived;
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

        protected virtual void Start()
        {
            healthBase = GetComponentInParent<HealthBase>();
            healthBase.HitReceived += OnHit;
            healthBase.DeadReceived += OnDead;
            healthBase.RevivalReceived += OnRevival;

            characterMovement = GetComponentInParent<CharacterMovementBase>();
            characterMovement.Fire1Received += OnFire1;

            animator = GetComponentInChildren<Animator>();
            velXHash = Animator.StringToHash("VelX");
            velYHash = Animator.StringToHash("VelY");

            attack1Hash = Animator.StringToHash("Attack1Trigger");
            attack1IndexHash = Animator.StringToHash("Attack1Index");

            hitTriggetHash = Animator.StringToHash("HitTrigger");
            deadTriggerHash = Animator.StringToHash("DeadTrigger");
            revivalTriggerHash = Animator.StringToHash("RevivalTrigger");
        }

        protected virtual void StartAttackAnimation()
        {
            startAnimReceived?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void EndAttackAnimation()
        {
            endAnimReceived?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Update()
        {
            OnMove();
        }

        protected abstract void OnMove();

        protected virtual void OnHit(object sender, EventArgs e)
        {
            if(!healthBase.IsDead) animator.SetTrigger(hitTriggetHash);
        }

        protected virtual void OnDead(object sender, EventArgs e)
        {
            animator.SetTrigger(deadTriggerHash);
        }

        protected virtual void OnRevival(object sender, EventArgs e)
        {
            animator.SetTrigger(revivalTriggerHash);
        }

        protected virtual void OnFire1(object sender, EventArgs e)
        {
            animator.SetTrigger(attack1Hash);
        }

        protected virtual void OnDestroy()
        {
            startAnimReceived = null;
            endAnimReceived = null;
        }
    }
}

