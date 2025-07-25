using CombatZone.Interfaces;
using System.Collections;
using UnityEngine;

namespace CombatZone.VisualEffects
{

    public class LavaEffect : BaseEffects
    {

        [Header("Lava Properties")]
        private int tickTime = 0;
        [SerializeField] private float tickDuration = 10f;
        private Coroutine lavaCoroutine;
        public IHealth health;

        [SerializeField] private GameObject lavaBall;
        [SerializeField] private GameObject lavaStream;
        [SerializeField] private GameObject lavaRipple;

        [SerializeField] private float elapsedTime;

        /**************** Start, Update, Etc. ****************/

        #region Start
        private void Start()
        {
            PlayAudioEffect();
        }
        #endregion

        #region Update
        void Update()
        {
            bool maxScale;
            maxScale = HasFinishedScalingOvertime(ref currentTime, initalScale, targetScale, growthDuration, transform);
            if (maxScale)
            {
                //lavaBall.SetActive(false);
                //lavaStream.SetActive(false);
                //lavaRipple.SetActive(false);
            }

            //GrowthEffect(lavaRipple.transform);

            if(currentTime >= growthDuration)
            {
                BurnDamage(false, 1f);
                Destroy(gameObject.transform.root.gameObject);
            }

        }
        #endregion

        #region On Trigger Enter
        private void OnTriggerEnter(Collider other)
        {
            if (IsInLayerMask(other.gameObject, collisionMask))
            {
                health = other.GetComponent<IHealth>();
                if (health == null)
                {
                    Debug.Log("Not working");
                    return;
                }

                BurnDamage(true, 0f);
            }
        }
        #endregion

        #region On Trigger Exit
        private void OnTriggerExit(Collider other)
        {
            if (IsInLayerMask(other.gameObject, collisionMask))
            {
                BurnDamage(false, 1f);
            }
        }
        #endregion

        /**************** Lava Effect Methods ****************/

        #region Damage Overtime After Leaving
        private void BurnDamage(bool infinit, float loopAmount)
        {
            if (lavaCoroutine != null)
            {
                StopCoroutine(lavaCoroutine);
                lavaCoroutine = null;
            }
            
            if(health == null) { return; }
            lavaCoroutine = StartCoroutine(health.TickDamage(10f, 1f, infinit, loopAmount));    
        }
        #endregion

    }

}