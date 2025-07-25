using CombatZone.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Abilities
{

    public class CrossbowAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public PlayerMovement velocityVeil;
        public float velocityVeilDuration;

        [Header("TriggerAbility 1")]
        public GameObject shadowSnare;
        public float shadowSnareDuration;

        [Header("TriggerAbility 2")]
        public Animator anim;
        public float swiftReload;

        [Header("Right Click")]
        public Transform[] marksmanship;

        /************************** Input Actions **************************/

        protected override void OnUltimate(InputAction.CallbackContext context)
        {
            if (ultimateCooldownTimer > 0f) { return; }
            Ultimate(context);
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