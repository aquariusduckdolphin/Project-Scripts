using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatZone.Abilities
{

    public abstract class BaseAbilities : MonoBehaviour
    {

        #region Varaibles

        [Header("Ultimate")]
        public Image ultimate;

        public float ultimateCoolDown;

        [HideInInspector] public float ultimateCoolDownTimer;

        [Header("Ability One")]
        public Image ability;

        public float abilityCoolDown;

        [HideInInspector] public float abilityCoolDownTimer;

        [Header("Ability Two")]
        public Image ability2;

        public float abiliity2CoolDown;

        [HideInInspector] public float ability2CoolDownTimer;

        [Header("Controls")]
        public PlayerControls playerControls;

        #endregion

        /***************************************************************/

        #region Create A New Control Instance
        public void CreateNewControls()
        {

            playerControls = new PlayerControls();

            playerControls.Enable();

            playerControls.Player.Ultimate.performed += OnUltimate;

            playerControls.Player.Ability.performed += OnAbilityOne;

            playerControls.Player.Ability2.performed += OnAbilityTwo;


        }
        #endregion

        #region Base Ultimate Function
        public virtual void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCoolDownTimer > 0) { return; }
            else { ultimateCoolDownTimer = ultimateCoolDown; }

            ultimate.fillAmount = 0;

        }
        #endregion

        #region Base Ability One Function
        public virtual void OnAbilityOne(InputAction.CallbackContext context)
        {

            if (abilityCoolDownTimer > 0) { return; }
            else { abilityCoolDownTimer = abilityCoolDown; }

            ability.fillAmount = 0;

        }
        #endregion

        #region Base Ability Two Function
        public virtual void OnAbilityTwo(InputAction.CallbackContext context)
        {

            if (ability2CoolDownTimer > 0) { return; }
            else { ability2CoolDownTimer = abiliity2CoolDown; }

            ability2.fillAmount = 0;

        }
        #endregion

        #region Tick Down the Timer for Abilitites
        public void SettingAbilities()
        {

            AbilityTimer(ref ultimateCoolDownTimer, ultimateCoolDown, ultimate);

            AbilityTimer(ref abilityCoolDownTimer, abilityCoolDown, ability);

            AbilityTimer(ref ability2CoolDownTimer, abilityCoolDown, ability2);

        }

        public void AbilityTimer(ref float coolDownTimer, float coolDown, Image abilityImage)
        {

            if (coolDownTimer > 0)
            {

                coolDownTimer -= Time.deltaTime;

                abilityImage.fillAmount = coolDownTimer / coolDown;

            }
            else
            {

                abilityImage.fillAmount = 1f;

            }

        }

        public void AbilityTimer(ref float coolDownTimer)
        {

            if (coolDownTimer > 0)
            {

                coolDownTimer -= Time.deltaTime;

            }

        }
        #endregion

        public void SpawningObj(GameObject go, Vector3 position, Quaternion rotation, float duration)
        {

            GameObject effect = Instantiate(go, position, rotation);

            Destroy(effect, duration);

        }

    }

}