using CombatZone.Interfaces;
using CombatZone.Ragdolling;
using CombatZone.Spawner;
using System.Collections;
using UnityEngine;

namespace CombatZone.Character
{

    public abstract class BaseHealth : MonoBehaviour, IHealth
    {

        [SerializeField] protected float _maxHealth = 100f;
        public float maxHealth 
        { 
            get => _maxHealth; 
            private set => _maxHealth = value; 
        }

        [SerializeField] protected float _currentHealth = 0f;
        public float currentHealth 
        { 
            get => _currentHealth;
            private set => _currentHealth = Mathf.Clamp(value, 0f, maxHealth); 
        }

        public bool isDead { get; protected set; }

        [SerializeField] protected Animator animator;
        [SerializeField] protected Ragdoll ragdoll;

        [SerializeField] protected float delay = 10f;
        [SerializeField] protected float delayBetweenHealthRegen = 5f;

        protected const string hurtAnimation = "Hit";

        protected Coroutine giveHealth;

        protected PlayerStats botStats;
        protected GameObject spawn;


        /**************** Health Methods ****************/

        /*protected void TestRespawn()
        {
            botStats = GetComponent<PlayerStats>();
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
        }*/

        #region Reset Health
        protected void ResetHealth()
        {
            currentHealth = maxHealth;
            isDead = false;
        }
        #endregion

        #region Take Damage
        public void TakeDamage(float damage)
        {
            
            currentHealth -= damage;
            animator?.SetBool(hurtAnimation, true);

            if(giveHealth != null)
            {
                StopCoroutine(giveHealth);
                giveHealth = null;
            }

            if (currentHealth <= 0)
            {
                isDead = true;
                StartCoroutine(ResetCharacter(delay));
            }

            Invoke("ResetHurtAnimation", 1f);
            giveHealth = StartCoroutine(GiveHealth(delay));

        }
        #endregion

        #region Reset Hurt Animation
        protected void ResetHurtAnimation()
        {
            animator.SetBool(hurtAnimation, false);
        }
        #endregion

        #region Reset Character

        protected IEnumerator ResetCharacter(float delay)
        {
            animator.enabled = false;
            ragdoll.ToggleRagdoll(true);
            yield return new WaitForSeconds(delay);
            ragdoll.ToggleRagdoll(false);
            animator.enabled = true;
        }
        #endregion

        #region Give Health
        protected IEnumerator GiveHealth(float delay)
        {

            yield return new WaitForSeconds(delay);

            while (currentHealth < maxHealth)
            {
                currentHealth += 10f;
                yield return new WaitForSeconds(delayBetweenHealthRegen);
            }

            StopCoroutine(giveHealth);
            giveHealth = null;

        }
        #endregion

        #region Tick Damage
        public IEnumerator TickDamage(float damage, float delay, bool infiniteLoop, float loopAmount)
        {

            if (infiniteLoop)
            {
                while (true)
                {
                    currentHealth -= damage;
                    yield return new WaitForSeconds(delay);
                }
            }
            else
            {
                for(int i = 0; i <= loopAmount; i++)
                {
                    currentHealth -= damage;
                    yield return new WaitForSeconds(delay);
                }
            }

        }
        #endregion

    }

}