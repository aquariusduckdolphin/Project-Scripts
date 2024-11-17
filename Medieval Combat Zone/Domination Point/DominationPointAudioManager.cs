using System.Collections.Generic;
using UnityEngine;

namespace CombatZone.Objective
{

    public class DominationPointAudioManager : MonoBehaviour
    {

        #region Singleton
        public static DominationPointAudioManager _instance { get; set; }

        private void Awake()
        {

            if (_instance != null && _instance != this)
            {

                Destroy(this);

            }
            else
            {

                _instance = this;

            }

        }
        #endregion

        /***************************************************************/

        #region Variables

        [Header("Audio Properties")]
        private AudioSource dominationAudioSource;

        public Queue<AudioClip> audioQueue = new Queue<AudioClip>();

        #endregion

        /***************************************************************/

        #region Start & Update

        #region Start
        private void Start()
        {

            dominationAudioSource = GetComponent<AudioSource>();

        }
        #endregion

        #region Update
        void Update()
        {

            PlayAudioClip();

        }
        #endregion

        #endregion

        #region Play Audio Clip
        private void PlayAudioClip()
        {

            if (audioQueue.Count == 0 || dominationAudioSource.isPlaying) { return; }

            dominationAudioSource.clip = audioQueue.Peek();

            dominationAudioSource.Play();

            audioQueue.Dequeue();

        }
        #endregion

        #region Add To Audio Queue
        public void AddToAudiQueue(AudioClip clip)
        {

            audioQueue.Enqueue(clip);

            Debug.Log("Playering Audio");

        }
        #endregion

    }

}


