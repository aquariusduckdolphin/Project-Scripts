using System.Collections.Generic;
using UnityEngine;

namespace CombatZone.Manager
{

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {

        #region Singleton
        public static AudioManager _instance { get; set; }

        private void Awake()
        {
            if (_instance != null && _instance != this) { Destroy(this); }
            else { _instance = this; }
        }
        #endregion

        /**************** Variables ****************/

        #region Audio Properties
        [Header("Audio Properties")]
        private AudioSource audioSource;
        private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
        public int audioQueueLength = 0;
        #endregion

        /**************** Start, Update, Etc. ****************/

        #region Start
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        #endregion

        #region Update
        void Update()
        {
            PlayAudioClip();
        }
        #endregion

        /**************** Audio Methods ****************/

        #region Play Audio Clip
        private void PlayAudioClip()
        {
            if (audioQueue.Count == 0 || audioSource.isPlaying) { return; }
            audioSource.clip = audioQueue.Peek();
            audioSource.Play();
            audioQueue.Dequeue();
            audioQueueLength--;
        }
        #endregion

        #region Add To Audio Queue
        public void AddToAudiQueue(AudioClip clip)
        {
            audioQueue.Enqueue(clip);
            audioQueueLength++;
        }
        #endregion

    }

}