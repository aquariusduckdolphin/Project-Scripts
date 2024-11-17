using UnityEngine;

namespace CombatZone.Bullet
{

    public class FireBullet : BaseBullet, IBullet
    {

        public IBullet.ElementType meleeElementWeapon => IBullet.ElementType.Fire;

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.transform == playerTransform) { return; }

            HandleBulletCollision(other, 0);

            ApplyDamageToObject(other);

            ScheduleBulletDestruction(gameObject);

        }

    }

}