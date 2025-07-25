using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using CombatZone.Objective;

namespace CombatZone.Scoreboard
{

    public class ScoreboardEntry : MonoBehaviour
    {

        #region Scoreboard Ui Slots struct
        [System.Serializable]
        public struct ScoreboardUISlots
        {

            public TMP_Text playerNameText;

            public TMP_Text playerScoreText;

            public TMP_Text playerKillCountText;

            public TMP_Text playerDeathCountText;

        }
        #endregion

        #region Player Score Data struct
        [System.Serializable]
        public struct PlayerScoreData
        {

            public string playerName;

            public int totalScore;

            public int killCount;

            public int deathCount;

        }
        #endregion

        #region Variables

        #region Other
        [SerializeField] private GameObject scoreboard;

        private PlayerControls playerControls;

        public bool isOpen = false;

        [SerializeField] private bool isFirstTimeOpen = true;
        #endregion

        #region Initialized arrays
        [SerializeField] private PlayerScoreData[] playersTemp = new PlayerScoreData[20];

        [SerializeField] private ScoreboardUISlots[] scoreboardUI = new ScoreboardUISlots[20];

        #endregion

        #region Scores
        [Space]
        [Header("Score")]
        [SerializeField] private int scorePerKill = 40;

        [SerializeField] private int scorePerDeath = -20;

        [SerializeField] private int scorePerObjective = 60;
        #endregion

        #endregion

        /***************************************************************/

        #region Start
        private void Start()
        {

            playerControls = new PlayerControls();
            playerControls.Enable();
            playerControls.Player.Scoreboard.performed += OnUIOpen;
            playerControls.Player.Scoreboard.canceled += OnUIOpen;
            //scoreboard.SetActive(false);

        }
        #endregion

        #region Open and Close Scoreboard
        private void OnUIOpen(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                scoreboard.SetActive(true);

                if (isFirstTimeOpen) { InitializeUI(); }
                else { RefreshScoreboard(); }

                isOpen = true;

            }

            
            if (context.canceled)
            {

                //scoreboard.SetActive(false);
                //isOpen = false;

            }
            

        }
        #endregion

        #region Update
        private void Update()
        {

            if (isOpen) { RefreshScoreboard(); }

        }
        #endregion

        /***************************************************************/

        #region Initialize Scoreboard
        private void InitializeUI()
        {

            for (int i = 0; i < scoreboardUI.Length; i++)
            {

                var uiElement = GameObject.FindGameObjectsWithTag("ScoreboardNameUI")[i];
                scoreboardUI[i].playerNameText = uiElement.transform.GetChild(0).transform.GetComponent<TMP_Text>();
                scoreboardUI[i].playerScoreText = uiElement.transform.GetChild(1).transform.GetComponent<TMP_Text>();
                scoreboardUI[i].playerKillCountText = uiElement.transform.GetChild(2).transform.GetComponent<TMP_Text>();
                scoreboardUI[i].playerDeathCountText = uiElement.transform.GetChild(3).transform.GetComponent<TMP_Text>();

            }

            isFirstTimeOpen = false;

        }
        #endregion

        #region Update Player Data
        private void UpdatePlayerData()
        {

            var redTeam = DominationPointManager._instance.redTeam;
            var blueTeam = DominationPointManager._instance.blueTeam;

            for (int i = 0; i < playersTemp.Length; i++)
            {

                PlayerStats player = i < 10 ? redTeam[i].GetComponent<PlayerStats>() : blueTeam[i - 10].GetComponent<PlayerStats>();
                playersTemp[i].playerName = player.playerName;
                playersTemp[i].killCount = player.kill;
                playersTemp[i].deathCount = player.death;
                playersTemp[i].totalScore = CalculateScore(player.kill, player.death, player.objectScore);

            }

        }
        #endregion

        #region Calculate Score
        private int CalculateScore(int kill, int death, int objectiveScore)
        {

            int totalScore = 0;

            totalScore += (scorePerKill * kill);

            totalScore += (scorePerDeath * death);

            totalScore += (scorePerObjective * objectiveScore);

            return totalScore;

        }
        #endregion

        #region Sort Players By Score
        private void SortPlayersByScore()
        {

            int mid = playersTemp.Length / 2;

            Array.Sort(playersTemp, 0, mid, new ScoreboardSorter());

            Array.Sort(playersTemp, mid, playersTemp.Length - mid, new ScoreboardSorter());

            UpdateUISlots();

        }
        #endregion

        #region Update Ui Slots
        private void UpdateUISlots()
        {

            for (int i = 0; i < playersTemp.Length; i++)
            {

                scoreboardUI[i].playerNameText.text = playersTemp[i].playerName;
                scoreboardUI[i].playerScoreText.text = playersTemp[i].totalScore.ToString();
                scoreboardUI[i].playerKillCountText.text = playersTemp[i].killCount.ToString();
                scoreboardUI[i].playerDeathCountText.text = playersTemp[i].deathCount.ToString();

            }

        }
        #endregion

        #region Referesh Scoreboard
        private void RefreshScoreboard()
        {

            UpdatePlayerData();
            SortPlayersByScore();

        }
        #endregion

    }

}