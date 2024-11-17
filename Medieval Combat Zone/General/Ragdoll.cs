using UnityEngine;

namespace CombatZone.Ragdolling
{

    public class Ragdoll : MonoBehaviour
    {

        [Tooltip("Set this to true of there is a collider on the parent.")]
        [SerializeField] private bool excludeParentCollider = false;

        [Tooltip("Read-Only. This will indate if the ragdoll is on or off.")]
        [SerializeField] private bool isRagdollActive;

        [SerializeField] private Animator characterAnimator;

        [SerializeField] private Rigidbody[] allRigidbodies;

        [SerializeField] private Collider[] allColliders;

        #region Gather Info
        void Start()
        {

            allRigidbodies = GetComponentsInChildren<Rigidbody>();

            allColliders = GetComponentsInChildren<Collider>();

            characterAnimator = GetComponent<Animator>();

            if (characterAnimator == null) { characterAnimator = GetComponentInChildren<Animator>(); }

            //Set the ragdoll to be off
            ToggleRagdoll(false);

        }
        #endregion

        #region Ragdolling Effect
        public void ToggleRagdoll(bool isActiveRagdoll)
        {

            isRagdollActive = isActiveRagdoll;

            int maxLength = Mathf.Max(allColliders.Length, allRigidbodies.Length);

            for (int i = 0; i < maxLength; i++)
            {

                if (excludeParentCollider && i == 0) { continue; }

                allRigidbodies[i].isKinematic = !isActiveRagdoll;

                allColliders[i].enabled = isActiveRagdoll;

            }

            if (characterAnimator != null) { characterAnimator.enabled = !isActiveRagdoll; }

        }
        #endregion

    }

}