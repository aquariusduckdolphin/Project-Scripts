using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace CombatZone.Utilities
{

    public abstract class Duration : MonoBehaviour
    {

        #region Destory on Duration End
        protected virtual void DestructionTimer(ref float remainingDuration, float destructionThreshold, GameObject targetObject)
        {
            if (remainingDuration > 0) { remainingDuration -= Time.deltaTime; }
            else if (remainingDuration <= destructionThreshold) { Destroy(targetObject); }
        }
        #endregion

        #region Update Cooldown
        protected virtual void UpdateAbilityCooldown(ref float coolDownTimer, float cooldownDuration, Image abilityIcon)
        {
            if (coolDownTimer > 0)
            {
                if (coolDownTimer > 0) { coolDownTimer -= Time.deltaTime; }
                abilityIcon.fillAmount = coolDownTimer / cooldownDuration;
            }
            else { abilityIcon.fillAmount = 1f; }
        }
        #endregion

        #region Growth Duration
        protected virtual void ScaleOvertime(ref float time, Vector3 initalScale, Vector3 targetScale, float growthDuration, Transform target)
        {
            if (time < growthDuration) { time += Time.deltaTime; }
            float t = time / growthDuration;
            target.localScale = Vector3.Lerp(initalScale, targetScale, t);
        }

        protected virtual bool HasFinishedScalingOvertime(ref float time,  Vector3 initalScale, Vector3 targetScale, float growthDuration, Transform target)
        {
            ScaleOvertime(ref time, initalScale, targetScale, growthDuration, target);
            if (target.localScale == targetScale) { return true; }
            return false;
        }
        #endregion

        #region Scale Overtime Lerp
        protected virtual float ScaleOvertimeLerp(ref float time, float initalScale, float targetScale, float growthDuration)
        {
            if (time < growthDuration) { time += Time.deltaTime; }
            float t = time / growthDuration;
            float newScale = Mathf.Lerp(initalScale, targetScale, t);
            return newScale;
        }
        #endregion

    }

}