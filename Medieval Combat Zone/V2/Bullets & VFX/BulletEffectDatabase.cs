using CombatZone.Utilities;
using UnityEngine;

namespace CombatZone.Bullet
{

    #region Bullet Class
    [System.Serializable]
    public class BulletTypes
    {
        public string elementName;
        public GameObject bullet;
        public AudioClip[] clips;
    }

    [System.Serializable]
    public class ImpactEffectGroup
    {
        public string elementName;
        public ImpactEffect[] impactEffects;
    }

    [System.Serializable]
    public class ImpactEffect
    {
        public string impactName;
        public GameObject impactGameObject;
        public AudioClip clip;
    }
    #endregion

    /*************************************************/

    [CreateAssetMenu(fileName = "Effects Datas", menuName = "BulletTypes/Effects Datas")]
    public class BulletEffectDatabase : ScriptableObject
    {
        [Header("Bullet Properties")]
        public float bulletDamage = 10f;
        public float lifetime = 5f;
        public float groundCheckDistance = 5f;

        public BulletTypes[] bullet;
        public ImpactEffectGroup[] ImpactEffectGroup;
        public ImpactEffect[] groundImpactEffects;

        /**************** Audio Methods ****************/

        #region Play bullet audio
        public void InitializeBulletAudio(MonoBehaviour be, AudioSource audioSource, float volume, int currentBullet)
        {
            if (audioSource == null || volume == 0f) { return; }

            be.StartCoroutine(AudioUtility.DelaySoundClipLength(
                audioSource,
                bullet[currentBullet].clips[0],
                volume));

            PlayLoopingBulletAudio(audioSource, volume, currentBullet, 1);
        }

        public void PlayLoopingBulletAudio(AudioSource audioSource, float volume, int currentBullet, int clipIndex)
        {
            AudioUtility.LoopableSound(audioSource,
                bullet[currentBullet].clips[clipIndex],
                volume,
                true);
        }
        #endregion

        #region Play Impact Effect Audio
        public void PlayImpactSoundLoopable(AudioSource audioSource, int elementIndex, int impactEffectIndex, float volume)
        {
            if (CheckForNullAudio(ImpactEffectGroup, elementIndex, impactEffectIndex)) { return; }
            AudioUtility.LoopableSound(audioSource,
                ImpactEffectGroup[elementIndex].impactEffects[impactEffectIndex].clip,
                volume,
                true);
        }

        public void PlayImapactSoundSingle(AudioSource audioSource, int elementIndex, int impactEffectIndex, float volume)
        {
            if (CheckForNullAudio(ImpactEffectGroup, elementIndex, impactEffectIndex)) { return; }
            AudioUtility.PlayOnce(audioSource,
                ImpactEffectGroup[elementIndex].impactEffects[impactEffectIndex].clip,
                volume);
        }
        #endregion

        #region Play Ground Effect Audio
        public void PlayGroundImpactSound(AudioSource audioSource, int groundEffectIndex, float volume)
        {
            AudioUtility.PlayOnce(audioSource,
                groundImpactEffects[groundEffectIndex].clip,
                volume);
        }
        #endregion

        #region Check For Null Audio
        private bool CheckForNullAudio(ImpactEffectGroup[] elementSounds, int i, int j)
        {
            if (elementSounds[i].impactEffects[j].clip == null) { return true; }
            return false;
        }
        #endregion

        /**************** Destroy GameObjects ****************/

        #region Destroy GameObject
        public void DestroyAfterSound(GameObject gameObject, AudioSource audioSource, int groundEffectIndex)
        {
            if (audioSource.clip.length == groundImpactEffects[groundEffectIndex].clip.length)
            {
                Destroy(gameObject);
            }
        }
        #endregion

        #region Destroy After Sound Impact
        public void DestroyAfterSoundImpact(GameObject gameObject, AudioSource audioSource, int elementIndex, int impactEffectIndex)
        {
            if (audioSource.clip.length == ImpactEffectGroup[elementIndex].impactEffects[impactEffectIndex].clip.length)
            {
                Destroy(gameObject);
            }
        }
        #endregion
    }

}