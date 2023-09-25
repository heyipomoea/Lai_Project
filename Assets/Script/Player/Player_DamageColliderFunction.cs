using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DamageColliderFunction : MonoBehaviour
{
    [SerializeField] private GameObject ref_FX;

    private PlayerMovementController playerMovementController;
    private PlayerAnimationController playerAnimationController;
    private FireEventArgs fireEventArgs;
    private Collider col;

    private GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;

        playerMovementController = GetComponentInParent<PlayerMovementController>();
        playerMovementController.Fire1Received += OnFire1;

        playerAnimationController = GetComponentInParent<PlayerAnimationController>();
        playerAnimationController.StartAnimReceived += OnStartAttackAnimation;
        playerAnimationController.EndAnimReceived += OnEndAttackAnimation;

        owner = playerMovementController.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger)
        {
            Rigidbody rb = other.attachedRigidbody;
            if(rb)
            {
                Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
                Vector3 direction = Vector3.Normalize(other.gameObject.transform.position - owner.transform.position);
                rb.AddForce(direction * fireEventArgs.FireForce, ForceMode.Impulse);
                GameObject tempFx = Instantiate(ref_FX, contactPoint, Quaternion.identity);
                tempFx.AddComponent<ParticleEffectController>();

                if(!other.CompareTag("Player"))
                {
                    float TotalDamage = fireEventArgs.FireDamage;
                    EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
                    if(enemyHealth)
                    {
                        enemyHealth.OnDamage(TotalDamage);
                    }
                }
            }
        }
    }

    private void OnFire1(object sender, FireEventArgs e)
    {
        fireEventArgs = e;
    }
    private void OnStartAttackAnimation(object sender, EventArgs e)
    {
        col.enabled = true;
    }
    private void OnEndAttackAnimation(object sender, EventArgs e)
    {
        col.enabled = false;
    }
}
