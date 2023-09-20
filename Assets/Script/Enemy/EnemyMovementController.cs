using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    [Header("移動參數")]
    [SerializeField] private float maxMoveDistance = 10f;
    [SerializeField] private float moveSpeed = 3f;
    public float MoveSpeed => moveSpeed;

    private Vector3 deltaPos;
    public Vector3 DeltaPos => deltaPos;

    [SerializeField] private float RotationSpeed = 5f;
    private float deltaVertical, deltaHorizontal;
    private Quaternion deltaRot;

    private Vector2 deltaInput;
    public Vector2 DeltaInput => deltaInput;

    [SerializeField] private float JumpForce = 5f;
    private float groundCheckDistance = 0.1f;

    [Header("攻擊1參數")]
    [SerializeField] private float closeAttackDistance = 2.0f;
    [SerializeField] private Vector2 FireRandomInterval = new Vector2(1f, 2f);
    [SerializeField] private float Fire1Interval = .1f;
    [SerializeField] private float Fire1Damage = 1.0f;
    [SerializeField] private float Fire1Force = 1.0f;
    private float fire1Cooldown = 0f;

    private EventHandler<FireEventArgs> fire1Received, fire2Received;
    public event EventHandler<FireEventArgs> Fire1Received
    {
        add { fire1Received += value; }
        remove { fire1Received -= value; }
    }

    public ForceMode forceMode = ForceMode.Impulse;

    private Rigidbody rb;

    private Camera cam;
    CapsuleCollider capsuleCollider;

    private EventHandler jumpReceived;
    public event EventHandler JumpReceived
    {
        add { jumpReceived += value; }
        remove { jumpReceived -= value; }
    }

    private PlayerHealth playerHealth;

    private Vector3 originPos;
    private EnemyHealth enemyHealth;
    private NavMeshAgent agent;
    private GameObject target;
    
    void Start()
    {
        originPos = transform.position;
        enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.DeadReceived += OnDead;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.stoppingDistance = closeAttackDistance;

        fire1Cooldown = UnityEngine.Random.Range(FireRandomInterval.x, FireRandomInterval.y);
    }

    void Update()
    {
        if(target)
        {
            OnMove();
            OnFire();
        }
        else
        {
            if(Vector3.Distance(transform.position, originPos) >= maxMoveDistance || target == null)
            {
                agent.SetDestination(originPos);
            }
        }
    }

    private void OnMove()
    {
        agent.SetDestination(target.transform.position);
        deltaPos = agent.velocity;
    }

    private void OnFire()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if(distance <= closeAttackDistance)
        {
            Quaternion targetRot = Quaternion.LookRotation(target.transform.position - agent.transform.position);
            targetRot = targetRot.normalized;
            targetRot.x = targetRot.z = 0;
            Quaternion deltaRot = Quaternion.Slerp(agent.transform.rotation, targetRot, agent.speed * Time.deltaTime);
            if (deltaRot.eulerAngles.magnitude != 0) agent.transform.rotation = targetRot;

            if(fire1Cooldown <= 0)
            {
                FireEventArgs fireEventArgs = new FireEventArgs(Fire1Damage, Fire1Force);
                fire1Received?.Invoke(this, fireEventArgs);
                fire1Cooldown = UnityEngine.Random.Range(FireRandomInterval.x, FireRandomInterval.y);
            }
            if(fire1Cooldown > 0f)
            {
                fire1Cooldown -= Time.deltaTime;
            }
        }
    }

    private void OnDead(object sender, EventArgs e)
    {
        agent.isStopped = true;
        enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
            if (playerHealth.IsDead) target = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }
}
