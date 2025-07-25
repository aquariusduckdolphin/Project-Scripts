using System.Collections;
using UnityEngine;

namespace CombatZone.VisualEffects
{
    public class PlayLavaEffect : MonoBehaviour
    {

        public Renderer lavaBall;
        public Renderer lavaStream;
        public GameObject lavaBounceGO;
        private Renderer lavaBounce;
        public GameObject lavaRippleGO;
        private Renderer lavaRipple;

        public float duration = 2f;
        public float elapsedTime = 0f;
        private float elapsedTime2 = 0f;
        private float elapsedTime3 = 0f;

        public float delay = 5f;
        public bool hasBeenReset = false;

        public VisualEffectProfile properties;

        /**************** Start, Update, Etc. ****************/

        #region Start
        void Start()
        {
            lavaBounce = lavaBounceGO.GetComponent<Renderer>();
            lavaRipple = lavaRippleGO.GetComponent<Renderer>();

            properties.InitializeEffect(lavaBall, properties.core);
            properties.InitializeEffect(lavaStream, properties.stream);

            ToggleEffects(false);
        }
        #endregion

        #region Update
        void Update()
        {
            StartCoroutine(AnimateLavaMaterial());
        }
        #endregion

        /**************** Effect Methods ****************/

        #region Set Gameobjects Off
        private void ToggleEffects(bool isActive)
        {
            lavaBounceGO.SetActive(isActive);
            lavaRippleGO.SetActive(isActive);
        }
        #endregion

        #region Set Material
        private IEnumerator AnimateLavaMaterial()
        {
            properties.ApplyLavaFlowEffect(lavaBall, properties.core, false, ref elapsedTime);
            yield return new WaitForSeconds(delay);
            properties.ApplyLavaFlowEffect(lavaStream, properties.stream, true, ref elapsedTime2, 0f);
            yield return new WaitForSeconds(delay);
            ToggleEffects(true);
            properties.RippleEffect(ref lavaRipple, ref elapsedTime3);
        }
        #endregion
    }
}