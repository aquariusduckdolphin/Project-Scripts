#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Attribute
{

    [CustomPropertyDrawer(typeof(TitleAttribute), true)]
    public class TitlePropertyDrawer : PropertyDrawer
    {

        #region On GUI
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            TitleAttribute titleAttribute = (TitleAttribute)attribute;

            EditorGUI.BeginProperty(position, label, property);
            
            float labelWidth = GUI.skin.label.CalcSize(new GUIContent(titleAttribute.title)).x;
            
            float separatorWidth = (position.width - labelWidth - 20f) / 2;

            Rect leftSeparatorRect = CreateRect(position.x, position.y + titleAttribute.spacing, separatorWidth, titleAttribute.height);

            Rect labelRect = CreateRect(leftSeparatorRect.xMax + 10f, position.y, labelWidth, EditorGUIUtility.singleLineHeight);

            Rect rightSeparatorRect = CreateRect(labelRect.xMax + 10f, position.y + titleAttribute.spacing, separatorWidth, titleAttribute.height);

            Color textColor = ParsedColor.SetColor(titleAttribute.textColor);

            GUI.contentColor = textColor;

            EditorGUI.LabelField(labelRect, titleAttribute.title);

            Color lineColor = ParsedColor.SetColor(titleAttribute.lineColor);

            GUI.contentColor = Color.white;

            EditorGUI.DrawRect(leftSeparatorRect, lineColor);
            
            EditorGUI.DrawRect(rightSeparatorRect, lineColor);

            position.y += EditorGUIUtility.singleLineHeight + titleAttribute.spacing + titleAttribute.height;

            EditorGUI.PropertyField(position, property, label, true);

            EditorGUI.EndProperty();

        }
        #endregion

        #region Create Rectangle
        private Rect CreateRect(float xPosition, float yPosition, float width, float height)
        {

            return new Rect(xPosition, yPosition, width, height);

        }
        #endregion

        #region Get Property Height
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            TitleAttribute titleAttribute = (TitleAttribute)attribute;

            return base.GetPropertyHeight(property, label) + EditorGUIUtility.singleLineHeight+ titleAttribute.spacing;

        }
        #endregion

    }

}
#endif
