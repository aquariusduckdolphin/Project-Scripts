using System.Collections;
using UnityEngine;

namespace CombatZone.Utilities
{

    public static class AudioUtility
    {

        private const float loudness = 1f;

        /// <summary>
        /// Play the audio once
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="loudness"> How loud the clip will play at. </param>
        public static void PlayOnce(AudioSource audioSource, AudioClip effect, float loudness = loudness)
        {
            if (audioSource.isPlaying) { audioSource.Stop(); }
            audioSource.clip = effect;
            audioSource.loop = false;
            audioSource.PlayOneShot(effect, loudness);
        }

        /// <summary>
        /// Play the audio once and turn off looping or allow it to play forever.
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="canLoop"> Will either loop or not loop the audio clip. </param>
        public static void LoopableSound(AudioSource audioSource, AudioClip effect, float loudness = loudness, bool canLoop = false)
        {
            if(audioSource.clip == null) { return; }
            audioSource.clip = effect;
            audioSource.volume = loudness;
            audioSource.loop = canLoop;
            audioSource.Play();
        }

        /// <summary>
        /// Play the audio at a specific coordinate
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="Xpos"> The x position to spawn. </param>
        /// <param name="Ypos"> The y position to spawn. </param>
        /// <param name="Zpos"> The z position to spawn. </param>
        public static void AreaOfSound(AudioClip effect, int Xpos, int Ypos, int Zpos)
        {
            AudioSource.PlayClipAtPoint(effect, new Vector3(Xpos, Ypos, Zpos));
        }

        /// <summary>
        /// Have a delay of a specific time, then play audio for the full length. 
        /// This will happen once only.  
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="loudness"> How loud the clip will play at. </param>
        /// <param name="timeDelay"> The time before playing to audio clip. </param>
        /// <returns></returns>
        public static IEnumerator DelaySound(AudioSource audioSource, AudioClip effect, float timeDelay, float loudness = loudness)
        {
            yield return new WaitForSeconds(timeDelay);
            if (audioSource.isPlaying) { audioSource.Stop(); }
            audioSource.PlayOneShot(effect, loudness);
            yield return new WaitForSeconds(audioSource.clip.length);
        }

        /// <summary>
        /// Play the effect for the full duration. ONLY HAPPENS ONCE.
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="effect"></param>
        /// <param name="loudness"></param>
        /// <returns></returns>
        public static IEnumerator DelaySoundClipLength(AudioSource audioSource, AudioClip effect, float loudness = loudness)
        {
            if (audioSource.isPlaying) { audioSource.Stop(); }
            audioSource.PlayOneShot(effect, loudness);
            if(audioSource.clip == null) { yield break; }
            yield return new WaitForSeconds(audioSource.clip.length);
        }

    }

}