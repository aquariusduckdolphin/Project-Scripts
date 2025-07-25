using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Abilities
{

    public class WizardAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject enchantments;
        public float enchantmentDuration;
        public int currentEnchantment = 0;

        [Header("TriggerAbility 1")]
        public GameObject divination;
        public float divinationDuration = 10f;

        [Header("TriggerAbility 2")]
        public GameObject alchemy;
        public float alchemyDuration = 10f;

        /************************** Input Actions **************************/

        protected override void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCooldownTimer > 0f) { return; }
            Ultimate(context);
            SpawnEffect(enchantments, transform.position, Quaternion.identity, enchantmentDuration);

        }

        protected override void OnPrimaryAbility(InputAction.CallbackContext context)
        {
            
            if (primaryCooldownTimer > 0) { return; }
            PrimaryAbility(context);
            SpawnEffect(divination, transform.position, Quaternion.identity, divinationDuration);

        }

        protected override void OnSecondaryAbility(InputAction.CallbackContext context)
        {

            if (secondaryCooldownTimer > 0) { return; }
            SecondaryAbility(context);
            SpawnEffect(alchemy, transform.position, Quaternion.identity, divinationDuration);
        }

    }

}