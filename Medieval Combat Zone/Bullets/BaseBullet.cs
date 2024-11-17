using UnityEngine;
using CombatZone.Damage;

namespace CombatZone.Bullet
{

    public abstract class BaseBullet : MonoBehaviour
    {

        #region Variables

        #region Bullet Element Type
        [Header("Bullet Element Type")]
        [SerializeField] protected IBullet.ElementType currentBullletType;
        #endregion

        #region Bullet References
        [Header("Bullet Properties")]
        public Transform playerTransform;

        public BulletEffectData bulletData;
        #endregion

        #endregion

        /***************************************************************/////

        #region Spawning Effect
        protected void SpawnEffect(GameObject effect)
        {

            Instantiate(effect, transform.position, Quaternion.identity);

        }
        #endregion

        #region Bullet Collision 
        protected void HandleBulletCollision(Collider colliderdObject, int startImpactEffectIndex)
        {

            IBullet bullet = colliderdObject.GetComponent<IBullet>();

            if (bullet == null) { return; }

            currentBullletType = bullet.meleeElementWeapon;

            switch (currentBullletType)
            {

                case IBullet.ElementType.Fire:
                    ValidateEffect(bulletData.impactEffects, startImpactEffectIndex);
                    Destroy(this.gameObject);
                    break;

                case IBullet.ElementType.Water:
                    ValidateEffect(bulletData.impactEffects, startImpactEffectIndex + 1);
                    Destroy(this.gameObject);
                    break;

                case IBullet.ElementType.Air:
                    ValidateEffect(bulletData.impactEffects, startImpactEffectIndex + 2);
                    Destroy(this.gameObject);
                    break;

                case IBullet.ElementType.Earth:
                    ValidateEffect(bulletData.impactEffects, startImpactEffectIndex + 3);
                    Destroy(this.gameObject);
                    break;

            }

        }
        #endregion

        #region Damage Object
        protected void ApplyDamageToObject(Collider targetObject)
        {

            IDamage damageableObject = targetObject.GetComponent<IDamage>();

            if (damageableObject != null)
            {

                damageableObject.TakeDamage(bulletData.bulletDamage);

                Destroy(this.gameObject);

            }

        }
        #endregion

        #region Destroys the bullet
        protected void ScheduleBulletDestruction(GameObject bullet)
        {

            Destroy(bullet, bulletData.destroyAfterSeconds);

        }
        #endregion

        #region Null Effect Check
        protected void ValidateEffect(GameObject[] effects, int index)
        {

            if (effects[index] == null)
            {

                print("Does not have.");

                return;

            }
            else
            {

                SpawnEffect(effects[index]);

            }

        }
        #endregion

    }

}
