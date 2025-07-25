using CombatZone.Utilities;
using UnityEngine;

namespace CombatZone.VisualEffects
{
    public class ExplosiveCombustion : BaseEffects
    {

        [Header("Flash Properties")]
        [SerializeField] private ParticleSystem flash;
        [SerializeField] private float flashInitialScale = 10.14f;
        [SerializeField] private float flashMaxScale = 50f;

        [Header("Smoke Properties")]
        [SerializeField] private ParticleSystem smoke;
        [SerializeField] private float smokeInitialScale = 3.38f;
        [SerializeField] private float smokeMaxScale = 50f;

        [Header("Spark Properties")]
        [SerializeField] private ParticleSystem spark;
        [SerializeField] private float sparkInitialScale = 0.8449999f;
        [SerializeField] private float sparkMaxScale = 2f;

        [Header("Explosion Properties")]
        [SerializeField] private float sourceMinDistance = 10f; 
        [SerializeField] private float sourceMaxDistance = 30f; 
        [SerializeField] private GameObject explosion;

        /**************** Start, Update, Etc. ****************/

        #region Start
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            AudioUtility.LoopableSound(audioSource, bulletData.bulletSounds[0].clips[1], volumeOfEffect);
        }
        #endregion

        #region Update
        void Update()
        {
            var main = flash.main;
            main.startSize = ScaleOvertimeLerp(ref currentTime, flashInitialScale, flashMaxScale, growthDuration);

            var smokeMain = smoke.main;
            smokeMain.startSize = ScaleOvertimeLerp(ref currentTime, smokeInitialScale, smokeMaxScale, growthDuration);

            var sparkMain = spark.main;
            sparkMain.startSize = ScaleOvertimeLerp(ref currentTime, sparkInitialScale, sparkMaxScale, growthDuration);

            audioSource.maxDistance = ScaleOvertimeLerp(ref currentTime, sourceMinDistance, sourceMaxDistance, growthDuration);

            if(currentTime >= growthDuration && explosion != null)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        #endregion

    }
}