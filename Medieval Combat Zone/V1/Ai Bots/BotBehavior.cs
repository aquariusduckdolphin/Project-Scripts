using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CombatZone.Objective;
using CombatZone.Damage;

namespace CombatZone.Bot
{

    public class BotBehavior : BotFunctions
    {

        #region Varables

        #region Mental State & Behavior
        [Header("Mental State & Behavior")]
        [SerializeField] private MentalState mentalState = MentalState.None;

        private enum MentalState { Aggressive, Passive, Stealth, Climber, None }

        [SerializeField] private Behavior mentalBehavior = Behavior.None;

        private enum Behavior { Chase, Attack, UseAbility, Teabag, Retreat, None }
        #endregion

        #region Bot Properties
        [Header("Bot Properties")]
        [SerializeField] private BotData botData;

        private PlayerStats playerStats;

        public Health health;

        private NavMeshAgent agent;

        private GameObject botTarget;

        private Transform enemySpawn;

        private Transform currentObjective;

        private bool inDominationPoint = false;

        private bool inEnemySpawn = false;

        private bool retreating = false;

        private List<GameObject> climbPoints = new List<GameObject>();

        private List<GameObject> retreatPoints = new List<GameObject>();
        #endregion

        #region Field Of View
        [Header("FieldOfView")]
        public FOVScriptableObject fov;

        [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();
        #endregion

        #region Shooting Properties
        [Header("Shooting Properties")]
        [SerializeField] private int bulletLeft = 1;

        private int bulletsShot;

        [SerializeField] private bool readyToShoot = true;

        [SerializeField] private bool reloading;

        private int currentBulletPrefabType = 0;

        [SerializeField] private float delayShot;

        [SerializeField] private Vector3 shootOffset;
        #endregion

        #region Animations
        [Header("Animations")]
        public Animator anim;

        private const string attacking = "Attack";

        private const string walking = "Vertical";
        #endregion

        #endregion

        /***************************************************************/

        #region Start, Update, Etc.

        #region Start
        void Start()
        {

            InitializeVariables();

            readyToShoot = true;

            //RandomMentalState();

            //StartCoroutine(MyUpdate(0.5f));

            mentalState = MentalState.Stealth;

            //botTarget = CheckTeamClosetEnemy(playerStats, PlayerStats.Team.red, DominationPointManager._instance.blueTeam, DominationPointManager._instance.redTeam, true);

            currentObjective = DominationPointManager._instance.dominationPoints[0].transform;

            StateMachineMentalState(playerStats);

            StartCoroutine(FindTargetsWithDelay(0.5f));

        }
        #endregion

        #region Initialize bot
        private void InitializeVariables()
        {

            agent = GetComponent<NavMeshAgent>();

            playerStats = GetComponent<PlayerStats>();

            enemySpawn = GetOpponentSpawnPoint(playerStats);

            GatherObjectives("Point", climbPoints);

            GatherObjectives("Retreat", retreatPoints);

            if (agent == null || botData == null) { return; }

            agent.speed = botData.agentSpeed;

            agent.stoppingDistance = botData.agentStopDist;

            agent.updateRotation = true;

        }
        #endregion

        #region RandomMentalState
        private void RandomMentalState()
        {

            mentalState = (MentalState)Random.Range(0, 3);

            if (mentalState == MentalState.Passive || mentalState == MentalState.Stealth)
            {

                currentObjective = SelectRandomObjective(DominationPointManager._instance.dominationPoints);

            }

        }
        #endregion

        #region Update
        private void Update()
        {

            if (health.isDead) 
            { 

                agent.isStopped = true; 
                
                return; 

            }

            StateMachineMentalState(playerStats);

        }
        #endregion


        #endregion

        /***************************************************************/

        #region State Machine Handler Functions

        #region StateMachineMentalState
        private void StateMachineMentalState(PlayerStats playerStats)
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
                    HandleClimbState();
                    RandomMentalState();
                    break;

            }

        }
        #endregion

        #region HandlerAggressiveState
        private void HandleAggressiveState()
        {

            Debug.Log("StartAggresive");

            botTarget = CheckTeamClosetEnemy(playerStats, PlayerStats.Team.red, DominationPointManager._instance.blueTeam, DominationPointManager._instance.redTeam);

            if (botTarget != null || CalcuateDistance(transform, botTarget.transform) > 10f)
            {

                agent.isStopped = false;

                Watch(botTarget.transform, botData.rotationSpeed);

                StateMachineBehavior(Behavior.Chase, botTarget?.transform);

            }
            else
            {

                agent.isStopped = true;

                anim.SetFloat(walking, 0f);

            }

            Debug.Log("EndAggresive");

        }
        #endregion

        #region HandlePassiveState
        private void HandlePassiveState()
        {

            float random = Random.Range(0.1f, 16f);

            if (CalcuateDistance(transform, currentObjective) > random)
            {

                StateMachineBehavior(Behavior.Chase, currentObjective.transform);

            }
            else
            {

                agent.isStopped = true;

                anim.SetFloat(walking, 0f);

            }

        }
        #endregion

