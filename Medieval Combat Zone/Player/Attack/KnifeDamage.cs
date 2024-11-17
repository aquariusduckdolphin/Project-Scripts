using UnityEngine;
using CombatZone.Damage;

namespace CombatZone.Player
{

    public class KnifeDamage : MonoBehaviour
    {

        [SerializeField] private float damage = 10f;

        private void OnTriggerEnter(Collider other)
        {

            IDamage enemy = other.GetComponent<IDamage>();

            if (enemy != null) { print("Hitting enemy"); }

            enemy?.TakeDamage(damage);

        }

    }

}