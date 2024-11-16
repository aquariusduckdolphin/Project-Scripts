using UnityEngine;

namespace SceneManagerToolkit.Attribute
{

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class TitleAttribute :  PropertyAttribute
    {

        public readonly float height;

        public readonly float spacing;

        public readonly string textColor;

        public readonly string lineColor;

        public readonly string title;

        public TitleAttribute(string _title = "Title", float _height = 1f, float _spacing = 10f, string _lineColor = "#FFFFFF", string _textColor = "#000000")
        {

            title = _title;

            height = _height;

            spacing = _spacing;

            textColor = _textColor;

            lineColor = _lineColor;

        }
        
    }
}