        #region HandleStealthState
        private void HandleStealthState()
        {

            Transform objective = !inDominationPoint ? currentObjective : enemySpawn;

            float dist = CalcuateDistance(transform, objective);

            if (dist > 20 && !inDominationPoint || dist > 10 && !inEnemySpawn)
            {

                StateMachineBehavior(Behavior.Chase, objective.transform);

            }
            else
            {

                if (!inDominationPoint) { inDominationPoint = true; }
                else if (!inEnemySpawn) { inEnemySpawn = true; }

                if (inDominationPoint && inEnemySpawn)
                {

                    botTarget = CheckTeamClosetEnemy(playerStats, PlayerStats.Team.red, DominationPointManager._instance.blueTeam, DominationPointManager._instance.redTeam);

                    StateMachineBehavior(Behavior.Chase, botTarget.transform);

                }

            }

        }
        #endregion

        #region HandleClimbState
        private void HandleClimbState()
        {

            Collider[] rays = Physics.OverlapSphere(transform.position, fov.viewRadius);

            Transform point = null;

            if (rays.Length <= 0) { return; }

            for (int i = 0; i < rays.Length; i++)
            {

                float closestPoint = Mathf.Infinity;

                float dist = CalcuateDistance(transform, climbPoints[i].transform);

                if (dist < fov.viewRadius && dist < closestPoint)
                {

                    closestPoint = dist;

                    point = retreatPoints[i].transform;

                }

            }

            StateMachineBehavior(Behavior.Chase, point);

        }
        #endregion

        #region StateMachineBehaviour
        private void StateMachineBehavior(Behavior behaviorState, Transform stateMachineTarget)
        {

            mentalBehavior = behaviorState;

            switch (mentalBehavior)
            {

                case Behavior.None:
                    break;

                case Behavior.Chase:
                    Chase(agent, stateMachineTarget);
                    anim.SetFloat(walking, 1f);
                    break;

                case Behavior.Attack:
                    break;

                case Behavior.Teabag:
                    break;

                case Behavior.Retreat:
                    retreating = true;
                    if (retreating) { Debug.Log("Retreating"); }
                    break;

                case Behavior.UseAbility:
                    break;

            }

        }
        #endregion

        #endregion

        /***************************************************************/

        #region Shooting & Reloading Functions

        #region Shooting
        private void Shooting()
        {

            if (bulletLeft > 0 && readyToShoot)
            {

                delayShot -= Time.deltaTime;

                if (delayShot <= 0)
                {

                    delayShot = botData.currentDelayShot;

                    Invoke("Shoot", botData.timeBetweenShots);

                }

            }
            else if (bulletLeft <= 0 && !reloading)
            {

                Invoke("Reload", botData.reloadTime);

            }

        }
        #endregion

        #region Shoot
        private void Shoot()
        {

            print("Shooting");

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, fov.viewRadius) && 
                (hit.collider.CompareTag("Red Team") || hit.collider.CompareTag("Blue Team")))
            {

                IDamage damageAmount = hit.transform.GetComponent<IDamage>();

                damageAmount?.TakeDamage(10f);

                currentBulletPrefabType = Random.Range(0, 4);

                Vector3 newPos = transform.position + shootOffset;

                GameObject bullet = Instantiate(botData.bulletPrefab[currentBulletPrefabType], newPos, Quaternion.identity);

                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * botData.bulletForce, ForceMode.Impulse);
                
                anim.SetBool(attacking, true);

            }

            bulletLeft--;

            bulletsShot--;

        }
        #endregion

        #region Reload
        private void Reload()
        {

            Debug.Log("Reloading");

            readyToShoot = false;

            reloading = true;

            Invoke("ReloadFinished", botData.reloadTime);

        }
        #endregion

        #region ReloadFinished
        private void ReloadFinished()
        {

            Debug.Log("Finished Reloading");

            bulletLeft = botData.magazineSize;

            reloading = false;

            readyToShoot = true;

        }
        #endregion

        #endregion

        /***************************************************************/

        #region Field of View Functions

        #region DirFromAngle
        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {

            if (!angleIsGlobal) { angleInDegrees += transform.eulerAngles.y; }

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));

        }
        #endregion

        #region FindVisibleTargets
        void FindVisibleTargets()
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

                Watch(closestTarget, botData.rotationSpeed);

                Shooting();

            }
            else
            {

                agent.updateRotation = true;

                anim.SetBool(attacking, false);

            }

        }
        #endregion

        #region FindTargetWithDelay
        IEnumerator FindTargetsWithDelay(float delay)
        {

            while (true)
            {

                yield return new WaitForSeconds(delay);

                FindVisibleTargets();

            }

        }
        #endregion

        #endregion

        /***************************************************************/

        #region Other Functions

        #region MyUpdate
        /*private IEnumerator MyUpdate(float delay)
        {

            while (true)
            {

                yield return new WaitForSeconds(delay);

                StartCoroutine(MyUpdate(delay));

            }

        }*/
        #endregion

        #endregion

        /***************************************************************/

    }

}
