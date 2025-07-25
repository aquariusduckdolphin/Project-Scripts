
using UnityEngine;

namespace CombatZone.Character
{
    public class CharacterAnimations : ScriptableObject
    {
        [Header("Movement")]
        public readonly int verticalMovement = Animator.StringToHash("Vertical");
        public readonly int horizontalMovement = Animator.StringToHash("Horizontal");

        [Header("Other")]
        public readonly int attack = Animator.StringToHash("Attack");
        public readonly int hitReaction = Animator.StringToHash("Hit");
        public readonly int crouch = Animator.StringToHash("Crouch");
        public readonly int jump = Animator.StringToHash("Jumping");

        public void Idle(Animator animator)
        {
            animator.SetFloat(verticalMovement, 0f);
            animator.SetFloat(horizontalMovement, 0f);
            animator.SetBool(attack, false);
            animator.SetBool(hitReaction, false);
            animator.SetBool(crouch, false);
            animator.SetBool(jump, false);
        }

        public void SetMovement(Animator animator, int movementDirection, float direction)
        {
            Idle(animator);
            animator.SetFloat(movementDirection, direction);
        }

        public void SetOtherActions(Animator animator, int action, bool preformAction)
        {
            animator.SetBool(action, preformAction);
        }
    }
}
