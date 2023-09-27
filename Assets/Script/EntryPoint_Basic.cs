﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint_Basic : MonoBehaviour
{
    private GameManager_Basic gameManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager_Basic>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameManager.SetRevivalPos(transform.position);
            Destroy(gameObject);
        }
    }
}
