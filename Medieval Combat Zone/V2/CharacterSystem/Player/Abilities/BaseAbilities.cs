using CombatZone.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CombatZone.Character.Abilities
{

    [System.Serializable]
    public struct Ability
    {
        public string abilityName;
        public Image ultimateAbilityIcon;
        public float ultimateCooldownDuration;
        public float ultimateCooldownTimer;

        public void SetUltimateCooldown()
        {
            ultimateCooldownDuration = 2f;
        }

    }

    public abstract class BaseAbilities : Duration
    {

        /************************** Variables **************************/

        [Header("Ultimate")]
        [SerializeField] protected Image ultimateAbilityIcon;
        [SerializeField] protected float ultimateCooldownDuration = 2f;
        [SerializeField] protected float ultimateCooldownTimer;

        [Space]

        [Header("Primary Ability")]
        [SerializeField] protected Image primaryAbilityIcon;
        [SerializeField] protected float primaryAbilityCooldownDuration = 2f;
        [SerializeField] protected float primaryCooldownTimer;

        [Space]

        [Header("Secondary Ability")]
        [SerializeField] protected Image secondaryAbilityIcon;
        [SerializeField] protected float secondaryAbilityCooldownDuration = 2f;
        [SerializeField] protected float secondaryCooldownTimer;

        [Space]

        [Header("Controls")]
        protected PlayerControls playerControls;

        [Header("Audio Properties")]
        [SerializeField] private AudioClip clip;
        [SerializeField] private AudioSource audioSource;
        [SerializeField, Range(0f, 1f)] private float volume = 1f;

        /************************** Start, Update, Etc. **************************/

        #region Awake
        protected void Awake() 
        {
            playerControls = new PlayerControls();
            EnableAbilityControls(); 
        }
        #endregion

        #region On Disable
        protected void OnDisable()
        {
            DisableAbilityControls();
        }
        #endregion

        #region Update
        protected void Update()
        {
            UpdateAbilityCooldowns();
        }
        #endregion

        /************************** Input Actions **************************/

        #region Create A New Control Instance
        public void EnableAbilityControls()
        {
            playerControls.Enable();
            playerControls.Abilities.Ultimate.performed += OnUltimate;
            playerControls.Abilities.Ability.performed += PrimaryAbility;
            playerControls.Abilities.Ability2.performed += SecondaryAbility;
        }
        #endregion

        #region Disable Ability Controls
        public void DisableAbilityControls()
        {
            playerControls.Abilities.Ultimate.performed -= OnUltimate;
            playerControls.Abilities.Ability.performed -= PrimaryAbility;
            playerControls.Abilities.Ability2.performed -= SecondaryAbility;
            playerControls.Disable();
        }
        #endregion

        #region Ultimate
        protected abstract void OnUltimate(InputAction.CallbackContext context);
        
        protected void Ultimate(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                TriggerAbility(ref ultimateCooldownTimer, ultimateCooldownDuration, ref ultimateAbilityIcon);
                Debug.Log("Ultimate");
            }

        }
        #endregion

        #region Primary Ability
        protected abstract void OnPrimaryAbility(InputAction.CallbackContext context);

        protected void PrimaryAbility(InputAction.CallbackContext context) 
        {

            if (context.performed)
            {
                TriggerAbility(ref primaryCooldownTimer, primaryAbilityCooldownDuration, ref primaryAbilityIcon);
                Debug.Log("Primary TriggerAbility");
            }

        }
        #endregion

        #region Secondary Ability
        protected abstract void OnSecondaryAbility(InputAction.CallbackContext context);

        public void SecondaryAbility(InputAction.CallbackContext context) 
        {

            if (context.performed)
            {
                TriggerAbility(ref secondaryCooldownTimer, secondaryAbilityCooldownDuration, ref secondaryAbilityIcon);
                Debug.Log("Primary TriggerAbility");
            }

        }
        #endregion

        /************************** Cooldown Management **************************/

        #region Trigger Ability
        protected void TriggerAbility(ref float coolDownTimer, float cooldownDuration,  ref Image abilityIcon)
        {
            coolDownTimer = cooldownDuration;
            abilityIcon.fillAmount = 0;
        }
        #endregion

        #region Update Ability Cooldown
        public void UpdateAbilityCooldowns()
        {
            UpdateAbilityCooldown(ref ultimateCooldownTimer, ultimateCooldownDuration, ultimateAbilityIcon);
            UpdateAbilityCooldown(ref primaryCooldownTimer, primaryAbilityCooldownDuration, primaryAbilityIcon);
            UpdateAbilityCooldown(ref secondaryCooldownTimer, primaryAbilityCooldownDuration, secondaryAbilityIcon);
        }
        #endregion

        #region Update Cooldown
        protected override void UpdateAbilityCooldown(ref float coolDownTimer, float cooldownDuration, Image abilityIcon)
        {
            base.UpdateAbilityCooldown(ref coolDownTimer, cooldownDuration, abilityIcon);
        }
        #endregion

        /************************** Instantiate **************************/

        #region Spawn Effect
        protected void SpawnEffect(GameObject effectPrefab, Vector3 position, Quaternion rotation, float duration = 0f)
        {
            GameObject effect = Instantiate(effectPrefab, position, rotation);
            Destroy(effect, duration);
        }
        #endregion


        protected void PlayAudioEffect()
        {
            audioSource = GetComponent<AudioSource>();
            AudioUtility.PlayOnce(audioSource, clip, volume);
        }

    }

}