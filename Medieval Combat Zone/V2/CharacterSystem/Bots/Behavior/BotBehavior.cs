using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CombatZone.Manager;

namespace CombatZone.Character.Bot
{

    [RequireComponent(typeof(PlayerStats))]
    public class BotBehavior : MonoBehaviour
    {

        private enum MentalState { Aggressive, Passive, Stealth, Climber, None }
        [SerializeField] private MentalState mentalState = MentalState.None;
        [SerializeField] private Behavior behavior = Behavior.None;

        [SerializeField] private MentalState previousState = MentalState.None;

        [Space()]

        #region Global Variables
        [Header("Global Variables")]
        [SerializeField] private float maxPercentage = 3.0f;
        [SerializeField] private float overlapSphereStartSize = 1f;
        [SerializeField] private float randomPointChance;

        int count = 0;
        float random = Mathf.Infinity;

        [SerializeField] float minRange = 1f;
        [SerializeField] float maxRange = 10f;
        [SerializeField] float distance = Mathf.Infinity;
        int randomIndex = 0;
        #endregion

        [Space()]

        #region Bot Properties
        [Header("Bot Properties")]
        [SerializeField] private BotBlackboard blackboard;
        [SerializeField] private GameObject botTarget;
        [SerializeField] private NavMeshAgent agent;
        public bool isMeleeBot;
        #endregion

        [Space()]

        #region Movement & Objective
        [Header("Movement & Objective")]
        [SerializeField] private Transform enemySpawn;
        [SerializeField] private Transform currentObjective;

        [SerializeField] private bool inDominationPoint = false;
        [SerializeField] private bool inEnemySpawn = false;

        [SerializeField] private bool isClimbing = false;
        [SerializeField] private bool isCrouching = false;
        #endregion

        [Space()]

        #region Field Of View
        [Header("FieldOfView")]
        public FOVScriptableObject fov;
        [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Start
        void Start()
        {
            InitializeBot();
            RandomMentalState();
            StateMachineMentalState();
            StartCoroutine(FindTargetsWithDelay(0.5f));
        }
        #endregion

        #region Initialize bot
        private void InitializeBot()
        {
            blackboard = GetComponent<BotBlackboard>();
            agent = GetComponent<NavMeshAgent>();

            if (agent == null || blackboard == null)
            {
                Debug.Log("Missing one or more of the three required Components: " +
                    "Agent, Blackboard");
                this.enabled = false;
                return;
            }

            enemySpawn = blackboard.BotData.GetOpponentSpawnPoint(blackboard.PlayerStats);

            agent.speed = blackboard.BotData.agentSpeed;
            agent.stoppingDistance = blackboard.BotData.agentStopDist;
            agent.updateRotation = true;
        }
        #endregion

        #region RandomMentalState
        public void RandomMentalState()
        {
            mentalState = (MentalState)Random.Range(0, 3);
            previousState = mentalState;
        }
        #endregion

        #region Update
        private void Update()
        {
            if (blackboard.BotHealth.isDead) 
            { 
                agent.isStopped = true;
                behavior = Behavior.Dead;
                return; 
            }
            StateMachineMentalState();
        }
        #endregion

        /**************** State Machine Handler Functions ****************/

        #region StateMachineMentalState
        private void StateMachineMentalState()
        {
            switch (mentalState)
            {
                case MentalState.None:
                    break;

                case MentalState.Aggressive:
                    HandleAggressiveState();
                    break;

                case MentalState.Passive:
                    HandlePassiveState();
                    break;

                case MentalState.Stealth:
                    HandleStealthState();
                    break;

                case MentalState.Climber:
                    HandleClimbState(overlapSphereStartSize, blackboard.BotData.climbtPoints);
                    break;
            }
        }
        #endregion

        #region StateMachineBehaviour
        public void StateMachineBehavior(Behavior behaviorState, Transform stateMachineTarget = null)
        {
            behavior = behaviorState;

            switch (behavior)
            {
                case Behavior.None:
                    break;

                case Behavior.Chase:
                    if(stateMachineTarget == null) { return; }
                    blackboard.BotData.Chase(agent, stateMachineTarget);
                    break;

                case Behavior.Teabag:
                    break;

                case Behavior.Retreat:
                    HandleRetreatState(overlapSphereStartSize,
                        blackboard.BotData.retreatPoints);
                    break;

                case Behavior.UseAbility:
                    break;

                case Behavior.Crouch:
                    blackboard.BotData.Crouch(ref isCrouching, transform, blackboard.CapsuleRb, 0.5f, 1f);
                    break;

                case Behavior.Dead:
                    break;
            }
        }
        #endregion

        /**************** Mental State Handlers ****************/

        #region HandleAggressiveState
        private void HandleAggressiveState()
        {
            botTarget = blackboard.BotData.CheckTeamClosetEnemy(transform, blackboard.PlayerStats, Team.red, DominationPointManager._instance.blueTeam, DominationPointManager._instance.redTeam);
            if (botTarget == null) { return; }

            distance = blackboard.BotData.CalcuateDistance(transform, botTarget.transform);

            if (distance > 10f)
            {
                blackboard.BotData.Watch(transform, botTarget.transform, blackboard.BotData.rotationSpeed);
                StateMachineBehavior(Behavior.Chase, botTarget?.transform);
                agent.isStopped = false;
                blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 1f);
            }

            BotYield(distance, 10f);
        }
        #endregion

