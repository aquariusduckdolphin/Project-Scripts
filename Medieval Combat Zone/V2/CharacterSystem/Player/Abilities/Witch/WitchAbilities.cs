using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Abilities
{

    public class WitchAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject[] herbalism;
        public int currentHerbalism;
        public float herbalismDuration;

        [Header("TriggerAbility 1")]
        public Vector3[] divination;
        public Quaternion[] divinationRotation;
        [SerializeField] private Transform witch;
        [SerializeField] private int currentIndex = 0;
        [SerializeField] private int framesToRecord = 1000;
        public bool rewinding = false;
        public float rewindSpeed = 10f;

        [Header("TriggerAbility 2")]
        public GameObject charmCasting;
        public float charmCastingDuration;

        /************************** Start, Update, Etc. **************************/

        void Start()
        {
            witch = gameObject.transform;
            divination = new Vector3[framesToRecord];
            divinationRotation = new Quaternion[framesToRecord];
        }

        void FixedUpdate()
        {
            if (!rewinding) { TrackLocations(); }
            else { Rewind(); }
        }

        /************************** Input Actions **************************/

        protected override void OnUltimate(InputAction.CallbackContext context)
        {
            if (ultimateCooldownTimer > 0f) { return; }
            Ultimate(context);
            Herbalism();
        }

        protected override void OnPrimaryAbility(InputAction.CallbackContext context)
        {
            if (primaryCooldownTimer > 0f) { return; }
            PrimaryAbility(context);
            Rewind();
            rewinding = true;
        }

        protected override void OnSecondaryAbility(InputAction.CallbackContext context)
        {
            if (secondaryCooldownTimer > 0f) { return; }
            SecondaryAbility(context);
            GameObject charm = Instantiate(charmCasting, transform.position, Quaternion.identity);
            Rigidbody charmRB = charm.GetComponent<Rigidbody>();
            charmRB.AddForce(transform.forward * 1000f, ForceMode.Impulse);
        }

        /************************** Operations **************************/

        private void Herbalism()
        {
            int enchant = Random.Range(0, herbalism.Length);
            SpawnEffect(herbalism[enchant], transform.position, Quaternion.identity, herbalismDuration);
        }

        private void TrackLocations()
        {
            currentIndex = (currentIndex + 1) % divination.Length;
            divination[currentIndex] = witch.position;
            divinationRotation[currentIndex] = witch.rotation;
        }

        private void Rewind()
        {

            currentIndex--;

            if (currentIndex < 0)
            {
                rewinding = false;
                return;
            }

            // Interpolate between current position/rotation and previous recorded position/rotation
            Vector3 targetPosition = divination[currentIndex];
            Quaternion targetRotation = divinationRotation[currentIndex];
            witch.position = Vector3.Lerp(witch.position, targetPosition, Time.deltaTime * rewindSpeed);
            witch.rotation = Quaternion.Slerp(witch.rotation, targetRotation, Time.deltaTime * rewindSpeed);

            // Check if close enough to the target position/rotation, then stop rewinding
            float positionDifference = Vector3.Distance(witch.position, targetPosition);
            float rotationDifference = Quaternion.Angle(witch.rotation, targetRotation);
            if (positionDifference < 0.1f && rotationDifference < 1f) { rewinding = false; }

        }
        
    }

}