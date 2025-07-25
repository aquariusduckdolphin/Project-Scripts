using CombatZone.Character;
using CombatZone.Character.Bot;
using CombatZone.Ragdolling;
using UnityEngine;

namespace CombatZone
{
    public class BotBlackboard : MonoBehaviour
    {

        [Header("Scripts")]
        public PlayerStats PlayerStats;
        public BotBehavior BotBehavior;
        public BotShooting BotShooting;
        public AssignCharacterType CharacterTypeAssigner;
        public Ragdoll RagdollController;   
        public BotHealth BotHealth;
        public Rigidbody CapsuleRb;
        public Animator CharacterAnimator;

        public Transform EnemySpawn;
        public Transform Spawn;
            
        public GameObject CapusleCollider;

        /**************** Scriptable Objects ****************/

        [Header("ScriptableObjects")]
        [SerializeField] private BotData botDataAsset;
        public BotData BotData
        {
            get => botDataAsset;
            set => botDataAsset = value;
        }

        [SerializeField] private SetAnimation animationAsset;
        public SetAnimation BotAnimations
        {
            get => animationAsset;
            set => animationAsset = value;
        }

        [SerializeField] private FOVScriptableObject fieldOfViewAsset;
        public FOVScriptableObject FieldOfViewSettings
        {
            get => fieldOfViewAsset;
            set => fieldOfViewAsset = value;
        }

        /**************** Start, Update, Etc. ****************/

        #region Awake
        void Awake()
        {
            GatherComponents();
        }
        #endregion

        #region Gather Components
        private void GatherComponents()
        {
            CapusleCollider = transform.GetChild(0).gameObject;
            PlayerStats = GetComponent<PlayerStats>();
            BotBehavior = GetComponent<BotBehavior>();
            BotShooting = GetComponent<BotShooting>();
            CharacterTypeAssigner = GetComponent<AssignCharacterType>();
            
            //CapsuleRb = CapusleCollider.transform.GetComponent<Rigidbody>();
            RagdollController = CapusleCollider.transform.GetComponent<Ragdoll>();
            BotHealth = CapusleCollider.transform.GetComponent<BotHealth>();

            CharacterTypeAssigner.ChooseRandomCharacter();
            CharacterAnimator = CapusleCollider.transform.GetChild(0).GetComponent<Animator>();
            BotHealth.GatherHealtComponents();

            DisplayError(BotData, "Bot data SO does not exist!", this);
            DisplayError(BotAnimations, "Bot animtions SO does not exist!", this);
            DisplayError(FieldOfViewSettings, "Bot fov SO does not exist!", this);

        }
        #endregion

        #region Display Error
        public void DisplayError<T>(T item, string message, Object context = null)
        {
            if(item == null)
            {
                Debug.LogError(message, context);
            }
        }
        #endregion

        /**************** Start, Update, Etc. ****************/

        public void Attack(int action, bool type)
        {
            BotAnimations.SetAction(CharacterAnimator, action, type);
        }

    }
}
