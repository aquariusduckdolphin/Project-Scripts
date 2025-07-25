using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Character.Abilities
{

    public class AssassinAbilities : BaseAbilities
    {

        [Header("Ultimate")]
        public GameObject[] handToHand;
        public float handToHandDuration;
        private int handWeapon = 0;

        [Header("TriggerAbility 1")]
        public GameObject disguise;
        public MeshRenderer meshRender;
        public Material baseMaterial;
        public float disappearAmount;

        [Header("TriggerAbility 2")]
        public Animator anim;
        public bool canLockPick = false;
        public BoxCollider colliders;

        /************************** Input Actions **************************/

        protected override void OnUltimate(InputAction.CallbackContext context)
        {
            if (ultimateCooldownTimer > 0f) { return; }
            Ultimate(context);
            StartCoroutine(ChangeHand());
        }

        protected override void OnPrimaryAbility(InputAction.CallbackContext context)
        {
            if (primaryCooldownTimer > 0f) { return; }
            PrimaryAbility(context);
            StartCoroutine(NPC());
        }

        protected override void OnSecondaryAbility(InputAction.CallbackContext context)
        {
            if (secondaryCooldownTimer > 0 && canLockPick) { return; }
            SecondaryAbility(context);
            StartCoroutine(NPC());
            Debug.Log("Opened the area for players");
            Destroy(colliders.gameObject, 10f);
        }

        /************************** Operations **************************/

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

        IEnumerator NPC()
        {
            meshRender.material.color = Color.black;
            yield return new WaitForSeconds(10f);
            meshRender.material.color = baseMaterial.color;
        }

    }

}