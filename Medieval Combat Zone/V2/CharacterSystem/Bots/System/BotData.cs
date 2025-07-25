using CombatZone.Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CombatZone.Character.Bot
{

    [CreateAssetMenu(fileName = "BotData", menuName = "Bots/BotData")]
    public class BotData : ScriptableObject
    {

        [Header("Agent Properties")]
        public float rotationSpeed = 10f;
        public float agentSpeed = 100f;
        public float agentStopDist = 2f;

        [Space]

        /**************** Shooting Properties ****************/

        [Header("Static Shooting Properties")]
        public float timeBetweenShots = 0.25f;
        public float currentDelayShot;
        public float reloadTime = 2f;
        public float bulletForce = 100f;
        public int magazineSize = 1;
        public Vector3 shootOffset;
        public GameObject[] bulletPrefab = new GameObject[4];

        [Space]

        /**************** Climb & Retreat Properties ****************/

        [Header("Climb & Retreat Properties")]
        public LayerMask climbtPoints;
        public LayerMask retreatPoints;

        /**************** Bot Methods ****************/

        #region  Find enemy spawn
        public Transform GetOpponentSpawnPoint(PlayerStats playerStats)
        {
            if (playerStats == null) { return null; }
            string spawnTag = playerStats.isRedTeam ? "Blue Team Spawn" : "Red Team Spawn";
            return GameObject.FindGameObjectWithTag(spawnTag)?.transform;
        }
        #endregion

        #region Find Enemy
        public GameObject FindEnemy(List<GameObject> potentialEnemies, bool findClosest, Transform thisBot)
        {
            GameObject selectedEnemy = null;
            float closetDist = findClosest ? Mathf.Infinity : 0f;

            foreach (GameObject enemy in potentialEnemies)
            {
                float dist = Vector3.Distance(enemy.transform.position, thisBot.position);
                if ((findClosest && dist < closetDist) || (!findClosest && dist > closetDist))
                {
                    selectedEnemy = enemy;
                    closetDist = dist;
                }
            }

            return selectedEnemy;
        }
        #endregion

        #region Check Team Closet Enemey
        public GameObject CheckTeamClosetEnemy(Transform thisBot, PlayerStats playerStats, Team teamSide, List<GameObject> blueTeam, List<GameObject> redTeam, bool findClosest = true)
        {
            if(redTeam.Count <= 0 || blueTeam.Count <= 0) { return null; } 
            var targetList = playerStats.team == teamSide ? blueTeam : redTeam;
            Debug.Log(targetList);
            return FindEnemy(targetList, findClosest, thisBot);
        }
        #endregion

        #region Random Objective 
        public Transform SelectRandomObjective(List<GameObject> objectives)
        {
            if (objectives.Count == 0) { return null; }
            int randomPoint = Random.Range(0, objectives.Count);
            return objectives[randomPoint]?.transform;
        }
        #endregion

        #region Are All Objectives Secured
        public bool AreAllObhjectivesSecured(List<GameObject> objectives, PlayerStats stat)
        {
            if (objectives.Count == 0) { return false; }
            foreach (GameObject objective in objectives)
            {
                DominationPoint point = objective.GetComponent<DominationPoint>();
                if (stat.isRedTeam && !point.isRedTeamPoint)
                {
                    return false;
                }
                else if(!stat.isRedTeam && !point.isBlueTeamPoint)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        /**************** Bot Behavior Methods ****************/

        #region Chase Function
        public void Chase(NavMeshAgent bot, Transform chaseTarget)
        {
            if(chaseTarget == null) { return; }
            bot.SetDestination(chaseTarget.position);
        }
        #endregion

        #region Watch Function
        public void Watch(Transform thisBot, Transform watchTarget, float rotateSpeed)
        {
            Vector3 targetDir = watchTarget.transform.position - thisBot.position;
            targetDir.y = 0f;
            Quaternion rotate = Quaternion.LookRotation(targetDir);
            thisBot.rotation = Quaternion.Slerp(thisBot.rotation, rotate, rotateSpeed * Time.deltaTime);
        }
        #endregion

        /**************** Calculate Distance ****************/

        #region Calculate Distance
        public float CalcuateDistance(Transform bot, Transform target)
        {
            if(bot == null || target == null)
            {
                Debug.Log("Missing");
                return Mathf.Infinity;
            }
            return Vector3.Distance(bot.position, target.transform.position);
        }
        #endregion

        /**************** Crouch ****************/

        #region Crouch Function
        public Transform Crouch(ref bool isCrouching, Transform localTransfrom, Rigidbody rb, float crouchYScale, float startYScale)
        {
            if (!isCrouching)
            {
                isCrouching = false;
                localTransfrom.localScale = new Vector3(localTransfrom.localScale.x, startYScale, localTransfrom.localScale.z);
            }

            if (isCrouching)
            {
                isCrouching = true;
                localTransfrom.localScale = new Vector3(localTransfrom.localScale.x, crouchYScale, localTransfrom.localScale.z);
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }

            return localTransfrom;
        }
        #endregion

    }

}
