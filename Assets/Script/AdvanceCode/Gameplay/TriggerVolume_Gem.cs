using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode 
{
    public class TriggerVolume_Gem : TriggerVolumeBase
    {
        protected override void ExecTriggerEnter(Collider other)
        {
            GameManager.Instance.AddGems();
        }
    }
}

