using UnityEngine;

namespace CombatZone.Bullet
{

    public class EarthBullet : BaseBullet, IBullet
    {

        public IBullet.ElementType meleeElementWeapon => IBullet.ElementType.Earth;

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.transform == playerTransform) { return; }

            HandleBulletCollision(other, 8);

            ApplyDamageToObject(other);

            ScheduleBulletDestruction(gameObject);

        }

    }

}