using UnityEngine;

namespace CombatZone.Fountain
{

    public class DestroyFountain : MonoBehaviour
    {

        [SerializeField] private GameObject fountain;

        private void OnTriggerEnter(Collider other)
        {
            Destroy(fountain);
        }

    }

}