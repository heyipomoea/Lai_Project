using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public abstract class TriggerVolumeBase : MonoBehaviour
    {
        [SerializeField] private string compareTag = "Player";

        protected virtual void OnTriggerEnter(Collider other) 
        {
            if (other.CompareTag(compareTag))
            {
                ExecTriggerEnter(other);
                Destroy(gameObject);
            }
        }

        protected abstract void ExecTriggerEnter(Collider other);
    }
}

