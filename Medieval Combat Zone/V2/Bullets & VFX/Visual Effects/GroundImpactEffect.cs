using System.Collections;
using UnityEngine;

namespace CombatZone.Bullet.GroundImpact
{
    [RequireComponent(typeof(AudioSource))]
    public class GroundImpactEffect : MonoBehaviour
    {

        [SerializeField] private BulletEffectData bulletData;

        [SerializeField] private float destroyDelay = 3f;

        private AudioSource audioSource;
        [Range(0, 3)] public int groundEffectIndex;
        [SerializeField, Range(0f, 1f)] private float volume;

        /**************** Start, Update, Etc. ****************/

        #region Start
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            bulletData.PlayGroundImpactSound(audioSource, groundEffectIndex, volume);

            Destroy(gameObject, destroyDelay);
        }
        #endregion

    }
}