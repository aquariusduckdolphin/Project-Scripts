#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Attribute
{

    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorPropertyDrawer : DecoratorDrawer
    {

        #region On GUI
        public override void OnGUI(Rect position)
        {

            SeparatorAttribute separator = (SeparatorAttribute)attribute;

            Color lineColor = GUI.color;

            Color newLineColor = ParsedColor.SetColor(separator.color);

            Rect separatorRect = new Rect(position.xMin, position.yMin + separator.spacing / 2, position.width - 3, separator.height);

            EditorGUI.DrawRect(separatorRect, newLineColor);

            GUI.color = lineColor;

        }
        #endregion

        #region GetHeight
        public override float GetHeight()
        {

            SeparatorAttribute separator = (SeparatorAttribute)attribute;

            return separator.height + separator.spacing;

        }
        #endregion

    }

}
#endif
