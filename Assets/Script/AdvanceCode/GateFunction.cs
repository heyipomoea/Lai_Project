//using BasicCode;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace AdvanceCode
{
    public class GateFunction : MonoBehaviour
    {
        [SerializeField] private GameObject gate;
        private List<Health_Enemy> enemyHealths = new List<Health_Enemy>();
        private int clearConditionAmount = 0;
        private int maxCount = 0;

        private Vector3 targetPos;
        private bool isTrigger = false;

        private float lerp = 0.01f;
        private float AfterDestoryTime = 3f;

        private void OnTriggerEnter(Collider other)
        {
            Health_Enemy health_Enemy = other.GetComponent<Health_Enemy>();
            if (health_Enemy)
            {
                enemyHealths.Add(health_Enemy);
                health_Enemy.DeadReceived += OnDead;
                maxCount++;
            }
        }

        void Start()
        {
            var size = gate.GetComponent<BoxCollider>().size;
            targetPos = gate.transform.position + (Vector3.up * -size.y);
        }

        private void OnDead(object sender, EventArgs e)
        {
            clearConditionAmount += 1;
            if (clearConditionAmount >= maxCount)
            {
                isTrigger = true;
                StartCoroutine(AfterDestory());
            }
        }

        private void Update()
        {
            if (isTrigger)
            {
                gate.transform.position = Vector3.Lerp(gate.transform.position, targetPos, lerp);
            }
        }

        IEnumerator AfterDestory()
        {
            yield return new WaitForSeconds(AfterDestoryTime);
            Destroy(gameObject);
        }
    }
}

