using UnityEngine;
using UnityEngine.UI;

namespace CombatZone.Objective
{

    public class FillBar : MonoBehaviour
    {

        [SerializeField] private Image barFillImage;

        public void FillTheBar(float amount)
        {

            barFillImage.fillAmount = amount;

        }

    }

}