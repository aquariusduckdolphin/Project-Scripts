using System.Collections;
using UnityEngine;
using CombatZone.Ragdolling;
using CombatZone.Utilities;

namespace CombatZone.Character.Player
{

    public class PlayerHealth : BaseHealth
    {

        [Header("Death Properties")]
        [SerializeField] private GameObject[] cams = new GameObject[2];

        [Header("Health Settings")]
        [SerializeField] private Transform respawnPoint;

        /**************** Start, Update, Etc.  ****************/

        #region Start
        void Start()
        {
            ResetHealth();
            SetCameras(true);

            animator = transform.GetAnimator();
            ragdoll = transform.GetChild(1).transform.GetComponent<Ragdoll>();
        }
        #endregion

        #region Update
        private void Update()
        {
            if (isDead) { StartCoroutine(SetDeathCam(delay)); }
        }
        #endregion

        /**************** Cameras ****************/

        #region SetDeathCams
        private IEnumerator SetDeathCam(float delay)
        {
            SetCameras(false);
            StartCoroutine(ResetCharacter(delay));
            yield return new WaitForSeconds(delay);
            ResetHealth();
            //Respawn();
            SetCameras(true);
        }
        #endregion

        #region SetCameras
        private void SetCameras(bool isCameraActive)
        {
            for (int i = 0; i < cams.Length; i++)
            {
                if (cams[i] == null) { return; }
            }

            cams[cams.Length - cams.Length].SetActive(isCameraActive);
            cams[cams.Length - 1].SetActive(!isCameraActive);
        }
        #endregion

        /**************** Respawn ****************/

        #region Respawn
        private void Respawn()
        {
            transform.position = respawnPoint.transform.position;
        }
        #endregion

    }

}