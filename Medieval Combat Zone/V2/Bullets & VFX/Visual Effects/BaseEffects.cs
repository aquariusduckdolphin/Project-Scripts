using CombatZone.Character.Player;
using UnityEngine;
using CombatZone.Interfaces;
using System.Collections;
using CombatZone.Utilities;
using CombatZone.Bullet;

namespace CombatZone.VisualEffects
{
    public abstract class BaseEffects : Duration
    {

        #region Variables
        [Header("Scale")]
        [SerializeField] protected Vector3 initalScale = new Vector3(0.1f, 0.1f, 0.1f);
        [SerializeField] protected Vector3 targetScale = new Vector3(1f, 1f, 1f);

        [Header("Duration")]
        [SerializeField] protected float growthDuration = 10f;
        [SerializeField] protected float remainingTime = 10f;
        protected float currentTime;

        protected bool isTakingDamage = false;

        [Header("Audio")]
        protected AudioSource audioSource;
        [SerializeField, Range(0f, 1.0f)] protected float volumeOfEffect = 0.5f;
        #endregion

        [SerializeField] protected BulletEffectData bulletData;
        [SerializeField, Range(0, 3)] protected int elementType;
        [SerializeField, Range(0, 3)] protected int impactEffectIndex;

        [SerializeField] protected LayerMask collisionMask;

        /**************** Start, Update, Etc. ****************/

        #region Start
        /*private void Start()
        {
            PlayAudioEffect();
        }*/
        #endregion

        /**************** Effects ****************/

        #region ModifyPlayerSpeed
        protected void ModifyPlayerSpeed(Collider other, float walkSpeed, float sprintSpeed, float resetTime = 1f, bool shouldResest = true)
        {
            PlayerMovement players = other.transform.parent.GetComponent<PlayerMovement>();

            if (players != null)
            {
                players.walkSpeed = walkSpeed;
                players.sprintSpeed = sprintSpeed;
                if(shouldResest)
                { players.StartCoroutine(players.ResetWalkAndSprint(resetTime)); }
            }
        }
        #endregion

        #region Knockback Effect
        protected void Knockback(Vector3 position, float radius, LayerMask collisionMask, float knockbackValue)
        {
            RaycastHit[] hit = Physics.SphereCastAll(position, radius, Vector3.up, radius, collisionMask);

            foreach (RaycastHit player in hit)
            {
                Rigidbody rb = player.transform.GetComponent<Rigidbody>();
                Vector3 knockbackDirection = (player.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDirection * knockbackValue, ForceMode.Impulse);
            }
        }
        #endregion

        /**************** Utility ****************/

        #region Destruction Growth Effect
        protected void DestructionGrowthEffect()
        {
            ScaleOvertime(ref currentTime, initalScale, targetScale, growthDuration, transform);
            DestructionTimer(ref remainingTime, growthDuration, transform.root.gameObject);
        }
        #endregion

        #region Growth Effect
        protected void GrowthEffect(Transform transform)
        {
            ScaleOvertime(ref currentTime, initalScale, targetScale, growthDuration, transform);
        }
        #endregion

        #region Is In LayerMask
        protected bool IsInLayerMask(GameObject go, LayerMask layerMask)
        {
            return ((1 << go.layer) & collisionMask) != 0;
        }
        #endregion

        /**************** Audio ****************/

        #region Play Audio Effect
        protected void PlayAudioEffect()
        {
            audioSource = GetComponent<AudioSource>();
            if (GenericCheck(audioSource) || GenericCheck(bulletData) || volumeOfEffect == 0) { return; }
            bulletData.PlayImpactSoundLoopable(audioSource, elementType, impactEffectIndex, volumeOfEffect);
        }

        protected void PlayAudioOnce()
        {
            audioSource = GetComponent<AudioSource>();
            if (GenericCheck(audioSource) || GenericCheck(bulletData) || volumeOfEffect == 0) { return; }
            bulletData.PlayImapactSoundSingle(audioSource, elementType, impactEffectIndex, volumeOfEffect);
        }

        private bool GenericCheck<T>(T obj)
        {
            return obj == null;
        }
        #endregion

    }
}
