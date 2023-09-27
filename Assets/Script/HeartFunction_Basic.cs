using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFunction_Basic : MonoBehaviour
{
    [SerializeField] public int HealAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.OnHealing(HealAmount);
            Destroy(gameObject);
        }
    }
}
