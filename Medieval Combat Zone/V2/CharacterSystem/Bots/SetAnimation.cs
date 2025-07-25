using CombatZone.Character.Bot;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace CombatZone
{
    [CreateAssetMenu(fileName = "Bot Animations", menuName = "Bots/Animations")]
    public class SetAnimation : ScriptableObject 
    {

        public readonly int ForwardWalk = Animator.StringToHash("Vertical");
        public readonly int SideWalk = Animator.StringToHash("Horizontal");
        public readonly int Attacking = Animator.StringToHash("Attack");
        public readonly int HitReaction = Animator.StringToHash("Hit");
        public readonly int Crouching = Animator.StringToHash("Crouch");
        public readonly int Jump = Animator.StringToHash("Jumping");

        void Start()
        {
            //animator = transform.GetChild(0).gameObject.transform.GetChild(0).transform.GetComponent<Animator>();
        }

        #region Set Walk Speed
        public void SetWalkSpeed(Animator animator, int walk, float walkSpeed = 0f)
        {
            if (animator == null) { return; }
            animator.SetFloat(walk, walkSpeed);
        }
        #endregion

        #region Set Action
        public void SetAction(Animator animator, int action, bool isAttaking = false)
        {
            if (animator == null) { return; }
            animator.SetBool(action, isAttaking);
        }
        #endregion

        #region Disable All Animator
        public void DisableAllAnimations(Animator animator)
        {
            SetWalkSpeed(animator, ForwardWalk);
            SetWalkSpeed(animator, SideWalk);
            SetAction(animator, Attacking);
            SetAction(animator, HitReaction);
            SetAction(animator, Crouching);
            SetAction(animator, Jump);
        }
        #endregion

    }
}
