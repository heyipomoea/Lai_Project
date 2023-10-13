//using BasicCode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public abstract class DamageColliderFunctionBase : MonoBehaviour
    {
        [SerializeField] protected GameObject ref_FX;
        
        protected CharacterMovementBase characterMovementbase;
        protected AnimationControllerBase animationControllerBase;
        protected GameObject owner;
        protected string selfTag;

        protected FireEventArgs fireEventArgs;

        protected Collider col;

        protected virtual void Start()
        {
            col = GetComponent<Collider>();
            col.enabled = false;

            characterMovementbase = GetComponentInParent<CharacterMovementBase>();
            characterMovementbase.Fire1Received += OnFire1;
            owner = characterMovementbase.gameObject;
            selfTag = characterMovementbase.tag;

            animationControllerBase = GetComponentInParent<AnimationControllerBase>();
            animationControllerBase.StartAnimReceived += OnStartAttackAnimation;
            animationControllerBase.EndAnimReceived += OnEndAttackAnimation;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
            {
                Rigidbody rb = other.attachedRigidbody;
                if (rb)
                {
                    Vector3 direction = Vector3.Normalize(other.gameObject.transform.position - owner.transform.position);
                    rb.AddForce(direction * fireEventArgs.FireForce, ForceMode.Impulse);

                    if (!other.CompareTag(selfTag))
                    {
                        OnTakeFX(other);
                        OnTakeDamage(other);
                    }
                }
            }
        }

        protected virtual void OnTakeDamage(Collider other)
        {
            HealthBase healthBase = other.GetComponent<HealthBase>();
            if (healthBase) healthBase.OnDamage(fireEventArgs.FireDamage);
        }

        protected virtual void OnTakeFX(Collider other)
        {
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            GameObject tempFx = Instantiate(ref_FX, contactPoint, Quaternion.identity);
            tempFx.AddComponent<ParticleEffectController>();
        }

        protected virtual void OnFire1(object sender, FireEventArgs e)
        {
            fireEventArgs = e;
        }

        protected virtual void OnStartAttackAnimation(object sender, EventArgs e)
        {
            col.enabled = true;
        }

        protected virtual void OnEndAttackAnimation(object sender, EventArgs e)
        {
            col.enabled = false;
        }
    }
}
