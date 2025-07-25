using UnityEngine;
using UnityEngine.UI;

namespace CombatZone.Objective
{

    public class ChangeDominationPointColor : MonoBehaviour
    {

        [SerializeField] private Image inner;

        [SerializeField] private Image outer;

        [SerializeField] private Image fill;

        [SerializeField][Range(0f, 1f)] private float alphaColor = 0.5f;

        public void ChangeDominationColor(Color color)
        {

            Color newColor = new Color(color.r, color.g, color.b, alphaColor);
            inner.color = newColor;
            outer.color = newColor;
            fill.color = color;

        }

        public void ChangeBarColorWhileCapturing(Color color)
        { fill.color = color; }

    }

}