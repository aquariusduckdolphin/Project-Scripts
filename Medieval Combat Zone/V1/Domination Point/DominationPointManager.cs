using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using CombatZone.Spawner;

namespace CombatZone.Objective
{

    public class DominationPointManager : MonoBehaviour
    {

        #region Singleton
        public static DominationPointManager _instance { get; set; }

        private void Awake()
        {

            if (_instance != null && _instance != this)
            {

                Destroy(this);

            }
            else
            {

                _instance = this;

            }

        }
        #endregion

        /***************************************************************/

        #region Variables

        public List<GameObject> dominationPoints = new List<GameObject>();

        #region Points
        [Header("Points")]
        [SerializeField] private int redTeamTotal = 0;

        [SerializeField] private int blueTeamTotal = 0;

        [SerializeField] private int amountToWin = 100;

        [Tooltip("How fast the points will be obtained")]
        [SerializeField] private float pointAccumulationInterval = 1f;
        #endregion

        #region Red Team
        [Header("Red Team")]
        public List<GameObject> redTeam;

        [SerializeField] private TMP_Text redTeamScoreText;

        [SerializeField] private Spawners redTeamSpawn;
        #endregion

        #region Blue Team
        [Header("Blue Team")]
        public List<GameObject> blueTeam;

        [SerializeField] private TMP_Text blueTeamScoreText;

        [SerializeField] private Spawners blueTeamSpawn;
        #endregion

        #endregion

        /***************************************************************/

        #region Start & Update

        #region Start
        private void Start()
        {

            SetScore(redTeamTotal, blueTeamTotal);

            GatherDominationPoint();

            if (dominationPoints.Count == 0) { return; }

            StartCoroutine(AddPoints(pointAccumulationInterval));

        }
        #endregion

        #region Update
        private void Update()
        {

            if (blueTeam.Count == 0 && redTeam.Count == 0) { return; }

            blueTeamSpawn?.CleanUpBots(blueTeam);

            redTeamSpawn?.CleanUpBots(redTeam);

            if (redTeamTotal == amountToWin)
            {

                SceneManager.LoadScene("RedTeamWinScene");

            }
            else if (blueTeamTotal == amountToWin)
            {

                SceneManager.LoadScene("BlueTeamWinScene");

            }

        }
        #endregion

        #endregion

        #region Find & store domination point
        private void GatherDominationPoint()
        {

            dominationPoints.Clear();

            var dominationPointsObjective = GameObject.FindGameObjectsWithTag("DominationPoint");

            foreach (var dp in dominationPointsObjective)
            {

                if (dp != null)
                {

                    dominationPoints.Add(dp.gameObject);

                }

            }

        }
        #endregion

        #region Setting score for player
        private void SetScore(int playerTeam, int enemyTeam)
        {

            if (redTeamScoreText == null || blueTeamScoreText == null) { return; }

            redTeamScoreText.text = playerTeam.ToString();

            blueTeamScoreText.text = enemyTeam.ToString();

        }
        #endregion

        #region Add points over time
        private IEnumerator AddPoints(float delay)
        {

            yield return new WaitForSeconds(delay);

            if (dominationPoints?.Count > 0)
            {

                foreach (GameObject domp in dominationPoints)
                {

                    DominationPoint dp = domp.GetComponent<DominationPoint>();

                    if (dp.isPointContested)
                    {
                        continue;
                    }
                    else if (dp.isRedTeamPoint)
                    {

                        redTeamTotal += 1;

                    }
                    else if (dp.isBlueTeamPoint)
                    {

                        blueTeamTotal += 1;

                    }

                }

                SetScore(redTeamTotal, blueTeamTotal);

                StartCoroutine(AddPoints(delay));

            }

        }
        #endregion

    }

}