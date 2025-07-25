using UnityEngine;
using CombatZone.Interfaces;

namespace CombatZone.Character.Player
{

    public class KnifeDamage : MonoBehaviour
    {

        [SerializeField] private float damage = 10f;

        private void OnTriggerEnter(Collider other)
        {

            IHealth enemy = other.GetComponent<IHealth>();

            if (enemy != null) { Debug.Log("Hitting enemy"); }

            enemy?.TakeDamage(damage);

        }

    }

}