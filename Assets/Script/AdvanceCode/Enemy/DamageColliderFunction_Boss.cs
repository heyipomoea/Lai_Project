using AdvanceCode;
using System;
using UnityEngine;


namespace Heyipomoea
{
    public class DamageColliderFunction_Boss : DamageColliderFunction_Enemy
    {
        [SerializeField] private GameObject ref_BoomFX;
        [SerializeField] private Transform anchor_Boom;
        [SerializeField] private Vector3 ScaleFX = new Vector3(5, 5, 5);
        [SerializeField] private float BoomRadius = 3f;
        [SerializeField] private float BoomForce = 10f;

        protected override void OnTriggerEnter(Collider other) {}


        protected override void OnEndAttackAnimation(object sender, EventArgs e)
        {
            base.OnEndAttackAnimation(sender, e);

            GameObject tempFx = Instantiate(ref_BoomFX, anchor_Boom.position, Quaternion.identity);
            tempFx.transform.localScale = ScaleFX;
            tempFx.AddComponent<ParticleEffectController>();

            Collider[] cols = Physics.OverlapSphere(anchor_Boom.position, BoomRadius);
            foreach (Collider col in cols)
            {
                if(!col.isTrigger && !col.CompareTag(selfTag))
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();
                    if(rb)
                    {
                        Vector3 direction = rb.transform.position - transform.position;
                        rb.AddForceAtPosition(direction * BoomForce, transform.position, ForceMode.Impulse);
                        OnTakeDamage(col);
                    }
                }
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(anchor_Boom.position, BoomRadius);
        }
    }
}

