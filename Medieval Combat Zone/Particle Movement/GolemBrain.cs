using CombatZone.Bot;
using UnityEngine;
using UnityEngine.AI;

public class GolemBrain : BotFunctions
{

    #region Varables

    #region MentalState
    public enum MentalState { Chase, Attack, None }

    public MentalState mentalState = MentalState.None;
    #endregion

    #region Nav Mesh Agnet Variables
    [Header("Nav Mesh Agent Variables")]
    [SerializeField] private float agentSpeed = 10f;

    private NavMeshAgent agent;
    #endregion

    #region Animation
    [Header("Animation")]
    private Animator anim;

    private const string walk = "Walking";

    private const string attack = "Attacking";
    #endregion

    #region Targets
    [Header("Targets")]
    [SerializeField] private GameObject botTarget;

    [SerializeField] private float maxDist = 10f;

    [SerializeField] private float miniDist = 5f;
    #endregion

    #endregion

    /***************************************************************/

    #region Start, Update, Etc.

    #region Start
    void Start()
    {
        
        InitializeBot();

        //botTarget = CheckTeamClosetEnemy(PlayerStats.Team.red, PlayerStats.Team.red, DominationPointManager._instance.blueTeam, DominationPointManager._instance.redTeam);

    }
    #endregion

    #region Update
    void Update()
    {

        if(botTarget == null) { return; }

        float dist = CalcuateDistance(transform, botTarget.transform);

        if (dist > maxDist)
        {

            mentalState = MentalState.Chase;

            StateMachine(botTarget);

        }
        else if (dist < miniDist)
        {

            mentalState = MentalState.Attack;

            StateMachine(botTarget);

        }

    }
    #endregion

    #endregion

    /***************************************************************/

    #region Other Functions

    #region InitializeBot
    private void InitializeBot()
    {
        
        agent = GetComponent<NavMeshAgent>();

        agent.speed = agentSpeed;

        agent.stoppingDistance = miniDist;

        anim = GetComponent<Animator>();

    }
    #endregion

    #region StateMachine
    private void StateMachine(GameObject stateMachineTarget)
    {

        switch (mentalState)
        {

            case MentalState.Chase:
                Chase(agent, stateMachineTarget.transform);
                SetAnimation(walk, true);
                SetAnimation(attack, false);
                break;

            case MentalState.Attack:
                SetAnimation(walk, false);
                SetAnimation(attack, true);
                break;

            case MentalState.None:
                break;

        }

    }
    #endregion

    #region SetAnimation
    private void SetAnimation(string currentState, bool settingState)
    {

        anim.SetBool(currentState, settingState);

    }
    #endregion

    #endregion

}
