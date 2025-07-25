using CombatZone.Interfaces;
using UnityEngine;

namespace CombatZone.VisualEffects
{

    public class Explosion : BaseEffects
    {

        [SerializeField] private float radius = 10f;
        [SerializeField] private GameObject explosionEffect;
        [SerializeField] private float amountOfHealthReduction = 10f;

        private bool hasExplosionSpawned = false;

        /**************** Start, Update, Etc. ****************/

        #region Update
        void Update()
        {
            DestructionTimer(ref remainingTime, 0f, gameObject);

            if (hasExplosionSpawned)
            { DamagePlayer(transform.position, radius, collisionMask); }
        }
        #endregion

        /**************** Explosion Methods ****************/

        #region Destruction SelectEffectVariantBySurface Override
        protected override void DestructionTimer(ref float remainingDuration, float destructionThreshold, GameObject targetObject)
        {
            if (remainingDuration > 0) { remainingDuration -= Time.deltaTime; }

            else if (remainingDuration <= destructionThreshold) 
            { 
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                hasExplosionSpawned = true;
                Destroy(targetObject);          
            }
        }
        #endregion

        #region Damage Players
        private void DamagePlayer(Vector3 postion, float radius, LayerMask collisionMask)
        {
            RaycastHit[] hit = Physics.SphereCastAll(postion, radius, Vector3.up, radius, collisionMask);

            foreach (RaycastHit player in hit)
            {
                IHealth health = player.collider.GetComponent<IHealth>();
                health?.TakeDamage(amountOfHealthReduction);
            }
        }
        #endregion

        /**************** Gizmos ****************/

        #region Draw Gizmos Selected
        private void OnDrawGizmosSelected()
        {         
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        #endregion

    }

}