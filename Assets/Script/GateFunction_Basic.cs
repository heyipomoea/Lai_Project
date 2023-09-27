using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateFunction_Basic : MonoBehaviour
{
    public List<EnemyHealth> enemyHealths;
    private int clearConditionAmount = 0;
    private int maxCount = 0;

    private Vector3 targetPos;
    private bool isTrigger = false;

    private float lerp = 0.01f;
    private float AfterDestoryTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        var size = GetComponent<BoxCollider>().size;
        targetPos = transform.position + (Vector3.up * -size.y);

        maxCount = enemyHealths.Count;

        foreach(EnemyHealth health in enemyHealths)
        {
            health.DeadReceived += OnDead;
        }
        
    }

    private void OnDead(object sender, EventArgs e)
    {
        clearConditionAmount += 1;
        if(clearConditionAmount >= maxCount)
        {
            isTrigger = true;
            StartCoroutine(AfterDestory());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isTrigger)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, lerp);
        }
    }

    IEnumerator AfterDestory()
    {
        yield return new WaitForSeconds(AfterDestoryTime);
        Destroy(gameObject);
    }
}
