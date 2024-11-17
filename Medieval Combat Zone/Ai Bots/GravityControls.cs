using UnityEngine;

namespace CombatZone.Bot
{

    public class GravityControls : MonoBehaviour
    {

        #region Variables

        #region Gravity Control Settings
        [Header("Gravity Control SettingsD")]
        private Rigidbody rb;

        public LayerMask mask;
        #endregion

        #endregion

        /***************************************************************/

        #region Start, Update, Etc

        #region Start
        void Start()
        {

            rb = GetComponent<Rigidbody>();

        }
        #endregion

        #region Update
        void Update()
        {

            if (Physics.Raycast(transform.position, -transform.up, 2f, mask))
            {

                rb.useGravity = false;

            }
            else
            {

                rb.useGravity = true;

            }

        }
        #endregion

        #endregion

    }


}

