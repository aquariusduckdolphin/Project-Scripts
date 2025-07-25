using UnityEngine;

namespace CombatZone.Bullet
{

    public class AirBullet : BaseBullet, IBullet
    {

        IBullet.ElementType IBullet.meleeElementWeapon => IBullet.ElementType.Air;

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.transform == playerTransform) { return; }

            HandleBulletCollision(other, 11);

            ApplyDamageToObject(other);

            ScheduleBulletDestruction(gameObject);

        }

    }

}