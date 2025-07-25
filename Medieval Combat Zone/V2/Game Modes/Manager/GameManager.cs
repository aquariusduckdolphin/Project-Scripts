using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CombatZone.Manager 
{
    
    public abstract class GameManager : MonoBehaviour
    {

        /**************** Variables ****************/

        [Header("Point Properties")]
        [SerializeField] protected int redTeamTotal = 0;
        [SerializeField] protected int blueTeamTotal = 0;
        [SerializeField] protected int amountToWin = 100;
        [Tooltip("How fast the points will be obtained. " +
            "Used for objectives that will be held by teams.")]
        [SerializeField] protected float pointAccumulationInterval = 1f;

        [Space(10f)]
        
        [Header("Objective Properties")]
        public List<GameObject> objectives = new List<GameObject>();
        protected Coroutine coroutine;
        
        [Space(10f)]
        
        [Header("Red Team Properties")]
        public List<GameObject> redTeam;

        [Space(10f)]

        [Header("Blue Team Properties")]
        public List<GameObject> blueTeam;

        /**************** Base Methods ****************/

        #region Win Screens
        protected void SetWinScreen()
        {
            if (blueTeam.Count == 0 && redTeam.Count == 0) { return; }

            if (redTeamTotal == amountToWin)
            {
                SceneManager.LoadScene("RedTeamWinScene");
                DisableCoroutine(coroutine);
                Destroy(gameObject);
            }
            else if (blueTeamTotal == amountToWin)
            {
                SceneManager.LoadScene("BlueTeamWinScene");
                DisableCoroutine(coroutine);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Setting score for player
        protected void SetScore(TMP_Text redTeamScoreText, TMP_Text blueTeamScoreText)
        {
            if (redTeamScoreText == null || blueTeamScoreText == null) { return; }
            redTeamScoreText.text = redTeamTotal.ToString();
            blueTeamScoreText.text = blueTeamTotal.ToString();
        }
        #endregion

        #region Disable Coroutines
        /// <summary>
        /// Stop a coroutine and set it to null
        /// </summary>
        /// <param name="coroutine"></param>
        protected void DisableCoroutine(Coroutine coroutine)
        {
            if(coroutine == null) { return; }
            StopCoroutine(coroutine);
            coroutine = null;
        }
        #endregion

    }

}