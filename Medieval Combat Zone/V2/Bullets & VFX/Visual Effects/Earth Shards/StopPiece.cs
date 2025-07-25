using CombatZone.Interfaces;
using UnityEngine;

namespace CombatZone.VisualEffects
{
    public class StopPiece : MonoBehaviour
    {

        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float range = 0.1f;
        [SerializeField] private float radius = 1f;

        public bool isGrounded = false;

        private void OnTriggerEnter(Collider other)
        {         
            if(IsInLayerMask(other.gameObject, collisionMask))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }

            if (!isGrounded)
            {
                IHealth health = other.GetComponent<IHealth>();
                health?.TakeDamage(20f);
            }
        }

        private void Update()
        {
            if(RaycastTest(Vector3.up, range) || 
                RaycastTest(-Vector3.up, range) || 
                RaycastTest(Vector3.right, range) || 
                RaycastTest(-Vector3.right, range))
            {

                Rigidbody rb = GetComponent<Rigidbody>();
                rb.isKinematic = true;
                isGrounded = true;

            }
        }

        private bool RaycastTest(Vector3 position, float range)
        {
            if (Physics.Raycast(transform.position, position, range, collisionMask))
            { return true; }

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

        private bool IsInLayerMask(GameObject go, LayerMask layerMask)
        {
            return ((1 << go.layer) & collisionMask) != 0;
        }


    }

}