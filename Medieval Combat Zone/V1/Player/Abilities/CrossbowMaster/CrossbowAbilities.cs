using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Player;

namespace CombatZone.Abilities
{

    public class CrossbowAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public PlayerMovementAdvanced velocityVeil;

        public float velocityVeilDuration;

        [Header("Ability 1")]
        public GameObject shadowSnare;

        public float shadowSnareDuration;

        [Header("Ability 2")]
        public Animator anim;

        public float swiftReload;

        [Header("Right Click")]
        public Transform[] marksmanship;

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