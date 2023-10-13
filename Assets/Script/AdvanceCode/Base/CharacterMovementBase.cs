using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public abstract class CharacterMovementBase : MonoBehaviour
    {
        [Header("基礎移動參數")]
        [SerializeField] protected float moveSpeed = 15f;
        public float MoveSpeed => moveSpeed;
        [SerializeField] protected float RotationSpeed = 50f;
        protected Vector3 deltaPos;
        public Vector3 DeltaPos => deltaPos;

        [Header("基礎攻擊1參數")]
        [SerializeField] protected float Fire1Interval = .1f;
        [SerializeField] protected float Fire1Damage = 1.0f;
        [SerializeField] protected float Fire1Force = 1.0f;
        protected float fire1Cooldown = 0f;

        protected EventHandler<FireEventArgs> fire1Received;
        public event EventHandler<FireEventArgs> Fire1Received
        {
            add { fire1Received += value; }
            remove { fire1Received -= value; }
        }

        protected HealthBase healthBase;
        protected Rigidbody rb;

        protected virtual void Start()
        {
            healthBase = GetComponent<HealthBase>();
            healthBase.DeadReceived += OnDead;
            healthBase.RevivalReceived += OnRevival;

            rb = GetComponent<Rigidbody>();
        }

        protected virtual void Update() 
        {
            OnMove();
            OnRotation();
            OnFire();
            OnCoolDown();
        }

        protected abstract void OnMove();
        protected abstract void OnRotation();
        protected abstract void OnFire();

        protected virtual void Fire1()
        {
            FireEventArgs fireEventArgs = new FireEventArgs(Fire1Damage, Fire1Force);
            fire1Received?.Invoke(this, fireEventArgs);
            fire1Cooldown = Fire1Interval;
        }

        protected virtual void OnCoolDown()
        {
            if (fire1Cooldown > 0f) { fire1Cooldown -= Time.deltaTime; }
        }

        protected virtual void OnDead(object sender, EventArgs e)
        {
            enabled = false;
        }
        protected virtual void OnRevival(object sender, EventArgs e)
        {
            enabled = true;
        }

        protected virtual void OnDestroy()
        {
            fire1Received = null;
        }
    }
}

