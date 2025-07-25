using UnityEngine;

namespace CombatZone.Utilities
{
    public static class AnimatorUtilities
    {
        
        public static Animator GetAnimator(this Transform root)
        {

            Animator animator = root.GetComponent<Animator>();
            if(animator != null) { return animator; }

            animator = root.GetComponentInChildren<Animator>();
            if (animator != null) { return animator; }

            animator = root.root.GetComponent<Animator>();
            if (animator != null) { return animator; }

            animator = root.root.GetComponentInChildren<Animator>();
            if(animator != null) { return animator; }

            return null;

        }

    }

}