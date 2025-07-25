using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using CombatZone.Objective;
using UnityEngine.Search;

namespace CombatZone.Spawner
{

    public class Spawners : MonoBehaviour
    {

        #region Varables
        [SearchContext("t:prefab l:AI", SearchViewFlags.GridView | SearchViewFlags.Centered | SearchViewFlags.OpenInspectorPreview)]
        [SerializeField] private GameObject bots;

        [SerializeField] private bool isRedTeamSpawner = false;

        [SerializeField] private bool canSpawnBots = false;

        #region Spawn Location
        [Header("Spawn Location")]
        private Transform minSpawnLocation;

        private Transform maxSpawnLocation;
        #endregion

        #region Bots
        [Header("Bots")]
        [SerializeField] private int totalNumberOfBots = 0;

        [SerializeField] private int initialAmountToSpawn = 10;
        #endregion

        #endregion

        /***************************************************************/

        #region Start, Update, Etc

        #region Start
        void Start()
        {

            minSpawnLocation = transform.GetChild(0);

            maxSpawnLocation = transform.GetChild(1);

            if(canSpawnBots) { SpawnBots(initialAmountToSpawn, bots); }

        }
        #endregion

        #region Update
        private void Update()
        {

            if (canSpawnBots && totalNumberOfBots < 10) { SpawnBots(1, bots); }

        }
        #endregion

        #endregion

        #region Spawner Functions

        #region SpawnBots
        public void SpawnBots(int loopAmount, GameObject team)
        {

            for (int i = 0; i < loopAmount; i++)
            {

                Vector3 newSpawnLocation = Vector3.zero;

                newSpawnLocation.x = Random.Range(minSpawnLocation.position.x, maxSpawnLocation.position.x);

                newSpawnLocation.y = minSpawnLocation.position.y;

                newSpawnLocation.z = Random.Range(minSpawnLocation.position.z, maxSpawnLocation.position.z);

                Instantiate(team, newSpawnLocation, Quaternion.identity);

                totalNumberOfBots += 1;

            }

        }
        #endregion

        #region CleanUpBots
        public void CleanUpBots(List<GameObject> bots)
        {

            foreach (GameObject bot in bots.ToList())
            {

                if (bot == null)
                {

                    bots.Remove(bot);

                    totalNumberOfBots -= 1;

                }

            }

        }
        #endregion

        #endregion

    }

}
