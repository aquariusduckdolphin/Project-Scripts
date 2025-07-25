using System.Collections.Generic;
using UnityEngine;
using CombatZone.Objective; 

public class PlayerStats : MonoBehaviour
{

    #region Variables

    #region Team
    [Header("Team")]
    public Team team = Team.none;

    public  enum Team { red = 0, blue = 1, none = 2 }

    [SerializeField] public bool isRedTeam = false; 
    #endregion

    #region Name List
    [Header("Name List")]
    private List<string> names = new List<string>()
    {

         "Susan Pierpoint", "StanBarry", "Bob Day", "John32123", "Lauren_090JK", "Morgan$90112-", "Freeman", "Lee"

    };
    #endregion

    #region Stats
    [Header("Stats")]
    public string playerName;

    public int kill;

    public int death;

    public int objectScore;
    #endregion

    #endregion

    /***************************************************************/

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

    /***************************************************************/

    #region Set Teams

    #region Set Red Team
    private void SetRedTeam()
    {

        team = Team.red;

        DominationPointManager._instance.redTeam.Add(transform.gameObject);

    }
    #endregion

    #region Set Blue Team
    private void SetBlueTeam()
    {

        team = Team.blue;

        DominationPointManager._instance.blueTeam.Add(transform.gameObject);

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

                Destroy(gameObject);

                return;

            }

            DominationPointManager._instance.redTeam.Add(transform.gameObject);

        }
        else if (team == Team.blue)
        {

            if (DominationPointManager._instance.redTeam.Count > 10)
            {

                Destroy(gameObject);

                return;
            }

            DominationPointManager._instance.blueTeam.Add(transform.gameObject);

        }

    }
    #endregion

    #endregion

    /***************************************************************/

}
