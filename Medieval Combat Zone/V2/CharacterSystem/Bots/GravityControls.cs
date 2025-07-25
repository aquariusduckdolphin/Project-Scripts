using UnityEngine;

namespace CombatZone.Character.Bot
{

    public class GravityControls : MonoBehaviour
    {

        [Header("Gravity Control SettingsD")]
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float distance = 2f;
        private Rigidbody rb;

        /**************** Start, Update, Etc. ****************/

        #region Start
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        #endregion

        #region Update
        void Update()
        {
            if (Physics.Raycast(transform.position, -transform.up, distance, collisionMask))
            {
                rb.useGravity = false;
            }
            else
            {
                rb.useGravity = true;
            }
        }
        #endregion

    }


}

