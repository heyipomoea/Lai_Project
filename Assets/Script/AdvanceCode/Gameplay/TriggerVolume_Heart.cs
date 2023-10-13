using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode 
{
    public class TriggerVolume_Heart : TriggerVolumeBase
    {
        [SerializeField] public int HealAmount = 1;
        protected override void ExecTriggerEnter(Collider other)
        {
            Health_Player playerHealth = other.GetComponent<Health_Player>();
            playerHealth.OnHealing(HealAmount);
        }
    }
}

