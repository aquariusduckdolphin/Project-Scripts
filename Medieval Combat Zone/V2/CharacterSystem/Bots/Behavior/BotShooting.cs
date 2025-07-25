using CombatZone.Interfaces;
using UnityEngine;

namespace CombatZone.Character.Bot
{

    [RequireComponent(typeof(BotBehavior))]
    public class BotShooting : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private BotBlackboard blackboard;
        
        [Header("Shooting Properties")]
        [SerializeField] private int bulletLeft = 1;
        private int bulletsShot = 0;
        [SerializeField] private bool readyToShoot = true;
        [SerializeField] private bool reloading;
        private int currentBulletPrefabType = 0;
        [SerializeField] private float delayShot;

        /**************** Start ****************/

        #region Start
        private void Start()
        {
            blackboard = GetComponent<BotBlackboard>();
            if(bulletsShot > 0) { bulletsShot = 0; }
            readyToShoot = true;
        }
        #endregion

        /**************** Shooting & Reloading Functions ****************/

        #region Shooting
        public void Shooting()
        {
            if (bulletLeft > 0 && readyToShoot)
            {
                delayShot -= Time.deltaTime;
                if (delayShot <= 0)
                {
                    delayShot = blackboard.BotData.currentDelayShot;
                    Invoke("Shoot", blackboard.BotData.timeBetweenShots);
                }
            }
            else if (bulletLeft <= 0 && !reloading)
            {
                Invoke("Reload", blackboard.BotData.reloadTime);
            }
        }
        #endregion

        #region Shoot
        private void Shoot()
        {
            RaycastHit hit;
            bool raycast = Physics.Raycast(transform.position, 
                transform.forward, 
                out hit, 
                blackboard.FieldOfViewSettings.viewRadius);

            if (raycast == false) { return; }

            bool redTeamTag = hit.collider.CompareTag("Red Team");
            bool blueTeamTag = hit.collider.CompareTag("Blue Team");

            if (raycast && redTeamTag || blueTeamTag)
            {
                IHealth damageAmount = hit.transform.GetComponent<IHealth>();
                damageAmount?.TakeDamage(10f);

                currentBulletPrefabType = Random.Range(0, 4);
                Vector3 newPos = transform.position + blackboard.BotData.shootOffset;
                GameObject bullet = Instantiate(blackboard.BotData.bulletPrefab[currentBulletPrefabType], newPos, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * blackboard.BotData.bulletForce, ForceMode.Impulse);
                blackboard.BotAnimations.SetAction(
                    blackboard.CharacterAnimator, 
                    blackboard.BotAnimations.Attacking, 
                    true);
            }

            bulletLeft--;
            bulletsShot++;
        }
        #endregion

        #region Reload
        private void Reload()
        {
            readyToShoot = false;
            reloading = true;
            Invoke("ReloadFinished", blackboard.BotData.reloadTime);
        }
        #endregion

        #region ReloadFinished
        private void ReloadFinished()
        {
            bulletLeft = blackboard.BotData.magazineSize;
            reloading = false;
            readyToShoot = true;
        }
        #endregion

    }

}