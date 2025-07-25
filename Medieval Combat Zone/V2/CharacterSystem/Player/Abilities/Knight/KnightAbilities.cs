using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Character.Player;


namespace CombatZone.Character.Abilities
{

    public class KnightAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject divinePower;
        public float divinePowerDuration = 5f;

        [Header("TriggerAbility 1")]
        public GameObject leadership;
        public float leadershipDuration = 10f;

        [Header("TriggerAbility 2")]
        public GameObject equineSwiftness;
        private float equineSpeedDuration;
        private float maxEquineSpeedDuration;
        public float equineSpeed = 20f;
        private GameObject swiftness;

        [Header("References")]
        private PlayerMovement movement;

        /************************** Start, Update, Etc. **************************/

        void Start()
        {
            movement = GetComponent<PlayerMovement>();
            equineSpeedDuration = ultimateCooldownTimer;
            maxEquineSpeedDuration = ultimateCooldownDuration;
        }

        /*private void Update()
        {

            ReduceCooldownTimer(ref equineSpeedDuration);

            if (equineSpeedDuration <= 0f)
            {

                Destroy(swiftness);
                movement.walkSpeed = movement.defaultWalkSpeed;
                movement.sprintSpeed = movement.defaultSprintSpeed;

            }

        }*/

        /************************** Input Actions **************************/

        protected override void OnUltimate(InputAction.CallbackContext context)
        {
            if (ultimateCooldownTimer > 0f) { return; }
            Ultimate(context);
            GameObject divinePowerKnight = Instantiate(divinePower, transform.position, Quaternion.identity);
            Destroy(divinePowerKnight, divinePowerDuration);
        }

        protected override void OnPrimaryAbility(InputAction.CallbackContext context)
        {
            if (primaryCooldownTimer > 0f) { return; }
            PrimaryAbility(context);
            GameObject leadershipPower = Instantiate(leadership, transform.position, Quaternion.identity);
            Destroy(leadershipPower, leadershipDuration);
        }

        protected override void OnSecondaryAbility(InputAction.CallbackContext context)
        {
            if (secondaryCooldownTimer > 0f) { return; }
            SecondaryAbility(context);
            secondaryCooldownTimer = secondaryAbilityCooldownDuration;
            equineSpeedDuration = maxEquineSpeedDuration;
            Destroy(swiftness);
            swiftness = Instantiate(equineSwiftness, transform.position, Quaternion.identity);
            movement.walkSpeed = equineSpeed;
            movement.sprintSpeed = equineSpeed;
        }

    }

}