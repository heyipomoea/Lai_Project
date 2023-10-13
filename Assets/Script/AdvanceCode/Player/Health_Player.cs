using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode
{
    public class Health_Player : HealthBase 
    {
        protected override IEnumerator AfterDead()
        {
            yield return base.AfterDead();
            OnRevival();
        }
    }
}

