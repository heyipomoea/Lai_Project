using AdvanceCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public class Health_Enemy : HealthBase 
    {
        [SerializeField] private int Score = 1;
        protected override IEnumerator AfterDead()
        {
            GameManager.Instance.AddScore(Score);
            yield return base.AfterDead();
            Destroy(gameObject);
        }
    }
}

