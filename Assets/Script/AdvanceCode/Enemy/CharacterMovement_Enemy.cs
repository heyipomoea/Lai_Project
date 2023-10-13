//using BasicCode;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AdvanceCode
{
    [RequireComponent(typeof(Health_Enemy), typeof(NavMeshAgent))]
    public class CharacterMovement_Enemy : CharacterMovementBase
    {
        [Header("怪物移動參數")]
        [SerializeField] private float maxMoveDistance = 10f;

        [Header("怪物攻擊參數")]
        [SerializeField] private float closeAttackDistance = 2.0f;
        [SerializeField] private Vector2 fireRandomInterval = new Vector2(1f, 2f);

        private string triggerEnterTag = "Player";
        private bool inAttackRange = false;
        private bool isAttacking = false;
        public bool IsAttacking => isAttacking;
        private Vector3 originPos;
        private NavMeshAgent agent;
        private GameObject target;

        protected override void Start()
        {
            base.Start();
            originPos = transform.position;

            agent = GetComponent<NavMeshAgent>();
            agent.speed = moveSpeed;
            agent.stoppingDistance = closeAttackDistance;
            SetCoolDown();
        }

        protected override void OnMove()
        {
            if (target)
            {
                agent.SetDestination(target.transform.position);
            }
            else
            {
                if (Vector3.Distance(transform.position, originPos) >= maxMoveDistance)
                {
                    agent.SetDestination(originPos);
                }
            }
            deltaPos = agent.velocity;
        }

        protected override void OnRotation()
        {
            if (target && !isAttacking)
            {
                Quaternion targetRot = Quaternion.LookRotation(target.transform.position - agent.transform.position);
                targetRot = targetRot.normalized;
                targetRot.x = targetRot.z = 0;
                Quaternion deltaRot = Quaternion.Slerp(agent.transform.rotation, targetRot, RotationSpeed * Time.deltaTime);
                if (deltaRot.eulerAngles.magnitude != 0) agent.transform.rotation = deltaRot;
            }
        }

        public void SetAttackingMotion(bool value)
        {
            agent.isStopped = isAttacking = value;
        }

        protected override void OnFire()
        {
            if (target)
            {
                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance <= closeAttackDistance)
                {
                    inAttackRange = true;
                    if (fire1Cooldown <= 0) { Fire1(); }
                }
                else inAttackRange = false;
            }
        }

        protected override void Fire1()
        {
            FireEventArgs fireEventArgs = new FireEventArgs(Fire1Damage, Fire1Force);
            fire1Received?.Invoke(this, fireEventArgs);
            SetCoolDown();
        }

        protected virtual void SetCoolDown()
        {
            fire1Cooldown = UnityEngine.Random.Range(fireRandomInterval.x, fireRandomInterval.y);
        }

        protected override void OnCoolDown()
        {
            if(!isAttacking && inAttackRange) base.OnCoolDown();
        }

        protected override void OnDead(object sender, EventArgs e)
        {
            base.OnDead(sender, e);
            agent.isStopped = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(triggerEnterTag))
            {
                target = other.gameObject;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(triggerEnterTag))
            {
                target = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(triggerEnterTag))
            {
                target = null;
            }
        }
    }

}

