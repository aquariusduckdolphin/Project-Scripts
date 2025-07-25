using System.Collections;
using UnityEngine;

namespace CombatZone.Manager
{

    public class DominationPointManager : GameManager
    {

        #region Singleton
        public static DominationPointManager _instance { get; set; }

        protected void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(this); }
            else { _instance = this; }
        }
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Start
        private void Start()
        {
            GatherDominationPoint();
            if (objectives.Count == 0) { return; }
            StartCoroutine(AddPoints(pointAccumulationInterval));
        }
        #endregion

        #region Update
        private void Update()
        {
            if (blueTeam.Count == 0 && redTeam.Count == 0) { return; }
            SetWinScreen();
        }
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Find & store domination objectivePoint
        private void GatherDominationPoint()
        {
            objectives.Clear();
            var dominationPointsObjective = GameObject.FindGameObjectsWithTag("DominationPoint");

            foreach (var dp in dominationPointsObjective)
            {
                if (dp != null) { objectives.Add(dp.gameObject); }
            }
        }
        #endregion

        #region Add points over time
        private IEnumerator AddPoints(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (objectives?.Count > 0)
            {
                foreach (GameObject objectivePoint in objectives)
                {
                    DominationPoint dp = objectivePoint.GetComponent<DominationPoint>();

                    if (dp.isPointContested) { continue; }
                    else if (dp.isRedTeamPoint) { redTeamTotal += 1; }
                    else if (dp.isBlueTeamPoint) { blueTeamTotal += 1; }
                }

                DisableCoroutine(coroutine);
                coroutine = StartCoroutine(AddPoints(delay));
            }
        }
        #endregion

    }

}