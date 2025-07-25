using UnityEngine;

namespace CombatZone.VisualEffects
{

    public class Slowness : BaseEffects
    {

        [Header("Slowness Properties")]
        [SerializeField] private float playerWalkSpeed = 5f;
        [SerializeField] private float playerSprintSpeed = 10f;
        [SerializeField] private float timeToResetMovementSpeed = 10f;

        [SerializeField] private GameObject mudBall;
        [SerializeField] private GameObject mudStream;
        [SerializeField] private GameObject mudRipple;

        /**************** Start, Update, Etc. ****************/

        #region Update
        void Update()
        {
            bool maxScale;
            maxScale = HasFinishedScalingOvertime(ref currentTime, initalScale, targetScale, growthDuration, transform);
            if (maxScale)
            {
                mudBall.SetActive(false);
                mudStream.SetActive(false);
                mudRipple.SetActive(false);
            }
        }
        #endregion

        #region On Trigger Events
        private void OnTriggerEnter(Collider other)
        {
            ModifyPlayerSpeed(other, playerWalkSpeed, playerSprintSpeed, timeToResetMovementSpeed);
        }

        private void OnTriggerStay(Collider other)
        {
            ModifyPlayerSpeed(other, playerWalkSpeed, playerSprintSpeed, timeToResetMovementSpeed, false);
        }

        private void OnTriggerExit(Collider other)
        {
            ModifyPlayerSpeed(other, playerWalkSpeed, playerSprintSpeed, timeToResetMovementSpeed);
        }
        #endregion

    }

}