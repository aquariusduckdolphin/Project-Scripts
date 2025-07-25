using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Bullet;

namespace CombatZone.Character.Player
{

    public class PlayerShooting : MonoBehaviour
    {

        #region Varibales

        private PlayerControls playerControls;

        [SerializeField] private PlayerInputHandler inputHandler;

        public GameObject[] bullet;
        private int currentBulletType = 0;

        [SerializeField] private GameObject shootLocation;
        [SerializeField] private float shootStrength = 10000f;
        [SerializeField] private float inFront = 0.5f;

        [SerializeField] private Color[] weaponColor;
        [SerializeField] private MeshRenderer weapon;

        #endregion

        /**************** Awake, Start, Etc. ****************/

        #region Awake
        void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
            playerControls = inputHandler.controls;
        }
        #endregion

        #region Start
        private void Start()
        {
            SetWeaponMaterial(currentBulletType);
        }
        #endregion

        #region Controls Assignment
        private void AssignControls()
        {
            playerControls.Attack.Shooting.performed += OnShooting;
            playerControls.Attack.Shooting.canceled += OnShooting;
            playerControls.Attack.NextBullet.performed += Forward;
            playerControls.Attack.PreviousBullet.performed += Reverse;
        }
        #endregion

        /**************** On Enable, On Disable, Etc. ****************/

        #region On Enable
        private void OnEnable()
        {
            //AssignControls();
        }
        #endregion

        #region On Disable
        private void OnDisable()
        {
            /*playerControls.Attack.Shooting.performed -= OnShooting;
            playerControls.Attack.NextBullet.performed -= Forward;
            playerControls.Attack.PreviousBullet.performed -= Reverse;
            playerControls.Disable();*/
        }
        #endregion

        /**************** Shooting Methods ****************/

        #region Shooting
        public void OnShooting(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector3 newPosition = shootLocation.transform.position;
                newPosition.z += inFront;

                GameObject bullets = Instantiate(bullet[currentBulletType], shootLocation.transform.position, Quaternion.identity);
                BaseBullet baseBullet = bullets.GetComponent<BaseBullet>();
                baseBullet.playerTransform = gameObject.transform.GetChild(0).transform;
                print(baseBullet.playerTransform.name);

                Rigidbody rb = bullets.GetComponent<Rigidbody>();
                rb.AddForce(shootLocation.transform.forward * shootStrength);
            }
        }
        #endregion

        #region Bullet Forward
        public void Forward(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentBulletType++;
                if (currentBulletType > bullet.Length - 1) { currentBulletType = 0; }
                SetWeaponMaterial(currentBulletType);
            }
        }
        #endregion

        #region Reverse
        public void Reverse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                currentBulletType--;
                if (currentBulletType < 0) { currentBulletType = bullet.Length - 1; }
                SetWeaponMaterial(currentBulletType);
            }
        }
        #endregion

        #region Settings Material Color
        private void SetWeaponMaterial(int currentMaterialColor)
        {
            if(weapon == null) { return; }
            weapon.material.SetColor("_StaffColor", weaponColor[currentMaterialColor]);
        }
        #endregion

    }

}