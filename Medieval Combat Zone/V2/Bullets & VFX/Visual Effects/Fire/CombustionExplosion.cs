using CombatZone.Interfaces;
using System.Collections;
using UnityEngine;

namespace CombatZone.VisualEffects
{
    public class CombustionExplosion : BaseEffects
    {

        RaycastHit hit;
        [SerializeField] private float radius = 10f;
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private float damage = 50f;
        [SerializeField] private float destroyDelay = 5f;

        IEnumerator Start()
        {
            PlayAudioOnce();

            Collider[] collider = Physics.OverlapSphere(transform.position, radius, collisionMask);
            foreach (Collider col in collider)
            {
                IHealth health = col.GetComponent<IHealth>();
                health?.TakeDamage(damage);
            }

            yield return new WaitForSeconds(5f);

            bulletData.DestroyAfterSoundImpact(gameObject, audioSource, elementType, impactEffectIndex);

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

    }
}
