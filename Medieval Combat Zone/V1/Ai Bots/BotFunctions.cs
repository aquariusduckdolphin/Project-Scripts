using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CombatZone.Bot
{

    public abstract class BotFunctions : MonoBehaviour
    {

        #region Find all the objectives
        protected void GatherObjectives(string objectiveTagName, List<GameObject> objectives)
        {

            objectives.Clear();

            var foundObjectives = GameObject.FindGameObjectsWithTag(objectiveTagName);

            foreach (var objective in foundObjectives)
            {

                if (objective != null) { objectives.Add(objective.gameObject); }

            }

        }
        #endregion

        #region  Find enemy spawn
        protected Transform GetOpponentSpawnPoint(PlayerStats playerStats)
        {

            if (playerStats == null) { return null; }

            string spawnTag = playerStats.isRedTeam ? "Blue Team Spawn" : "Red Team Spawn";

            return GameObject.FindGameObjectWithTag(spawnTag)?.transform;

        }
        #endregion

        #region Find Enemies
        protected GameObject FindEnemy(List<GameObject> potentialEnemies, bool findClosest)
        {

            GameObject selectedEnemy = null;

            float closetDist = findClosest ? Mathf.Infinity : 0f;

            foreach (GameObject enemy in potentialEnemies)
            {

                float dist = Vector3.Distance(enemy.transform.position, transform.position);

                if ((findClosest && dist < closetDist) || (!findClosest && dist > closetDist))
                {

                    selectedEnemy = enemy;
                    closetDist = dist;

                }

            }

            return selectedEnemy;

        }

        protected GameObject CheckTeamClosetEnemy(PlayerStats playerStats, PlayerStats.Team teamSide, List<GameObject> blueTeam, List<GameObject> redTeam, bool findClosest = true)
        {

            var targetList = playerStats.team == teamSide ? blueTeam : redTeam;

            Debug.Log(targetList);

            return FindEnemy(targetList, findClosest);

        }
        #endregion

        #region Random Objective 
        protected Transform SelectRandomObjective(List<GameObject> objectives)
        {

            if (objectives.Count == 0) { return null; }

            int randomPoint = Random.Range(0, objectives.Count);

            return objectives[randomPoint]?.transform;

        }
        #endregion

        /***************************************************************/

        #region Chase Function
        protected void Chase(NavMeshAgent bot, Transform chaseTarget)
        {

            bot.SetDestination(chaseTarget.position);

        }
        #endregion

        #region Watch Function
        protected void Watch(Transform watchTarget, float rotateSpeed)
        {

            Vector3 targetDir = watchTarget.transform.position - transform.position;

            targetDir.y = 0f;

            Quaternion rotate = Quaternion.LookRotation(targetDir);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);

        }
        #endregion

        /***************************************************************/

        #region Calculate Distance
        protected float CalcuateDistance(Transform bot, Transform target)
        {

            return Vector3.Distance(bot.position, target.transform.position);

        }
        #endregion

        /***************************************************************/

        #region Crouch Function
        public void Crouch(Transform localTransfrom, bool isCrouching, Rigidbody rb, float crouchYScale, float startYScale)
        {

            if (!isCrouching)
            {

                isCrouching = true;

                localTransfrom.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            }

            if (isCrouching)
            {

                isCrouching = false;

                localTransfrom.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

            }

        }
        #endregion

    }

}