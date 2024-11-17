using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Abilities
{

    public class AbilityTest : BaseAbilities
    {

        public GameObject go;

        private void Awake()
        {

            CreateNewControls();

        }

        private void OnDisable()
        {

            playerControls.Player.Ultimate.performed -= OnUltimate;

            playerControls.Player.Ability.performed -= OnAbilityOne;

            playerControls.Player.Ability2.performed -= OnAbilityTwo;

            playerControls.Disable();

        }

        private void Update()
        {

            SettingAbilities();

        }

        public override void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCoolDownTimer > 0) { return; }
            else { ultimateCoolDownTimer = ultimateCoolDown; }

            ultimate.fillAmount = 0;

            Instantiate(go, transform.position, Quaternion.identity);

        }

        public override void OnAbilityOne(InputAction.CallbackContext context)
        {

            if (abilityCoolDownTimer > 0) { return; }
            else { abilityCoolDownTimer = abilityCoolDown; }

            ability.fillAmount = 0;

        }

        public override void OnAbilityTwo(InputAction.CallbackContext context)
        {

            if (ability2CoolDownTimer > 0) { return; }
            else { ability2CoolDownTimer = abiliity2CoolDown; }

            ability2.fillAmount = 0;

        }

    }


}