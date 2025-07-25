using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace CombatZone.Testing
{

    public class PlayerConfigurationManager : MonoBehaviour
    {

        private List<PlayerConfiguration> playerConfigs;

        [SerializeField] private int maxPlayers = 2;

        #region Singleton
        public static PlayerConfigurationManager _instance { get; private set; }

        private void Awake()
        {

            if (_instance != null)
            {

                Debug.Log("Trying to create another instance of singleton");

            }
            else
            {

                _instance = this;

                DontDestroyOnLoad(_instance);

                playerConfigs = new List<PlayerConfiguration>();

            }

        }
        #endregion

        public List<PlayerConfiguration> GetPlayerConfigs()
        {

            return playerConfigs;

        }

        //This will set the model character for the player.
        public void SetPlayerCharacter(int index, GameObject characterModel)
        {

            playerConfigs[index].character = characterModel;

        }

        public void ReadyPlayer(int index)
        {

            playerConfigs[index].isReady = true;

            if (playerConfigs.Count == maxPlayers && playerConfigs.All(p => p.isReady = true))
            {

                SceneManager.LoadScene("GameScene");

            }

        }

        public void HandlePlayerJoin(PlayerInput pi)
        {

            Debug.Log("Player JOined " + pi.playerIndex);

            if (!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
            {

                pi.transform.SetParent(transform);

                playerConfigs.Add(new PlayerConfiguration(pi));

            }

        }

    }

    public class PlayerConfiguration
    {

        public PlayerConfiguration(PlayerInput pi)
        {

            playerIndex = pi.playerIndex;

            input = pi;

        }

        public PlayerInput input { get; set; }

        public int playerIndex { get; set; }

        public bool isReady { get; set; }

        public GameObject character { get; set; }

    }

}