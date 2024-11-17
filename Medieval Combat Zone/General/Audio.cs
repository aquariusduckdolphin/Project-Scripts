using System.Collections;
using UnityEngine;

namespace CombatZone.Audio
{

    public abstract class Audio : MonoBehaviour
    {
        //An array to store the audio clips
        public AudioClip[] clip;

        //Stores the audio source from the game object
        private AudioSource source;

        //A variable to change the clips sound (range is zero or one)
        [Range(0f, 1f)]
        public float volume = 0.5f;

        void Start() { source = GetComponent<AudioSource>(); }

        /// <summary>
        /// Custom function that will take in the clip and volume to play the audio
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="loudness"> How loud the clip will play at. </param>
        public void PlayOnce(AudioClip effect, float loudness)
        {

            //On the audio source play a clip at a certain volume. Cannot be looped
            source.PlayOneShot(effect, loudness);

        }

        /// <summary>
        /// Custom function that will or will not loop
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="canLoop"> Will either loop or not loop the audio clip. </param>
        public void LoopableSound(AudioClip effect, bool canLoop)
        {

            GetComponent<AudioSource>().clip = effect;

            GetComponent<AudioSource>().loop = canLoop;

            GetComponent<AudioSource>().Play();

        }

        /// <summary>
        /// Custom function that will play the clip at a certain location
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="Xpos"> The x position to spawn. </param>
        /// <param name="Ypos"> The y position to spawn. </param>
        /// <param name="Zpos"> The z position to spawn. </param>
        public void AreaOfSound(AudioClip effect, int Xpos, int Ypos, int Zpos)
        {

            AudioSource.PlayClipAtPoint(effect, new Vector3(Xpos, Ypos, Zpos));

        }

        /// <summary>
        /// Custom function that will delay the audio
        /// </summary>
        /// <param name="effect"> The audio clip that is going to play. </param>
        /// <param name="loudness"> How loud the clip will play at. </param>
        /// <param name="timeDelay"> The time before playing to audio clip. </param>
        /// <returns></returns>
        public IEnumerator DelaySound(AudioClip effect, float loudness, float timeDelay)
        {

            yield return new WaitForSeconds(timeDelay);

            source.PlayOneShot(effect, loudness);

        }

    }

}