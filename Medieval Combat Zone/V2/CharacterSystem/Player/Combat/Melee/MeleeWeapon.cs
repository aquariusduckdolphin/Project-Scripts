using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Bullet;

namespace CombatZone.Character.Player
{

    public class MeleeWeapon : BaseBullet, IBullet, IMeleeWeapon
    {

        #region Varables
        [Header("Melee Weapon Properties")]

        [SerializeField] private int meleeWeaponElementType = 0;
        private int currentStartEffectIndex = 0;

        [Header("Weapon Color")]
        public Color[] weaponColor;

        [Header("Animation & Collider")]
        public MeshRenderer meshRender;
        public BoxCollider swordTrigger;
        public Animator anim;
        private const string attacking = "Attacking";

        [Header("Melee Weapon Element Property")]
        public IBullet.ElementType elementType
        {

            get => elementType;
            private set => elementType = value;

        }

        #endregion

        //////////////////////////////////////////////////////

        #region Input Action System

        #region nextElement
        public void nextElement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                meleeWeaponElementType++;
                if (meleeWeaponElementType > 3) { meleeWeaponElementType = 0; }
                ChangeMeleeWeaponProperties("Next Element");

            }
        }
        #endregion

        #region previousElement
        public void previousElement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                meleeWeaponElementType--;
                if (meleeWeaponElementType < 0) { meleeWeaponElementType = 3; }
                ChangeMeleeWeaponProperties("Previous Element");

            }
        }
        #endregion

        #region PrimaryAttack
        public void PrimaryAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                elementType = (IBullet.ElementType)meleeWeaponElementType;
                SwordSwing(attacking, true);

            }

            else { SwordSwing(attacking, false); }

        }
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Start, Update, Etc

        #region Start
        void Awake()
        {
            SetMaterialColor(meleeWeaponElementType);
        }
        #endregion

        #region OnTriggerEnter
        private void OnTriggerEnter(Collider other)
        {
            HandleBulletCollision(other, currentStartEffectIndex);
            ApplyDamageToPlayer(other);
        }
        #endregion

        #endregion

        #region Change Melee Weapon Properties
        private void ChangeMeleeWeaponProperties(string action = "Action")
        {
            UpdateMeleeWeaponProperties(meleeWeaponElementType);
            SetMaterialColor(meleeWeaponElementType);
            Debug.Log(action);
        }
        #endregion

        #region SetMeleeWeaponImpactEffect
        private void UpdateMeleeWeaponProperties(int weaponTypeNum)
        {
            switch (weaponTypeNum)
            {
                case 0:
                    currentStartEffectIndex = 0;
                    break;
                case 1:
                    currentStartEffectIndex = 4;
                    break;
                case 2:
                    currentStartEffectIndex = 8;
                    break;
                case 3:
                    currentStartEffectIndex = 11;
                    break;
            }
        }
        #endregion

        #region SetMaterialColor
        private void SetMaterialColor(int currentMaterialColor)
        {
            meshRender.material.SetColor("_StaffColor", weaponColor[currentMaterialColor]);
        }
        #endregion

        //////////////////////////////////////////////////////

        #region Animations

        #region Sword Idle
        public void SwordIdle(string attacking, bool attackingState)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Swing Sword
        public void SwordSwing(string attacking, bool attackingState)
        {
            anim.SetBool(attacking, attackingState);
            swordTrigger.enabled = attackingState;
            Debug.Log(attacking);
        }
        #endregion

        #endregion

    }

}