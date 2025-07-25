using CombatZone.Utilities;
using UnityEngine;

namespace CombatZone.Bullet
{

    [System.Serializable]
    public class ElementalSoundSet
    {
        public string name;
        public AudioClip[] clips;
    }

    [System.Serializable]
    public class ImpactEffectSet
    {
        public string name;
        public GameObject[] impactEffect;
    }

    [CreateAssetMenu(fileName = "Effect Data", menuName = "BulletTypes/Effect Data")]
    public class BulletEffectData : ScriptableObject
    {
        [Header("Bullet Properties")]
        public float bulletDamage = 10f;
        public float lifetime = 5f; 
        public float groundCheckDistance = 5f;

        [Header("Bullet Configuration")]
        [Tooltip("0 = Fire \n 1 = Water \n 2 = Earth \n 3 = Air")]
        public GameObject[] elementalBulletPrefab = new GameObject[4];
        [Tooltip("0 = Fire \n 1 = Water \n 2 = Earth \n 3 = Air")]
        public ElementalSoundSet[] bulletSounds = new ElementalSoundSet[4];

        [Header("Impact Effect Variations")]
        [Tooltip("0 = Fire \n 1 = Water \n 2 = Earth \n 3 = Air")]
        public ImpactEffectSet[] impactEffects = new ImpactEffectSet[4];

        [Tooltip("0 = Fire \n 1 = Water \n 2 = Earth \n 3 = Air")]
        [SerializeField] private ElementalSoundSet[] impactSounds = new ElementalSoundSet[4];

        [Header("Ground Impact Effect")]
        [Tooltip("0 = Fire \n 1 = Water \n 2 = Earth \n 3 = Air")]
        public GameObject[] groundImpactEffect = new GameObject[4];
        [Tooltip("0 = Fire \n 1 = Water \n 2 = Earth \n 3 = Air")]
        [SerializeField] private AudioClip[] groundImpactSounds = new AudioClip[4];

        #region Play Impact Effect Audio
        public void PlayImpactSoundLoopable(AudioSource audioSource, int elementIndex, int impactEffectIndex, float volume)
        {
            if (CheckForNullAudio(impactSounds, elementIndex, impactEffectIndex)) { return; }
            AudioUtility.LoopableSound(audioSource, 
                impactSounds[elementIndex].clips[impactEffectIndex], 
                volume, 
                true);
        }

        public void PlayImapactSoundSingle(AudioSource audioSource, int elementIndex, int impactEffectIndex, float volume)
        {
            if(CheckForNullAudio(impactSounds, elementIndex, impactEffectIndex)) { return; }
            AudioUtility.PlayOnce(audioSource,
                impactSounds[elementIndex].clips[impactEffectIndex],
                volume);
        }
        #endregion

        #region Play Ground Effect Audio
        public void PlayGroundImpactSound(AudioSource audioSource, int groundEffectIndex, float volume)
        {
            AudioUtility.PlayOnce(audioSource, 
                groundImpactSounds[groundEffectIndex], 
                volume);
        }
        #endregion

        #region Destroy GameObject
        public void DestroyAfterSound(GameObject gameObject, AudioSource audioSource, int groundEffectIndex)
        {
            if(audioSource.clip.length == groundImpactSounds[groundEffectIndex].length)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Destroy After Sound Impact
        public void DestroyAfterSoundImpact(GameObject gameObject, AudioSource audioSource, int elementIndex, int impactEffectIndex)
        {
            if (audioSource.clip.length == impactSounds[elementIndex].clips[impactEffectIndex].length)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Check For Null Audio
        private bool CheckForNullAudio(ElementalSoundSet[] elementSounds, int i, int j)
        {
            if (elementSounds[i].clips[j] == null) { return true; }
            return false;
        }
        #endregion

    }

}