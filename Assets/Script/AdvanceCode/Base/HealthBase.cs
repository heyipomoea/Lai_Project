using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public abstract class HealthBase : MonoBehaviour
    {
        [Header("基礎生命參數")]
        [SerializeField] protected GameObject ref_FX;
        [SerializeField] protected float maxHP = 10f;
        public float MaxHP => maxHP;
        protected float hp;
        public float CurrentHP => hp;

        protected float AfterDeadTime = 3f;
        protected bool isDead = false;
        public bool IsDead => isDead;
        protected EventHandler deadReceived;
        public event EventHandler DeadReceived
        {
            add { deadReceived += value; }
            remove { deadReceived -= value; }
        }

        protected EventHandler hitReceived;
        public event EventHandler HitReceived
        {
            add { hitReceived += value; }
            remove { hitReceived -= value; }
        }

        protected EventHandler revivalReceived;
        public event EventHandler RevivalReceived
        {
            add { revivalReceived += value; }
            remove { revivalReceived -= value; }
        }


        protected virtual void Start()
        {
            hp = maxHP;
        }

        public virtual void OnDamage(float damage)
        {
            hitReceived?.Invoke(this, EventArgs.Empty);
            hp -= damage;
            if (hp <= 0 && !isDead) { OnDead(); }
        }

        public virtual void OnHealing(float heal)
        {
            hp += heal;
            if (hp >= maxHP) { hp = maxHP; }
        }

        protected virtual void OnDead()
        {
            isDead = true;

            deadReceived?.Invoke(this, EventArgs.Empty);
            enabled = false;
            StartCoroutine(AfterDead());
        }

        protected virtual IEnumerator AfterDead()
        {
            yield return new WaitForSeconds(AfterDeadTime);
            GameObject tempFx = Instantiate(ref_FX, transform.position, Quaternion.identity);
            tempFx.AddComponent<ParticleEffectController>();
        }

        public virtual void OnRevival()
        {
            revivalReceived?.Invoke(this, EventArgs.Empty);
            hp = maxHP;

            isDead = false;
            enabled = true;
        }

        protected virtual void OnDestroy()
        {
            hitReceived = null;
            deadReceived = null;
            revivalReceived = null;
        }
    }
}

