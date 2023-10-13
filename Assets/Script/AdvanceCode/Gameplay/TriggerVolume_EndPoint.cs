using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode 
{
    public class TriggerVolume_EndPoint : TriggerVolumeBase
    {
        protected override void ExecTriggerEnter(Collider other)
        {
            GameManager.Instance.GameOver();
        }
    }
}

