using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Abilities
{

    public class AbilityTest : BaseAbilities
    {

        public GameObject go;

        protected override void OnUltimate(InputAction.CallbackContext context)
        {
            if (ultimateCooldownTimer > 0f) { return; }

            Ultimate(context);

            Instantiate(go, transform.position, Quaternion.identity);
        }

        protected override void OnPrimaryAbility(InputAction.CallbackContext context)
        {
            if (primaryCooldownTimer > 0f) { return; }

            PrimaryAbility(context);
        }

        protected override void OnSecondaryAbility(InputAction.CallbackContext context)
        {
            if (secondaryCooldownTimer > 0f) { return; }

            SecondaryAbility(context);
        }

    }

}