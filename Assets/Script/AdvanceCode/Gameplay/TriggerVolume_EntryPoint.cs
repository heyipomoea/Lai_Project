using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvanceCode 
{
    public class TriggerVolume_EntryPoint : TriggerVolumeBase
    {
        protected override void ExecTriggerEnter(Collider other)
        {
            GameManager.Instance.SetRevivalPos(transform.position);
        }
    }
}

