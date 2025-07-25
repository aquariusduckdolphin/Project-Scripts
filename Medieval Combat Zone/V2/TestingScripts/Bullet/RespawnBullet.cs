using System.Collections;
using UnityEngine;

namespace CombatZone.Testing
{

    public class RespawnBullet : MonoBehaviour
    {

        public GameObject bullet;

        [SerializeField] private float timeBetweenRespawn = 1f;

        private void OnTriggerExit(Collider other)
        {

            StartCoroutine(SpawnBullet());

        }

        IEnumerator SpawnBullet()
        {

            yield return new WaitForSeconds(timeBetweenRespawn);

            Instantiate(bullet, transform.position, Quaternion.identity);

        }

    }

}