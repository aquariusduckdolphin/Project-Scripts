using UnityEngine;

namespace CombatZone.VisualEffects
{

    public class KnockbackStatusEffect : BaseEffects
    {

        [Header("Knockback Power")]
        [SerializeField] private float initialKnockbackPower = 100f;
        [SerializeField] private float minKnockbackPower = 10f;
        private float currentKnockback;

        [Header("Sphere Cast")]
        [SerializeField] private float radius = 10f;

        #region Update
        void Update()
        {
            float growthFactor = Mathf.Clamp01(currentTime / growthDuration);
            currentKnockback = Mathf.Lerp(initialKnockbackPower, minKnockbackPower, growthFactor);

            float newRadius = initalScale.x / 2f;
            float endRadius = targetScale.x / 2;
            radius = Mathf.Lerp(newRadius, endRadius, growthFactor);

            Knockback(transform.position, radius, collisionMask, currentKnockback);
            ScaleOvertime(ref currentTime, initalScale, targetScale, growthDuration, transform);
            DestructionTimer(ref remainingTime, growthDuration, gameObject);
        }
        #endregion

        #region Draw Gizmos Selected
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        #endregion
    }

}