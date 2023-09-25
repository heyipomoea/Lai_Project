using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private EnemyMovementController enemyMovementController;
    private EnemyHealth enemyHealth;
    private Animator animator;
    private int velYHash;

    private int attack1Hash;

    private int hitTriggerHash, deadTriggerHash;

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
        enemyMovementController = GetComponentInParent<EnemyMovementController>();

        enemyMovementController.Fire1Received += OnFire1;
        attack1Hash = Animator.StringToHash("attack1Trigger");

        animator = GetComponentInChildren<Animator>();
        
        velYHash = Animator.StringToHash("VelY");

        enemyHealth = GetComponentInParent<EnemyHealth>();
        enemyHealth.HitReceived += OnHit;
        enemyHealth.DeadReceived += OnDead;
        
        hitTriggerHash = Animator.StringToHash("HitTrigger");
        deadTriggerHash = Animator.StringToHash("DeadTrigger");
    }

    

    private void OnDead(object sender, EventArgs e)
    {
        animator.SetTrigger(deadTriggerHash);
    }

    private void OnHit(object sender, EventArgs e)
    {
        animator.SetTrigger(hitTriggerHash);
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
        animator.SetTrigger(attack1Hash);
    }

   

    // Update is called once per frame
    void Update()
    {
        OnMove();
    }

    private void OnMove()
    {
        animator.SetFloat(velYHash, enemyMovementController.DeltaPos.magnitude);
    }
}
