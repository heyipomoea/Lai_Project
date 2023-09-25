using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageColliderFunction : MonoBehaviour
{
    [SerializeField] private GameObject ref_FX;

    private EnemyMovementController enemyMovementController;
    private EnemyAnimationController enemyAnimationController;
    private FireEventArgs fireEventArgs;
    private Collider col;

    private GameObject owner;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;

        enemyMovementController = GetComponentInParent<EnemyMovementController>();
        enemyMovementController.Fire1Received += OnFire1;

        enemyAnimationController = GetComponentInParent<EnemyAnimationController>();
        enemyAnimationController.StartAnimReceived += OnStartAttackAnimation;
        enemyAnimationController.EndAnimReceived += OnEndAttackAnimation;

        owner = enemyMovementController.gameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb)
            {
                Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
                Vector3 direction = Vector3.Normalize(other.gameObject.transform.position - owner.transform.position);
                rb.AddForce(direction * fireEventArgs.FireForce, ForceMode.Impulse);
                GameObject tempFx = Instantiate(ref_FX, contactPoint, Quaternion.identity);
                tempFx.AddComponent<ParticleEffectController>();

                if (!other.CompareTag("Enemy"))
                {
                    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                    if(playerHealth)
                    {
                        playerHealth.OnDamage(fireEventArgs.FireDamage);
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
