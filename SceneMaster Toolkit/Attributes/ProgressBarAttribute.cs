#if UNITY_EDITOR
using UnityEngine;

namespace SceneManagerToolkit.Attribute
{

    public class ProgressBarAttribute : PropertyAttribute
    {

        #region Progress Bar Variables
        public readonly float min;

        private readonly float defaultMin = 0f;

        public readonly float max;

        private readonly float defaultMax = 100f;

        public readonly string label;

        private readonly string defaultLabel = "Progress Bar";

        public readonly string textColor;

        private readonly string defaultTextColor = "#000000";

        public readonly string barColor;

        private readonly string defaultBarColor = "#FFFFFF";
        #endregion

        #region Progress Bar Functions
        public ProgressBarAttribute(float _min = 0f, float _max = 100f, string _label = "Progress Bar", string _textColor = "#000000", string _barColor = "#FFFFFF")
        {

            min = _min; 
            
            max = _max;

            label = _label;

            textColor = _textColor;

            barColor = _barColor;

        }

        public ProgressBarAttribute(float _min, float _max)
        {

            min = _min;

            max = _max;

            label = defaultLabel;

            textColor = defaultTextColor;

            barColor = defaultBarColor;

        }

        public ProgressBarAttribute(string _textColor, string _barColor)
        {

            min = defaultMin;

            max = defaultMax;

            label = defaultLabel;

            textColor = _textColor;

            barColor = _barColor;

        }

        public ProgressBarAttribute(string _label)
        {

            min = defaultMin;

            max = defaultMax;

            label = _label;

            textColor = defaultTextColor;

            barColor = defaultBarColor;

        }

        #endregion

    }

}
#endif