using UnityEngine;
using CombatZone.Damage;

namespace CombatZone.Player
{

    public class CrossbowShooting : MonoBehaviour
    {

        #region Varables

        #region Crossbow Setting
        [Header("Crossbow Setting")]
        public int damage;

        public float timeBetweenShooting;

        public float spread;

        public float range;

        public float reloadTime;

        public float timeBetweenShots;

        public int magazineSize;

        public int bulletsPerTap;

        public bool allowButtonHold;

        #endregion

        #region Crossbow Other Setting
        [Header("Crossbow Other Setting")]
        int bulletLeft;

        int bulletsShot;

        bool shooting;

        public bool readyToShoot;

        public bool reloading;

        public Camera fpsCam;

        public Transform attackPoint;

        public RaycastHit rayHit;

        public LayerMask whatIsEnemy;
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Start, Update, Etc

        #region Awake
        private void Awake()
        {

            bulletLeft = magazineSize;

            readyToShoot = true;

        }
        #endregion

        #region Update
        private void Update()
        {

            MyInput();

        }
        #endregion

        #endregion

        //////////////////////////////////////////////////////

        #region Gun Functions

        #region MyInput
        private void MyInput()
        {

            if (allowButtonHold) { shooting = Input.GetKey(KeyCode.Mouse0); }
            else { shooting = Input.GetKeyDown(KeyCode.Mouse0); }


            if (Input.GetKeyDown(KeyCode.R) && bulletLeft < magazineSize && !reloading)
            {

                Reload();

            }

            if (readyToShoot && shooting && !reloading && bulletLeft > 0)
            {

                bulletsShot = bulletsPerTap;

                Shoot();

            }

        }
        #endregion

        #region Shoot
        private void Shoot()
        {

            readyToShoot = false;

            //Spread
            float x = Random.Range(-spread, spread);

            float y = Random.Range(-spread, spread);

            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, whatIsEnemy))
            {

                if (rayHit.collider.CompareTag("Enemy"))
                {

                    rayHit.collider.GetComponent<IDamage>().TakeDamage(damage);

                }

            }

            bulletLeft--;

            bulletsShot--;

            Invoke("ResetShot", timeBetweenShooting);

            if (bulletsShot > 0 && bulletLeft > 0)
            {

                Invoke("Shoot", timeBetweenShots);

            }

        }
        #endregion

        #region ResetShot
        private void ResetShot()
        {

            readyToShoot = true;

        }
        #endregion

        #region Reload
        private void Reload()
        {

            reloading = true;

            Invoke("ReloadFinished", reloadTime);

        }
        #endregion

        #region ReloadFinished
        private void ReloadFinished()
        {

            bulletLeft = magazineSize;

            reloading = false;

        }
        #endregion

        #endregion

    }

}