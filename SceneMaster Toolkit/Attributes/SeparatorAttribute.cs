#if UNITY_EDITOR
using UnityEngine;

namespace SceneManagerToolkit.Attribute
{

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class SeparatorAttribute : PropertyAttribute
    {

        #region Separator Varaibles
        public readonly float height;

        public readonly float spacing;

        public readonly float width;

        public readonly string color;
        #endregion

        #region Separator Attribute Methods
        public SeparatorAttribute(float _height = 1f, float _width = 220f, float _spacing = 20f, string _color = "#FFFFFF")
        {

            height = _height;

            width = _width;

            spacing = _spacing;

            color = _color;

        }

        public SeparatorAttribute(float _height, float _width, float _spacing)
        {

            height = _height;

            width = _width;

            spacing = _spacing;

            color = "#FFFFFF";

        }

        public SeparatorAttribute(float _height, float _width, string _color)
        {

            height = _height;

            width = _width;

            spacing = 20f;

            color = _color;

        }

        public SeparatorAttribute(float _height, float _width)
        {

            height = _height;

            width = _width;

            spacing = 20f;

            color = "#FFFFFF";

        }

        public SeparatorAttribute(float _width, string _color)
        {

            height = 1f;

            width = _width;

            spacing = 20f;

            color = _color;

        }

        public SeparatorAttribute(float _width)
        {

            height = 1f;

            width = _width;

            spacing = 20f;

            color = "#FFFFFF";

        }

        public SeparatorAttribute(string _color)
        {

            height = 1f;

            width = 220f;

            spacing = 20f;

            color = _color;

        }
        #endregion

        #region Convert to Color
        public Color SetColor()
        {

            if (ColorUtility.TryParseHtmlString(color, out Color parsedColor))
            {

                return parsedColor;

            }

            return Color.white;

        }
        #endregion

    }

}
#endif