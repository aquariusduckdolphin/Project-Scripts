using UnityEngine;

namespace CombatZone.Character.Bot
{

    public class Health : BaseHealth
    {

        private BotBehavior behavior;
        [SerializeField] private float respawnDelay = 1f;

        /**************** Start, Update, Etc. ****************/

        #region Start Function
        void Start()
        {
            ResetHealth();
            behavior = gameObject.transform.parent.GetComponent<BotBehavior>();
        }
        #endregion

        #region Update
        private void Update()
        {
            if(currentHealth <= 50f)
            { behavior.StateMachineBehavior(Behavior.Retreat); }

            if (isDead) 
            {
                //TestRespawn();
                behavior.RandomMentalState();
                StartCoroutine(ResetCharacter(respawnDelay));
                ResetHealth();
            }
        }
        #endregion

    } 

}
