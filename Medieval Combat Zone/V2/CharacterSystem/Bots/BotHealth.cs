using CombatZone.Spawner;
using UnityEngine;

namespace CombatZone.Character.Bot
{

    public class BotHealth : BaseHealth
    {

        [SerializeField] private BotBehavior behavior;
        [SerializeField] private BotBlackboard blackboard;
        [SerializeField] private float respawnDelay = 1f;

        /**************** Start, Update, Etc. ****************/

        public void GatherHealtComponents()
        {
            ResetHealth();
            blackboard = transform.root.GetComponent<BotBlackboard>();
            animator = blackboard.CharacterAnimator;
            ragdoll = blackboard.RagdollController;
            behavior = blackboard.BotBehavior;
        }

        #region Update
        private void Update()
        {
            if (currentHealth <= 50f)
            { behavior.StateMachineBehavior(Behavior.Retreat); }

            if (isDead)
            {
                TestRespawn();
                behavior.RandomMentalState();
                StartCoroutine(ResetCharacter(respawnDelay));
                ResetHealth();
            }
        }
        #endregion

        private void TestRespawn()
        {
            botStats = transform.root.GetComponent<PlayerStats>();
            if (botStats.team == Team.red)
            {
                spawn = GameObject.FindGameObjectWithTag("Red Team Spawn");
            }
            else
            {
                spawn = GameObject.FindGameObjectWithTag("Blue Team Spawn");
            }
            Spawners spawner = spawn.GetComponent<Spawners>();
            if (spawner == null) { return; }
            spawner.GenerateBotSpawnLocation();
        }

    }

}