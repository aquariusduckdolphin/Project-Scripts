using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Abilities
{

    public class WitchAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject[] herbalism;

        public int currentHerbalism;

        public float herbalismDuration;

        [Header("Ability 1")]
        public Vector3[] divination;

        public Quaternion[] divinationRotation;

        [SerializeField] private Transform witch;

        [SerializeField] private int currentIndex = 0;

        [SerializeField] private int framesToRecord = 1000;

        public bool rewinding = false;

        public float rewindSpeed = 10f;

        [Header("Ability 2")]
        public GameObject charmCasting;

        public float charmCastingDuration;


        void Start()
        {

            CreateNewControls();

            witch = this.gameObject.transform;

            divination = new Vector3[framesToRecord];

            divinationRotation = new Quaternion[framesToRecord];

        }

        void Update()
        {

            SettingAbilities();

        }

        void FixedUpdate()
        {

            if (!rewinding)
            {

                TrackLocations();

            }
            else
            {

                Rewind();

            }

        }

        public override void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCoolDownTimer > 0) { return; }
            else { ultimateCoolDownTimer = ultimateCoolDown; }

            ultimate.fillAmount = 0;

            Herbalism();

        }

        private void Herbalism()
        {

            int enchant = Random.Range(0, herbalism.Length);

            SpawningObj(herbalism[enchant], transform.position, Quaternion.identity, herbalismDuration);

        }

        public override void OnAbilityOne(InputAction.CallbackContext context)
        {

            if (abilityCoolDownTimer > 0) { return; }
            else { abilityCoolDownTimer = abilityCoolDown; }

            ability.fillAmount = 0;

            Rewind();

            rewinding = true;

        }

        public override void OnAbilityTwo(InputAction.CallbackContext context)
        {

            if (ability2CoolDownTimer > 0) { return; }
            else { ability2CoolDownTimer = abiliity2CoolDown; }

            ability2.fillAmount = 0;

            GameObject charm = Instantiate(charmCasting, transform.position, Quaternion.identity);

            Rigidbody charmRB = charm.GetComponent<Rigidbody>();

            charmRB.AddForce(transform.forward * 1000f, ForceMode.Impulse);

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