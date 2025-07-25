using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Player;


namespace CombatZone.Abilities
{

    public class KnightAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject divinePower;

        public float divinePowerDuration = 5f;

        [Header("Ability 1")]
        public GameObject leadership;

        public float leadershipDuration = 10f;

        [Header("Ability 2")]
        public GameObject equineSwiftness;

        private float equineSpeedDuration;

        private float maxEquineSpeedDuration;

        public float equineSpeed = 20f;

        private GameObject swiftness;

        [Header("References")]
        private PlayerMovementAdvanced movement;

        void Start()
        {

            CreateNewControls();

            equineSpeedDuration = ultimateCoolDownTimer;

            maxEquineSpeedDuration = ultimateCoolDown;

            movement = GetComponent<PlayerMovementAdvanced>();

        }

        private void Update()
        {

            SettingAbilities();

            AbilityTimer(ref equineSpeedDuration);

            if (equineSpeedDuration <= 0f)
            {

                Destroy(swiftness);

                movement.walkSpeed = movement.defaultWalkSpeed;

                movement.sprintSpeed = movement.defaultSprintSpeed;

            }

        }

        public override void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCoolDownTimer > 0) { return; }
            else { ultimateCoolDownTimer = ultimateCoolDown; }

            ultimate.fillAmount = 0;

            GameObject divinePowerKnight = Instantiate(divinePower, transform.position, Quaternion.identity);

            Destroy(divinePowerKnight, divinePowerDuration);

        }

        public override void OnAbilityOne(InputAction.CallbackContext context)
        {

            if (abilityCoolDownTimer > 0) { return; }
            else { abilityCoolDownTimer = abilityCoolDown; }

            ability.fillAmount = 0;

            GameObject leadershipPower = Instantiate(leadership, transform.position, Quaternion.identity);

            Destroy(leadershipPower, leadershipDuration);

        }

        public override void OnAbilityTwo(InputAction.CallbackContext context)
        {

            if (ability2CoolDownTimer > 0) { return; }
            else
            {

                ability2CoolDownTimer = abiliity2CoolDown;

                equineSpeedDuration = maxEquineSpeedDuration;

            }

            ability2.fillAmount = 0;

            Destroy(swiftness);

            swiftness = Instantiate(equineSwiftness, transform.position, Quaternion.identity);

            movement.walkSpeed = equineSpeed;

            movement.sprintSpeed = equineSpeed;

        }

    }


}