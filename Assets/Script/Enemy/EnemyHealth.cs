using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private GameManager_Basic gameManager;
    public int Score = 1;
    [SerializeField] private GameObject ref_FX;
    [SerializeField] private float maxHP = 10f;
    public float MaxHP => maxHP;
    private float hp;
    public float CurrentHP => hp;
    private float AfterDeadTime = 3f;
    private bool isDead = false;
    public bool IsDead => isDead;

    private EventHandler deadReceived;
    public event EventHandler DeadReceived
    {
        add { deadReceived += value; }
        remove { deadReceived -= value; }
    }

    private EventHandler hitReceived;
    public event EventHandler HitReceived
    {
        add { hitReceived += value; }
        remove { hitReceived -= value; }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        hp = MaxHP;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager_Basic>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnDamage(2);
        }
    }

    public void OnDamage(float damage)
    {
        hitReceived?.Invoke(this, EventArgs.Empty);
        hp -= damage;
        if (hp <= 0 && !isDead) { OnDead(); }
    }

    public void OnHealing(float heal)
    {
        hp += heal;
        if (hp >= MaxHP) { hp = MaxHP; }
    }

    void OnDead()
    {
        isDead = true;

        deadReceived?.Invoke(this, EventArgs.Empty);
        enabled = false;
        StartCoroutine(AfterDead());
        gameManager.AddScore(1);
    }

    IEnumerator AfterDead()
    {
        yield return new WaitForSeconds(AfterDeadTime);
        GameObject tempFx = Instantiate(ref_FX, transform.position, Quaternion.identity);
        tempFx.AddComponent<ParticleEffectController>();
        Destroy(gameObject);
    }
}
