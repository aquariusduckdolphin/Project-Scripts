using System.Collections.Generic;
using UnityEngine;
using CombatZone.Manager;
using UnityEngine.UI;

namespace CombatZone.Character
{

    public class PlayerStats : MonoBehaviour
    {

        [Header("Team")]
        public Team team = Team.none;
        [SerializeField] public bool isRedTeam = false;
        [SerializeField] private Image playerIcon;

        #region Name List
        [Header("Name List")]
        private List<string> names = new List<string>()
        {
            "Susan Pierpoint",
            "StanBarry",
            "Bob Day",
            "John32123",
            "Lauren_090JK",
            "Morgan$90112-",
            "Freeman",
            "Lee"
        };
        #endregion

        [Header("Stats")]
        public string playerName { get; private set; }
        public int kill { get; private set; }
        public int death { get; private set; }
        public int objectScore { get; private set; }

        /**************** Start ****************/

        #region Start
        private void Start()
        {
            GenerateRandomName();

            if (isRedTeam)
            {
                SetRedTeam();
                return;
            }

            SetBlueTeam();
        }
        #endregion

        #region Generate Random Name
        private string GenerateRandomName()
        {
            playerName = names[Random.Range(0, names.Count)];
            return playerName;
        }
        #endregion

        /**************** Set Teams ****************/

        #region Set Red Team
        private void SetRedTeam()
        {
            team = Team.red;
            playerIcon.color = Color.red;
            isRedTeam = true;
            int layerName = LayerMask.NameToLayer("Red Team");
            gameObject.layer = layerName;

            DominationPointManager._instance.redTeam.Add(transform.gameObject);
            
            GameObject capsule = transform.GetChild(0).gameObject;
            capsule.layer = layerName;
            capsule.tag = "Red Team";

            if(capsule.transform.GetChild(0) == null) { return; }
            capsule.transform.GetChild(0).gameObject.layer = layerName;
            capsule.transform.GetChild(0).tag = "Red Team";
        }
        #endregion

        #region Set Blue Team
        private void SetBlueTeam()
        {
            team = Team.blue;
            playerIcon.color = Color.blue;
            int layerName = LayerMask.NameToLayer("Blue Team");
            gameObject.layer = layerName;

            DominationPointManager._instance.blueTeam.Add(transform.gameObject);

            GameObject capsule = transform.GetChild(0).gameObject;
            capsule.layer = layerName;
            capsule.tag = "Blue Team";
            
            if(capsule.transform.GetChild(0) == null) { return; }
            capsule.transform.GetChild(0).gameObject.layer = layerName;
            capsule.transform.GetChild(0).tag = "Blue Team";
        }
        #endregion

        #region Set Random Team
        private void SetRandomTeam()
        {
            team = (Team)Random.Range(0f, 2f);

            if (team == Team.red)
            {
                if (DominationPointManager._instance.redTeam.Count > 10)
                {
                    SetBlueTeam();
                    return;
                }

                SetRedTeam();
            }
            else if (team == Team.blue)
            {
                if (DominationPointManager._instance.redTeam.Count > 10)
                {
                    SetRedTeam();
                    return;
                }

                SetBlueTeam();
            }
        }
        #endregion

    }

}