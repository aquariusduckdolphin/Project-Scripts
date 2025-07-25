using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Search;

namespace CombatZone.Spawner
{

    public class Spawners : MonoBehaviour
    {

        /**************** Variables ****************/

        [SearchContext("t:prefab l:AI", SearchViewFlags.GridView | SearchViewFlags.Centered | SearchViewFlags.OpenInspectorPreview)]
        [SerializeField] private GameObject bots;
        [SerializeField] private bool canSpawnBots = true;
        [SerializeField] private float delay = 0.5f;

        [Header("Spawn Location")]
        private Transform minSpawnLocation;
        private Transform maxSpawnLocation;

        [Header("Bots")]
        [SerializeField] private int totalNumberOfBots = 0;
        [SerializeField] private int initialAmountToSpawn = 10;

        [SerializeField] private Scene cityScene;

        /**************** Start, Update, Etc. ****************/

        #region Start
        IEnumerator Start()
        {
            minSpawnLocation = transform.GetChild(0).transform;
            maxSpawnLocation = transform.GetChild(1).transform;
            yield return new WaitForSeconds(2.5f);
            if(canSpawnBots) 
            { 
                if(totalNumberOfBots == initialAmountToSpawn) { yield return null; }
                StartCoroutine(SpawnBots(bots, initialAmountToSpawn, delay)); 
            }
        }
        #endregion

        /**************** Spawner Functions ****************/

        #region Bot Spawning
        private IEnumerator SpawnBots(GameObject botToSpawn, int howManyBots, float delay)
        {
            for (int i = 0; i < howManyBots; i++)
            {
                Instantiate(botToSpawn, 
                    GenerateBotSpawnLocation(), 
                    Quaternion.identity);

                totalNumberOfBots += 1;
                yield return new WaitForSeconds(delay);
            }
        }
        #endregion

        #region Generate Bot Spawn Location
        public Vector3 GenerateBotSpawnLocation()
        {
            Vector3 newSpawnLocation = Vector3.zero;
            newSpawnLocation.x = Random.Range(minSpawnLocation.position.x, maxSpawnLocation.position.x);
            newSpawnLocation.y = 6.8f;
            newSpawnLocation.z = Random.Range(minSpawnLocation.position.z, maxSpawnLocation.position.z);
            return newSpawnLocation;
        }
        #endregion

    }

}
