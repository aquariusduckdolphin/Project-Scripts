using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Bullet;

namespace CombatZone.Player
{

    public class MeleeWeapon : BaseBullet, IBullet
    {

        #region Varables
        [Header("Bullet Type")]
        private PlayerControls playerControls;

        public IBullet.ElementType meleeElementWeapon => IBullet.ElementType.Fire;

        public int currentBulletType = 0;

        private int currentStartEffectIndex = 0;

        [Header("Weapon Color")]
        public Color[] weaponColor;

        [Header("Animation & Collider")]
        public MeshRenderer meshRender;

        public BoxCollider swordTrigger;

        public Animator anim;
        #endregion

        //////////////////////////////////////////////////////

        #region Input Action System

        #region nextElement
        public void nextElement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentBulletType++;

                if (currentBulletType > 3) { currentBulletType = 0; }

                SetWeaponType(currentBulletType);

                SetMaterialColor(currentBulletType);

                SetMeleeWeaponImpactEffect();

                print("Next Element");

            }
        }
        #endregion

        #region previousElement
        public void previousElement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentBulletType--;

                if (currentBulletType < 0) { currentBulletType = 3; }

                SetWeaponType(currentBulletType);

                SetMaterialColor(currentBulletType);

                SetMeleeWeaponImpactEffect();

                print("Previous Element");

            }
        }
        #endregion

        #region PrimaryAttack
        public void PrimaryAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //This will set the animation for the weapon to swing
                anim.SetBool("Attacking", true);

                print("Swinging Sword");

                swordTrigger.enabled = true;
                print("Attack");
            }

            else
            {
                //Set the animation for the swing to be off
                anim.SetBool("Attacking", false);

                print("Swing Completed");

                swordTrigger.enabled = false;
            }
        }
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Start, Update, Etc

        #region Start
        void Start()
        {

            SetMaterialColor(currentBulletType);

        }
        #endregion

        #endregion

        #region Other Functions

        #region OnTriggerEnter
        private void OnTriggerEnter(Collider other)
        {

            HandleBulletCollision(other, currentStartEffectIndex);

            ApplyDamageToObject(other);

        }
        #endregion

        #region SetMaterialColor
        private void SetMaterialColor(int currentMaterialColor)
        {

            meshRender.material.SetColor("_StaffColor", weaponColor[currentMaterialColor]);

        }
        #endregion

        #region SetMeleeWeaponImpactEffect 
        private void SetMeleeWeaponImpactEffect()
        {

            switch (meleeElementWeapon)
            {

                case IBullet.ElementType.Fire:
                    currentStartEffectIndex = 0;
                    break;

                case IBullet.ElementType.Water:
                    currentStartEffectIndex = 4;
                    break;

                case IBullet.ElementType.Earth:
                    currentStartEffectIndex = 8;
                    break;

                case IBullet.ElementType.Air:
                    currentStartEffectIndex = 11;
                    break;

            }

        }
        #endregion

        #region SetWeaponType
        private void SetWeaponType(int weaponTypeNum)
        {

            switch (weaponTypeNum)
            {

                case 0:
                    currentBullletType = IBullet.ElementType.Fire;
                    break;

                case 1:
                    currentBullletType = IBullet.ElementType.Water;
                    break;

                case 2:
                    currentBullletType = IBullet.ElementType.Earth;
                    break;

                case 3:
                    currentBullletType = IBullet.ElementType.Air;
                    break;

            }

        }
        #endregion

        #endregion

    }

}