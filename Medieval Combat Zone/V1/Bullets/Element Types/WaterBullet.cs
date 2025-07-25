using UnityEngine;

namespace CombatZone.Bullet
{

    public class WaterBullet : BaseBullet, IBullet
    {

        public IBullet.ElementType meleeElementWeapon => IBullet.ElementType.Water;

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.transform == playerTransform) { return; }

            HandleBulletCollision(other, 4);

            ApplyDamageToObject(other);

            ScheduleBulletDestruction(gameObject);

        }

    }

}