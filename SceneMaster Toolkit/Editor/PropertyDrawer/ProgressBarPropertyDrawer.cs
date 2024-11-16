#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Attribute
{

    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer
    {

        #region On GUI
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            ProgressBarAttribute progressBar = (ProgressBarAttribute)attribute;

            if(property.propertyType == SerializedPropertyType.Integer)
            {

                int value = property.intValue;

                Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

                int slider = EditorGUI.IntSlider(rect, value, (int)progressBar.min, (int)progressBar.max);

                value = Mathf.Clamp(slider, (int)progressBar.min, (int)progressBar.max);

                DrawProgressBar(position, value, progressBar.min, progressBar.max, progressBar.label, progressBar.barColor, progressBar.textColor);

                property.intValue = value;

                EditorGUILayout.Space(0.5f);

            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {

                float value = property.floatValue;

                Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

                float slider = EditorGUI.Slider(rect, value, progressBar.min, progressBar.max);

                value = Mathf.Clamp(slider, (int)progressBar.min, (int)progressBar.max);

                DrawProgressBar(position, value, progressBar.min, progressBar.max, progressBar.label, progressBar.barColor, progressBar.textColor); ;

                property.floatValue = value;

                EditorGUILayout.Space(0.5f);

            }
            else
            {

                Rect helpBoxRect = new Rect(position.x, position.y, position.width, position.height + 10f);

                EditorGUI.HelpBox(helpBoxRect, "Progress Bar only takes float or int.", MessageType.Warning);

                EditorGUILayout.Space(8f);

            }
        
        }
        #endregion

        #region Draw Progress Bar Function
        private void DrawProgressBar(Rect _position, float _value, float _min, float _max, string _label, string _barColor, string _textColor)
        {

            float fillAmount = Mathf.InverseLerp(_min, _max, _value);

            Rect backgroundRect = new Rect(_position.x, _position.y, _position.width, _position.height);

            EditorGUI.DrawRect(backgroundRect, Color.gray);

            Rect fillRect = new Rect(_position.x, _position.y, _position.width * fillAmount, _position.height);

            Color barColor = ParsedColor.SetColor(_barColor);

            Color textColor = ParsedColor.SetColor(_textColor);

            EditorGUI.DrawRect(fillRect, barColor);

            GUI.contentColor = textColor;

            if (!string.IsNullOrEmpty(_label))
            {

                EditorGUI.LabelField(_position, $"{_label}: {_value:F2}");

            }
            else
            {

                EditorGUI.LabelField(_position, _value.ToString("F2"));

            }

            GUI.contentColor = Color.white;

        }
        #endregion

    }

}
#endif