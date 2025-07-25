using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatZone.Testing
{

    public class PlayerInfo : MonoBehaviour
    {

        #region Singleton
        public static PlayerInfo _instance;

        private void Awake()
        {

            if (_instance != null && _instance != this)
            {

                Destroy(this);

            }
            else
            {

                _instance = this;

                DontDestroyOnLoad(gameObject);

            }

        }
        #endregion

        public int playerIndex = 0;

        public List<SetupPlayerControls> control;

        public int playerCount = 0;

        [SerializeField] private string sceneName;

        void Update()
        {

            if (playerCount == 2)
            {

                SceneManager.LoadScene(sceneName);

            }

        }

        public void PlayerTwoJoined()
        {

            playerIndex++;

        }

    }

}