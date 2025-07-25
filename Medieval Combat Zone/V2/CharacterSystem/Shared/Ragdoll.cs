using UnityEngine;

namespace CombatZone.Ragdolling
{

    public class Ragdoll : MonoBehaviour
    {

        [Tooltip("Set this to true if the script is on the character model.")]
        [SerializeField] private bool isOnModel;
        
        [Tooltip("Set this to true of there is a collider on the transform.")]
        [SerializeField] private bool excludeParentCollider = false;

        [Tooltip("Read-Only. This will indate if the ragdoll is on or off.")]
        [SerializeField] private bool isRagdollActive;

        [SerializeField] private Animator characterAnimator;

        [SerializeField] private Rigidbody[] allRigidbodies;
        [SerializeField] private Collider[] allColliders;

        [SerializeField] private float explosiveForce = 10f;

        /**************** Start, Update, Etc. ****************/

        #region Gather Info
        void Start()
        {
            if (isOnModel)
            {
                InitializeRagdoll();
                ToggleRagdoll(false);
                return;
            }
        }
        #endregion

        #region Initialize Ragdoll
        private void InitializeRagdoll()
        {
            characterAnimator = GetComponent<Animator>();
            if (characterAnimator == null) { characterAnimator = GetComponentInChildren<Animator>(); }
            GatherComponents();
            isRagdollActive = false;
        }
        #endregion

        /**************** Set Ragdoll State ****************/

        #region Gather Components
        public void GatherComponents()
        {
            allRigidbodies = GetComponentsInChildren<Rigidbody>();
            allColliders = GetComponentsInChildren<Collider>();
        }
        #endregion

        #region Ragdolling Effect
        public void ToggleRagdoll(bool isActiveRagdoll)
        {

            isRagdollActive = isActiveRagdoll;
            int minLength = Mathf.Min(allColliders.Length, allRigidbodies.Length);

            for (int i = 0; i < minLength; i++)
            {
                if (excludeParentCollider && i == 0) { continue; }
                allRigidbodies[i].isKinematic = !isActiveRagdoll;
                allColliders[i].enabled = isActiveRagdoll;
                if (isActiveRagdoll)
                {
                    allRigidbodies[i].AddExplosionForce(explosiveForce, transform.position, explosiveForce);
                }
            }

            if (characterAnimator != null) { characterAnimator.enabled = !isActiveRagdoll; }
        }
        #endregion

    }

}