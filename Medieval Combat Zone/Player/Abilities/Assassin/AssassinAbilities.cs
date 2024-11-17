using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Abilities
{

    public class AssassinAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject[] handToHand;

        public float handToHandDuration;

        private int handWeapon = 0;

        [Header("Ability 1")]
        public GameObject disguise;

        public MeshRenderer meshRender;

        public Material baseMaterial;

        public float disappearAmount;

        [Header("Ability 2")]
        public Animator anim;

        public bool canLockPick = false;

        public BoxCollider colliders;

        void Start()
        {

            CreateNewControls();

        }

        void Update()
        {

            SettingAbilities();

        }

        public override void OnUltimate(InputAction.CallbackContext context)
        {

            if (ultimateCoolDownTimer > 0) { return; }
            else { ultimateCoolDownTimer = ultimateCoolDown; }

            ultimate.fillAmount = 0;

            StartCoroutine(ChangeHand());

        }

        private IEnumerator ChangeHand()
        {

            handWeapon = Random.Range(0, handToHand.Length - 1);

            handToHand[handToHand.Length - 1].SetActive(false);

            handToHand[handWeapon].SetActive(true);

            yield return new WaitForSeconds(10f);

            handToHand[handWeapon].SetActive(false);

            handToHand[handToHand.Length - 1].SetActive(true);

            /*Vector3 newPosition = shootLocation.transform.position;

            newPosition.z += inFront;

            GameObject bullets = Instantiate(bullet[currentBulletType], shootLocation.transform.position, Quaternion.identity);

            Rigidbody rb = bullets.GetComponent<Rigidbody>();

            rb.AddForce(shootLocation.transform.forward * 1000f);*/

        }

        public override void OnAbilityOne(InputAction.CallbackContext context)
        {

            if (abilityCoolDownTimer > 0) { return; }
            else { abilityCoolDownTimer = abilityCoolDown; }

            ability.fillAmount = 0;

            StartCoroutine(NPC());

        }

        IEnumerator NPC()
        {

            meshRender.material.color = Color.black;

            yield return new WaitForSeconds(10f);

            meshRender.material.color = baseMaterial.color;

        }

        public override void OnAbilityTwo(InputAction.CallbackContext context)
        {

            if (canLockPick)
            {

                if (ability2CoolDownTimer > 0) { return; }
                else { ability2CoolDownTimer = abiliity2CoolDown; }

                ability2.fillAmount = 0;

                print("Opened the area for players");

                Destroy(colliders.gameObject, 10f);

            }

        }

    }

}