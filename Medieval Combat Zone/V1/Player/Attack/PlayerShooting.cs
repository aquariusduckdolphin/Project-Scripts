using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Bullet;

namespace CombatZone.Player
{

    public class PlayerShooting : MonoBehaviour
    {

        #region Varibales

        public GameObject[] bullet;

        private int currentBulletType = 0;

        private PlayerControls playerControls;

        public GameObject shootLocation;

        public float shootStrength = 10000f;

        [SerializeField] private float inFront = 0.5f;

        [SerializeField] private Color[] weaponColor;

        [SerializeField] private MeshRenderer weapon;

        #endregion

        ////////////////////////////////////////////////////////////

        #region Awake, Start, Etc.

        #region Awake
        void Awake()
        {

            playerControls = new PlayerControls();

            AssignControls();

        }
        #endregion

        #region Start
        private void Start()
        {

            SetWeaponMaterial(currentBulletType);

        }
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region On Enable, On Disable, Etc

        #region On Enable
        private void OnEnable()
        {

            AssignControls();

        }
        #endregion

        #region Controls Assignment
        private void AssignControls()
        {

            playerControls.Enable();

            playerControls.Player.Shooting.performed += OnShooting;

            playerControls.Player.Shooting.canceled += OnShooting;

            playerControls.Player.NextBullet.performed += Forward;

            playerControls.Player.PreviousBullet.performed += Reverse;

        }
        #endregion

        #region On Disable
        private void OnDisable()
        {

            playerControls.Player.Shooting.performed -= OnShooting;

            playerControls.Player.NextBullet.performed -= Forward;

            playerControls.Player.PreviousBullet.performed -= Reverse;

            playerControls.Disable();

        }
        #endregion

        #endregion

        #region Shooting
        private void OnShooting(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                Vector3 newPosition = shootLocation.transform.position;

                newPosition.z += inFront;

                GameObject bullets = Instantiate(bullet[currentBulletType], shootLocation.transform.position, Quaternion.identity);

                BaseBullet baseBullet = bullets.GetComponent<BaseBullet>();

                baseBullet.playerTransform = this.gameObject.transform.GetChild(0).transform;

                Rigidbody rb = bullets.GetComponent<Rigidbody>();

                rb.AddForce(shootLocation.transform.forward * shootStrength);

            }

        }
        #endregion

        #region bullet
        void Forward(InputAction.CallbackContext context)
        {

            currentBulletType++;

            if (currentBulletType > bullet.Length - 1)
            {

                currentBulletType = 0;

            }

            SetWeaponMaterial(currentBulletType);

        }

        void Reverse(InputAction.CallbackContext context)
        {

            currentBulletType--;

            if (currentBulletType < 0)
            {

                currentBulletType = bullet.Length - 1;

            }

            SetWeaponMaterial(currentBulletType);

        }
        #endregion

        #region Settings Material Color
        private void SetWeaponMaterial(int currentMaterialColor)
        {

            weapon.material.SetColor("_StaffColor", weaponColor[currentMaterialColor]);

        }
        #endregion

    }

}