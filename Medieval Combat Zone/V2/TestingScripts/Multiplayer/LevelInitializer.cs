using UnityEngine;

namespace CombatZone.Testing
{

    public class LevelInitializer : MonoBehaviour
    {

        public PlayerConfiguration[] playerConfigs;

        [SerializeField] private Transform[] playerSpawns;

        [SerializeField] private GameObject playerPrefab;

        // Start is called before the first frame update
        void Start()
        {

            playerConfigs = PlayerConfigurationManager._instance.GetPlayerConfigs().ToArray();

            for (int i = 0; i < playerConfigs.Length; i++)
            {

                var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);

                //player.GetComponent<PlayerMovement>().InitializePlayer(playerConfigs[i]);

            }

        }

    }

}