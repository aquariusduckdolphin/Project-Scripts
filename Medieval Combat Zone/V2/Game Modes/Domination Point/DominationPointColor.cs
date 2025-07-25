using UnityEngine;
using UnityEngine.UI;

namespace CombatZone.Objective
{

    public class DominationPointColor : MonoBehaviour
    {

        /**************** Variables ****************/

        [Header("Ring")]
        [SerializeField] private Image innerRingeSolidImage;
        [SerializeField] private Image outerRingOutlineImage;
        [SerializeField] private Image ringFillImage;

        [Header("Minimap")]
        [SerializeField] private Image minimapRingImage;
        [SerializeField] private Image minimapBackgroundImage;
        [SerializeField] private Image minimapFillImage;

        [Header("Color")]
        [SerializeField][Range(0f, 1f)] private float alphaColor = 0.5f;
        [SerializeField] private Color newColor;

        /**************** Domination Point Color Methods ****************/

        #region Set Control Point Color
        public void SetControlPointColor(Color color)
        {
            newColor = new Color(color.r, color.g, color.b, alphaColor);

            innerRingeSolidImage.color = newColor;
            outerRingOutlineImage.color = newColor;
            ringFillImage.color = newColor;
            minimapRingImage.color = newColor;

            minimapBackgroundImage.color = newColor;
            minimapFillImage.color = newColor;
        }
        #endregion

        #region Update Bar Color
        public void UpdateBarColor(Color color)
        {
            ringFillImage.color = color;
            minimapFillImage.color = color;
        }
        #endregion

        #region Update Progress Bar
        public void UpdateProgressBar(float fillAmount)
        {
            ringFillImage.fillAmount = fillAmount;
            minimapFillImage.fillAmount = fillAmount;
        }
        #endregion

    }

}