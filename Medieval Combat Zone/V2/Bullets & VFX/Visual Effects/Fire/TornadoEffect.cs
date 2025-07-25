using CombatZone.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace CombatZone.VisualEffects
{

    public class TornadoEffect : BaseEffects
    {

        [Header("Knockback Properties")]
        [SerializeField] private Vector3 movePosition;
        [SerializeField] private float radius;
        [SerializeField] private float midRadius;
        [SerializeField] private float knockback;

        private Vector3 bottomTransform;
        [SerializeField] private Vector3 midPointTransform;
        [SerializeField] private Vector3 topTransform;

        public Collider[] hit;

        private void Start()
        {
            PlayAudioEffect();
        }

        void Update()
        {
            transform.position += movePosition;

            Knockback(transform.position + bottomTransform, radius, collisionMask, knockback);
            Knockback(transform.position + midPointTransform, midRadius, collisionMask, knockback);
            Knockback(transform.position + topTransform, radius, collisionMask, knockback);
            SelectEffectVariantBySurface();
        }

        private void OnTriggerEnter(Collider other)
        {       
            IHealth health = other.GetComponent<IHealth>();

            if(IsInLayerMask(other.gameObject, collisionMask) && health != null)
            {
                
            }  
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + bottomTransform, radius);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + midPointTransform, midRadius);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + topTransform, radius);
        }

        private void SelectEffectVariantBySurface()
        {
            hit = Physics.OverlapSphere(transform.position, radius, collisionMask);

            foreach (Collider player in hit)
            {
                IHealth health = player.GetComponent<IHealth>();
                StartCoroutine(health?.TickDamage(10f, 1f, false, 10f));
            }
        }

    }

}