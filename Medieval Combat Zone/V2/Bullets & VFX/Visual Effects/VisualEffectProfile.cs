using UnityEngine;

namespace CombatZone.VisualEffects
{

    [CreateAssetMenu(fileName = "VisualEffectProfile", menuName = "VFX/VisualEffectProfile")]
    public class VisualEffectProfile : ScriptableObject
    {
        #region Shader Properties
        protected readonly string uvAxisProperty = "_U_OR_V";
        protected readonly string fadeDirectionProperty = "_FadeDirection";
        protected readonly string linearInvertProperty = "_LINEARINVERT";
        protected readonly string animationControlProperty = "_AnimationControl";

        protected const string isAnimatingProperty = "_ISANIMATING";

        protected readonly string effectRadiusProperty = "_EffectRadius";
        protected readonly string rippleFrequencyProperty = "_Frequency";
        protected readonly string rippleAmplitudeProperty = "_Amplitude";
        protected readonly string rippleDensityProperty = "_RippleDensity";
        protected readonly string edgeBlendProperty = "_EdgeBlend";
        protected readonly string gradientEdgeProperty = "_GradientEdge";
        #endregion

        public SurfaceEffectSettings core;
        public SurfaceEffectSettings stream;
        public SurfaceEffectSettings displacementEffect;
        public SurfaceEffectSettings ripple;

        [Space(10f)]

        [Tooltip("This is reset back to the default values that are set when entering/exiting play mode.")]
        [SerializeField] protected bool firstIniailzation = false;

        [Tooltip("Time it takes for the ripple effect to transition to its default state.")]
        [SerializeField] protected float duration = 5f;

        /**************** Start, Update, Etc. ****************/

        #region On Enable
        private void OnEnable()
        {
            if (firstIniailzation) { return; }

            core = new SurfaceEffectSettings
            {
                uv = 1f,
                isAnimated = 1f,
                linearInvert = 0f,
            };

            stream = new SurfaceEffectSettings
            {
                uv = 0f,
                isAnimated = 1f,
                linearInvert = 0f,
            };

            displacementEffect = new SurfaceEffectSettings
            {
                uv = 0f,
                isAnimated = 0f,
                linearInvert = 1f,
                animation = Vector2.zero,

                effectRadius = 100f,
                frequency = 1f,
                amplitude = 0.05f,
                rippleDensity = 10f,
                edgeBlend = 0.88f,
                gradientEdge = 10f
            };

            ripple = ripplePreset;
            ripple.linearInvert = 1f;

            firstIniailzation = true;
        }
        #endregion

        /**************** Srtucts ****************/

        #region Surface Effect Settings
        [System.Serializable]
        public struct SurfaceEffectSettings
        {
            [Tooltip("By default its use u. 1 means that its using v.")]
            [Range(0f, 1f)] public float uv;
            [Tooltip("By default its on. 1 means that its off.")]
            [Range(0f, 1f)] public float linearInvert;
            [Tooltip("By default its false. 1 means true." +
                "\n ONLY FOR THE MUD SHADER")]
            [Range(0f, 1f)] public float isAnimated;
            [Tooltip("Controls the fade direction for two effects:" +
            "\n- X-axis is the minimum value." +
            "\n- Y-axis affects the maximum value.")]
            public Vector2 fade;
            [Tooltip("Controls the animation movement direction:" +
            "\n- X-axis affects the stream (ripple) animation." +
            "\n- Y-axis affects the ball (core) animation.")]
            public Vector2 animation;

            [Header("Ripple Properties")]
            public float effectRadiusInitial;
            public float effectRadius;
            public float frequencyInitialial;
            public float frequency;
            public float amplitude;
            public float rippleDensity;
            public float edgeBlend;
            public float gradientEdge;
        }
        #endregion

        #region Ripple Preset
        protected SurfaceEffectSettings ripplePreset = new SurfaceEffectSettings
        {
            animation = Vector2.zero,
            effectRadiusInitial = 6f,
            effectRadius = 1f,
            frequencyInitialial = 4f,
            frequency = 1f,
            amplitude = 0.05f,
            rippleDensity = 4f,
            edgeBlend = 0f,
            gradientEdge = 10f
        };
        #endregion

        /**************** Methods ****************/

        #region Initialize Ripple Effects
        public void InitializeEffect(Renderer renderer, SurfaceEffectSettings surfaceSettings)
        {
            FloatPropertyCheck(renderer, uvAxisProperty, surfaceSettings.uv);
            FloatPropertyCheck(renderer, linearInvertProperty, surfaceSettings.linearInvert);
            FloatPropertyCheck(renderer, isAnimatingProperty, surfaceSettings.isAnimated);
            VectorPropertyCheck(renderer, fadeDirectionProperty, surfaceSettings.fade);
            VectorPropertyCheck(renderer, animationControlProperty, surfaceSettings.animation);

            FloatPropertyCheck(renderer, effectRadiusProperty, surfaceSettings.effectRadius);
            FloatPropertyCheck(renderer, rippleFrequencyProperty, surfaceSettings.frequency);
            FloatPropertyCheck(renderer, rippleAmplitudeProperty, surfaceSettings.amplitude);
            FloatPropertyCheck(renderer, rippleDensityProperty, surfaceSettings.rippleDensity);
            FloatPropertyCheck(renderer, edgeBlendProperty, surfaceSettings.edgeBlend);
            FloatPropertyCheck(renderer, gradientEdgeProperty, surfaceSettings.gradientEdge);
        }
        #endregion

        #region Float Property Check
        private void FloatPropertyCheck(Renderer renderer, string property, float enabled)
        {
            if (!renderer.material.HasProperty(property)) { return; }
            renderer.material.SetFloat(property, enabled);
        }
        #endregion

        #region Vector Property Check
        private void VectorPropertyCheck(Renderer renderer, string property, Vector2 direction)
        {
            if (!renderer.material.HasProperty(property)) { return; }
            renderer.material.SetVector(property, direction);
        }
        #endregion

        #region Ripple Effect
        public void RippleEffect(ref Renderer renderer, ref float elapsedTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float effectiveRadius = Mathf.Lerp(ripplePreset.effectRadiusInitial, ripplePreset.effectRadius, t);
            float frequencys = Mathf.Lerp(ripplePreset.frequencyInitialial, ripplePreset.frequency, t);
            renderer.material.SetFloat(effectRadiusProperty, effectiveRadius);
            renderer.material.SetFloat(rippleFrequencyProperty, frequencys);
        }
        #endregion

        #region Liquid Flow Effect
        public void ApplyLavaFlowEffect(Renderer targetRenderer, SurfaceEffectSettings surfaceSettings, bool affectXDirection, ref float elapsedTime, float uvThreshold = 1f)
        {
            Vector2 newDirection;
            float direction;

            if (Mathf.Approximately(surfaceSettings.uv, uvThreshold) && elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);

                direction = Mathf.Lerp(surfaceSettings.fade.x, surfaceSettings.fade.y, t);

                if(direction == surfaceSettings.fade.y) { return; }

                if (!affectXDirection)
                { newDirection = new Vector2(0f, direction); }
                else
                { newDirection = new Vector2(direction, 0f); }

                targetRenderer.material.SetVector(fadeDirectionProperty, newDirection);
                return;
            }
        }
        #endregion

    }
}
