using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Abilities
{

    public class WizardAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject enchantments;

        public float enchantmentDuration;

        public int currentEnchantment = 0;

        [Header("Ability 1")]
        public GameObject divination;

        public float divinationDuration = 10f;

        [Header("Ability 2")]
        public GameObject alchemy;

        public float alchemyDuration = 10f;

        void Start()
        {

            CreateNewControls();

        }

        void Update()
        {

            SettingAbilities();

        }

        public override void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCoolDownTimer > 0) { return; }
            else { ultimateCoolDownTimer = ultimateCoolDown; }

            ultimate.fillAmount = 0;

            SpawningObj(enchantments, transform.position, Quaternion.identity, enchantmentDuration);

        }

        public override void OnAbilityOne(InputAction.CallbackContext context)
        {

            if (abilityCoolDownTimer > 0) { return; }
            else { abilityCoolDownTimer = abilityCoolDown; }

            ability.fillAmount = 0;

            SpawningObj(divination, transform.position, Quaternion.identity, divinationDuration);

        }

        public override void OnAbilityTwo(InputAction.CallbackContext context)
        {

            if (ability2CoolDownTimer > 0) { return; }
            else { ability2CoolDownTimer = abiliity2CoolDown; }

            ability2.fillAmount = 0;

            SpawningObj(alchemy, transform.position, Quaternion.identity, divinationDuration);

        }

    }

}