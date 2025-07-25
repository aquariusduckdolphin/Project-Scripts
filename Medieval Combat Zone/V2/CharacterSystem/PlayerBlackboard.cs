using CombatZone.Character;
using CombatZone.Character.Player;
using CombatZone.Ragdolling;
using UnityEngine;
using UnityEngine.UI;

namespace CombatZone
{
    public class PlayerBlackboard : MonoBehaviour
    {

        [SerializeField] private Animator animator;
        public Animator CharacterAnimator 
        { 
            get => animator; 
            private set => animator = value; 
        }

        [SerializeField] private PlayerHealth playerHealth;
        public PlayerHealth PlayerHealth 
        { 
            get => playerHealth; 
            private set => playerHealth = value; 
        }

        [SerializeField] private Ragdoll ragdollController;
        public Ragdoll RagdollController 
        { 
            get => ragdollController; 
            private set => ragdollController = value; 
        }

        [SerializeField] private PlayerStats playerStats;
        public PlayerStats PlayerStats
        {
            get => playerStats;
            private set => playerStats = value;
        }

        [SerializeField] private Image healthBar;
        public Image HealthBar
        {
            get => healthBar;
            private set => healthBar = value;
        }

        [SerializeField] private Material healthBarMaterial;
        public Material HealthBarMaterial
        {
            get => healthBarMaterial;
            private set => healthBarMaterial = value;
        }

        void Start()
        {
            Initialized();
        }

        private void Initialized()
        {
            playerStats = GetComponent<PlayerStats>();

            healthBarMaterial = transform.GetChild(2).transform.GetChild(4).transform.GetComponent<Material>();
            if(healthBarMaterial != null )
            {
                Debug.Log("Got it");
            }
        } 

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
