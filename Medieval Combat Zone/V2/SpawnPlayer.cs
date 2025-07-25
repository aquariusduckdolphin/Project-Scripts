using System.Collections;
using UnityEngine;

namespace CombatZone
{
    public class SpawnPlayer : MonoBehaviour
    {

        [SerializeField] private GameObject player;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(player, transform.position, Quaternion.Euler(0f, 0f, 0f));
        }

    }
}