using System.Collections;
using UnityEngine;
using CombatZone.Ragdolling;
using CombatZone.Damage;

namespace CombatZone.Player
{

    public class PlayerHealth : MonoBehaviour, IDamage
    {

        #region Varables

        #region Health Setting
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;

        [SerializeField] private float currentHealth;
        #endregion

        #region Death Setting
        [Header("Death")]
        [SerializeField] private GameObject[] cams = new GameObject[2];

        [SerializeField] private Transform respawnPoint;
        #endregion

        #region Ragdoll Setting
        [Header("Ragdoll")]
        [SerializeField] private GameObject characterMesh;

        [SerializeField] private Ragdoll ragdoll;
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Start
        void Start()
        {

            currentHealth = maxHealth;

            SetCameras(true);

            //Respawn();

        }
        #endregion

        #region Other Functions

        #region TakeDamage
        public void TakeDamage(float damage)
        {

            if (currentHealth <= 0)
            {

                currentHealth = 0;

                //Need to change to make it ragdoll and wait before respawning.

                //wizardMesh.SetActive(true);

                //ragdoll.ActivateRagdoll();

                StartCoroutine(SetDeathCams());

            }

            currentHealth -= damage;

        }
        #endregion

        #region SetDeathCams
        private IEnumerator SetDeathCams()
        {

            SetCameras(false);

            yield return new WaitForSeconds(10f);

            currentHealth = maxHealth;

            Respawn();

            SetCameras(true);

            //wizardMesh.SetActive(false);

            //ragdoll.DeactivateRagdooll();

        }
        #endregion

        #region Respawn
        private void Respawn()
        {

            transform.position = respawnPoint.transform.position;

        }
        #endregion

        #region SetCameras
        private void SetCameras(bool value)
        {

            for (int i = 0; i < cams.Length; i++)
            {

                if (cams[i] == null) { return; }

            }

            cams[0].SetActive(value);

            cams[1].SetActive(!value);

        }
        #endregion

        #endregion

    }

}