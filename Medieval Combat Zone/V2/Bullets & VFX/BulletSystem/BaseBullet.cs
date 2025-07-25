using UnityEngine;
using CombatZone.Interfaces;
using CombatZone.Utilities;

namespace CombatZone.Bullet
{

    public abstract class BaseBullet : MonoBehaviour
    {

        /**************** Variables ****************/

        [Header("Bullet Element Type")]
        protected IBullet.ElementType collidedBulletType;

        [Header("Bullet Properties")]
        public Transform playerTransform;
        [SerializeField] protected BulletEffectData bulletData;

        [Header("Audio Properties")]
        [SerializeField, Range(0f, 1f)] protected float volumeOfEffect = 1f;
        protected AudioSource audioSource;

        [Header("Impact Rotation Properties")]
        Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);

        [Header("Raycast Properties")]
        public LayerMask collisionMask;
        public float raycastDistance = 10f;

        [SerializeField] private float destroyDelay = 5f;

        #region Get Sphere Collider Size
        private float GetSphereColliderSize()
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();
            return sphereCollider.radius;
        }
        #endregion

        /**************** Collision Detection Methods ****************/

        #region Handle Impact
        protected void HandleImpact(Collider other, int index)
        {
            ApplyDamageToPlayer(other);
            HandleBulletCollision(other, index);
            SpawnGroundImpactEffect(index, GetSphereColliderSize());
            Destroy(gameObject, destroyDelay);
        }
        #endregion

        /*--------------- Player Collision ---------------*/

        #region Damage Player
        protected void ApplyDamageToPlayer(Collider targetObject)
        {
            if (targetObject.gameObject.transform == playerTransform) { return; }
            IHealth damageableObject = targetObject.GetComponent<IHealth>();
            if (damageableObject == null) { return; }
            damageableObject.TakeDamage(bulletData.bulletDamage);
            Destroy(gameObject);
        }
        #endregion

        /*--------------- Bullet Collision ---------------*/

        #region Bullet Collision 
        protected void HandleBulletCollision(Collider collidedObject, int startImpactEffectIndex)
        {
            IBullet bullet = collidedObject.GetComponent<IBullet>();
            if (bullet == null) { return; }
            collidedBulletType = bullet.elementType;

            switch (collidedBulletType)
            {
                case IBullet.ElementType.Fire:
                    ValidateEffect(startImpactEffectIndex, 0);
                    break;

                case IBullet.ElementType.Water:
                    ValidateEffect(startImpactEffectIndex, 1);
                    break;

                case IBullet.ElementType.Earth:
                    ValidateEffect(startImpactEffectIndex, 2);
                    break;
                
                case IBullet.ElementType.Air:
                    ValidateEffect(startImpactEffectIndex, 3);
                    break;
            }
        }
        #endregion

        #region Select Effect Variant By Surface
        private int SelectEffectVariantBySurface(float distance)
        {
            bool raycast = Physics.Raycast(transform.position, Vector3.down, distance, collisionMask);
            if (raycast)
            {
                return 4;
            }
            return 2;
        }
        #endregion

        #region Null HandleImpact Check
        protected void ValidateEffect(int effectGroupIndex, int variantIndex)
        {
            if (bulletData.impactEffects[effectGroupIndex].impactEffect[variantIndex] == null) { return; }
            Instantiate(bulletData.impactEffects[effectGroupIndex].impactEffect[variantIndex], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        #endregion

        /*--------------- Ground Collision ---------------*/

        #region Spawn Ground Impact HandleImpact
        protected void SpawnGroundImpactEffect(int effectType, float radius = 4f)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, collisionMask);
            if (hits.Length > 0)
            {
                if(bulletData.groundImpactEffect[effectType] != null)
                {
                    Instantiate(bulletData.groundImpactEffect[effectType], transform.position, rotation);
                }
                print("Ground");
                foreach(var hit in hits)
                {
                    print(hit.gameObject.name);
                }
                Destroy(gameObject);
            }
        }
        #endregion

        /**************** Audio ****************/

        #region Initailize Bullet Audio
        protected void InitializeBulletAudio(int currentBullet)
        {
            GetAudioSource();
            StartCoroutine(AudioUtility.DelaySoundClipLength(
                audioSource,
                bulletData.bulletSounds[currentBullet].clips[0], 
                volumeOfEffect));

            PlayLoopingBulletAudio(currentBullet, 1);
        }
        #endregion

        #region Play Looping Bullet Audio
        protected void PlayLoopingBulletAudio(int currentBullet, int clipIndex)
        {
            GetAudioSource();
            AudioUtility.LoopableSound(audioSource,
                bulletData.bulletSounds[currentBullet].clips[clipIndex],
                volumeOfEffect,
                true);
        }
        #endregion

        private void GetAudioSource()
        {
            if(audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

    }

}