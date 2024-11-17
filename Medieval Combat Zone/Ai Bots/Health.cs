using UnityEngine;
using CombatZone.Damage;

namespace CombatZone.Bot
{

    public class Health : MonoBehaviour, IDamage
    {

        #region Variables

        public float maxHealth = 100f;

        public float currentHealth = 0f;

        public Animator anim;


        [SerializeField] private bool _isDead;

        public bool isDead
        {
            get => _isDead;
            set => _isDead = value;
        }

        private const string hurtAnimation = "Hit";

        #endregion

        //////////////////////////////////////////////////////////////

        #region Start Function
        void Start()
        {

            currentHealth = maxHealth;

            anim = GetComponent<Animator>();

        }
        #endregion

        #region Damage the Player
        public void TakeDamage(float damage)
        {

            currentHealth -= damage;

            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            anim.SetBool(hurtAnimation, true);

            if (currentHealth <= 0)
            {

                isDead = true;

                //Need to change to make it ragdoll and wait before respawning.
                Destroy(transform.root.gameObject);

            }

            Invoke("ResetAnimation", 1f);

        }

        private void ResetAnimation() { anim.SetBool(hurtAnimation, false); }
        #endregion

    } 

}