        #region HandlePassiveState
        private void HandlePassiveState()
        {
            if (blackboard.BotData.AreAllObhjectivesSecured(DominationPointManager._instance.objectives, blackboard.PlayerStats))
            {
                //RandomMentalState(); 
                mentalState = MentalState.Aggressive;
                return;
            }

            if (IsObjectiveClaimed(ref currentObjective) || currentObjective == null)
            {
                SetCurrentObjective();
                count = 0;
            }

            if(count < 1)
            {
                random = Random.Range(minRange, maxRange);
                agent.stoppingDistance = random;
                count++;
            }

            distance = blackboard.BotData.CalcuateDistance(transform, currentObjective);

            if (distance > random)
            {
                agent.isStopped = false;
                StateMachineBehavior(Behavior.Chase, currentObjective?.transform);
                blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 1f);
            }

            BotYield(distance, random);
        }
        #endregion

        #region HandleStealthState
        private void HandleStealthState()
        {
            if (IsObjectiveClaimed(ref currentObjective) || currentObjective == null)
            {
                SetCurrentObjective();
            }

            Transform objective = !inDominationPoint ? currentObjective : enemySpawn;
            
            if(objective == null) { return; }
            distance = blackboard.BotData.CalcuateDistance(transform, objective);

            if (inDominationPoint && inEnemySpawn)
            {
                previousState = mentalState;
                mentalState = MentalState.Aggressive;
                StateMachineBehavior(Behavior.Chase, botTarget?.transform);
                return;
            }

            bool tooFarFromDominationPoint = !inDominationPoint && distance > 20f;
            bool tooFarFromEnemySpawn = !inEnemySpawn && distance > 10f;

            if(tooFarFromDominationPoint || tooFarFromEnemySpawn)
            {
                StateMachineBehavior(Behavior.Chase, objective.transform);
                blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 1f);
                return;
            }

