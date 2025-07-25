using UnityEngine;

namespace CombatZone
{
    public class AnimationTest : MonoBehaviour
    {

        public Animator animator;
        
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            animator.SetBool("Attack", true);
            animator.SetFloat("Vertical", 1f);
        }
    }
}