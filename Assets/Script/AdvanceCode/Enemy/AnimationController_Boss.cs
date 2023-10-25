using AdvanceCode;
using UnityEngine;


namespace Heyipomoea
{
    public class AnimationController_Boss : AnimationController_Enemy
    {
        [SerializeField] private float StartAttackSpeed = 4f;
        [SerializeField] private float EndAttackSpeed = 1f;
        private int attackSpeedHash;

        protected override void Start()
        {
            base.Start();
            attackSpeedHash = Animator.StringToHash("AttackSpeed");
        }

        void StartAction()
        {
            characterMovement_Enemy.SetAttackingMotion(true);
        }

        void EndAction()
        {
            characterMovement_Enemy.SetAttackingMotion(false);
            animator.SetFloat(attackSpeedHash, EndAttackSpeed);
        }

        protected override void StartAttackAnimation()
        {
            base.StartAttackAnimation();
            animator.SetFloat(attackSpeedHash, StartAttackSpeed);
        }
    }
}