            if (!inDominationPoint && distance <= 20f)
            {
                inDominationPoint = true;
            }
            else if(!inEnemySpawn && distance <= 10f)
            {
                inEnemySpawn = true;
            }

        }
        #endregion

        #region Handle Climb State
        private void HandleClimbState(float sphereSize, LayerMask climbMask)
        {
            Collider[] rays = Physics.OverlapSphere(transform.position, sphereSize, climbMask);
            if(rays.Length <= 0)
            {
                overlapSphereStartSize = ExponentialGrowth(sphereSize);
                return;
            }

            isClimbing = true;
            Transform closestPoint = GetClosestPoint(rays, sphereSize);
            if (closestPoint == null) { return; }
            behavior = Behavior.Chase;
            StateMachineBehavior(Behavior.Chase, closestPoint);
            blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 1f);
            distance = blackboard.BotData.CalcuateDistance(transform, closestPoint);
            BotYield(distance, minRange);
        }
        #endregion

        #region Handle Retreat State
        private void HandleRetreatState(float sphereSize, LayerMask retreatMask)
        {
            Collider[] rays = Physics.OverlapSphere(transform.position, sphereSize, retreatMask);
            if (rays.Length <= 0)
            {
                overlapSphereStartSize = ExponentialGrowth(sphereSize);
                return;
            }

            if(count < 1)
            {
                randomPointChance = Mathf.Round(Random.Range(0.0f, maxPercentage));
                randomIndex = Mathf.RoundToInt(Random.Range(0.0f, Mathf.Min(1f, rays.Length - 1)));
                count++;
            }
            
            if (randomPointChance >= 2)
            {
                distance = blackboard.BotData.CalcuateDistance(transform, rays[randomIndex].transform);
                StateMachineBehavior(Behavior.Chase, rays[randomIndex].transform);
                blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 1f);
                BotYield(distance, minRange);
            }
            else
            {
                Transform closestPoint = GetClosestPoint(rays, sphereSize);
                if (closestPoint == null) { return; }
                distance = blackboard.BotData.CalcuateDistance(transform, closestPoint);
                StateMachineBehavior(Behavior.Chase, closestPoint);
                blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 1f);
                BotYield(distance, minRange);
            }
    
        }
        #endregion

        /**************** Field of View Functions ****************/

        #region DirFromAngle
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y; }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
        #endregion

        #region FindVisibleTargets
        private void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, fov.viewRadius, fov.targetMask);
            Transform closestTarget = null;
            float closestDist = Mathf.Infinity;

            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < fov.viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);
                    if (distToTarget < closestDist && !Physics.Raycast(transform.position, dirToTarget, distToTarget, fov.obstacleMask))
                    {
                        closestDist = distToTarget;
                        closestTarget = target;
                    }
                }
            }

            if (closestTarget != null)
            {
                visibleTargets.Add(closestTarget);
                botTarget = closestTarget.gameObject;
                agent.updateRotation = false;
                blackboard.BotData.Watch(transform, closestTarget, blackboard.BotData.rotationSpeed);

                AttackType();
            }
            else
            {
                agent.updateRotation = true;
            }
        }
        #endregion

        #region FindTargetWithDelay
        private IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }
        #endregion

        /**************** Other Methods ****************/

        #region Bot Yield
        private void BotYield(float dist, float min)
        {
            agent.stoppingDistance = min;

            if (dist < min)
            {
                agent.isStopped = true;
                agent.ResetPath();
                behavior = Behavior.None;
                StateMachineBehavior(Behavior.None);
                blackboard.BotAnimations.SetWalkSpeed(blackboard.CharacterAnimator, blackboard.BotAnimations.ForwardWalk, 0f);
                previousState = mentalState;
                AttackType();
            }
        }
        #endregion

        #region Previous Mental State
        private void PreviousMentalState()
        {
            mentalState = previousState;
        }
        #endregion

        #region Get Closest Point
        private Transform GetClosestPoint(Collider[] points, float maxRange)
        {
            Transform closet = null;
            float closestDist = Mathf.Infinity;

            foreach (Collider col in points)
            {
                float dist = blackboard.BotData.CalcuateDistance(transform, col.transform);
                if (dist < maxRange && dist < closestDist)
                {
                    closet = col.transform;
                    closestDist = dist;
                }
            }
            return closet;
        }
        #endregion

        #region Set Current Objective
        private void SetCurrentObjective()
        {
            currentObjective = blackboard.BotData.SelectRandomObjective(DominationPointManager._instance.objectives);
        }
        #endregion

        #region Exponential Growth
        private float ExponentialGrowth(float currentSize, float exponent = 2f)
        {
            return currentSize *= exponent;
        }
        #endregion

        #region Is Objective Claimed
        private bool IsObjectiveClaimed(ref Transform objective)
        {
            if(objective == null) { return false; }

            DominationPoint point = objective.GetComponent<DominationPoint>();

            if (blackboard.PlayerStats.isRedTeam)
            {
                return point.isRedTeamPoint;
            }
            else
            {
                return point.isBlueTeamPoint;
            }
        }

        #endregion

        #region Which Attack Type
        private void AttackType()
        {
            if (!isMeleeBot) { blackboard.BotShooting.Shooting(); }
            else
            {
                blackboard.BotAnimations.SetAction(blackboard.CharacterAnimator, blackboard.BotAnimations.Attacking, true);
                Invoke("ResetAttack", 1f);
            }
        }
        #endregion

        #region Reset Attack
        private void ResetAttack()
        {
            blackboard.BotAnimations.SetAction(blackboard.CharacterAnimator, blackboard.BotAnimations.Attacking);
        }
        #endregion

    }

}