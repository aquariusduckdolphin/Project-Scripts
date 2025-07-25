using CombatZone.Interfaces;
using UnityEngine;

namespace CombatZone
{
    public class HealthTest : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            
            IHealth health = other.GetComponent<IHealth>();

            print(other.name);

            health.TakeDamage(50f);


        }

    }
}
