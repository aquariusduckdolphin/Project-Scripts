#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Editor
{

    public class SceneManagerUIUtilities : ScriptableObject
    {

        public Vector2 scroll = Vector2.zero;

        public int buttonHeight = 45;

        public GUISkin toolkitStyle;

        public GUIStyle textFieldStyle;

        /***************************************************************/

        #region Centering a Header
        public void SetHeaderToCenter(string labelName, float windowWidth)
        {

            GUIStyle headerStyle = toolkitStyle.GetStyle("Titles");
            headerStyle.fontSize = 20;
            headerStyle.alignment = TextAnchor.MiddleCenter;

            Vector3 textSize = headerStyle.CalcSize(new GUIContent(labelName));
            float xPos = (windowWidth - textSize.x) / 2f;
            Rect headerRect = EditorGUILayout.GetControlRect(GUILayout.Height(textSize.y));
            headerRect.x = xPos;
            headerRect.width = textSize.x;

            EditorGUI.LabelField(headerRect, labelName, headerStyle);

        }
        #endregion

        #region Create a help box
        public void DisplayHelpBox(string message, MessageType type, bool width)
        {

            EditorGUILayout.HelpBox(message, type, width);

        }
        #endregion

        /***************************************************************/

        #region User Input Check
        public string UserInputCheck(ref string input)
        {

            if(input == string.Empty || input == " ") { return "Enter Name"; }

            return input;

        }
        #endregion

        #region Display Button
        public void DisplayButton(string label, System.Action buttonAction)
        {

            GUIStyle buttonStyle = toolkitStyle.GetStyle("button");

            if (GUILayout.Button(label, buttonStyle, GUILayout.ExpandWidth(true), GUILayout.Height(buttonHeight)))
            { buttonAction.Invoke(); }

        }
        #endregion

        #region Display Float Value
        public void DisplayFloatValue(string label, ref float sliderValue, float min, float max)
        {

            GUIStyle textFieldStyle = toolkitStyle.GetStyle("textfield");

            sliderValue = Mathf.Clamp(EditorGUILayout.FloatField(label, sliderValue, textFieldStyle), min, max);

        }
        #endregion

        #region Display Integer Value
        public void DisplayIntValue(string label, ref int sliderValue, float min, float max)
        {

            GUIStyle textFieldStyle = toolkitStyle.GetStyle("textfield");

            sliderValue = (int)Mathf.Clamp(EditorGUILayout.IntField(label, sliderValue, textFieldStyle), min, max);

        }
        #endregion

        #region Display Color Field
        public void DisplayColorField(GUIContent label, ref Color color)
        {

            color = EditorGUILayout.ColorField(label, color, false, true, true);

        }
        #endregion

        #region Display Toggle Group
        public void DisplayToggleGroup(string label, ref bool toggleValue, System.Action displayContent)
        {

            toggleValue = EditorGUILayout.BeginToggleGroup(label, toggleValue);

            displayContent.Invoke();

            EditorGUILayout.EndToggleGroup();

        }
        #endregion

    }

}
#endif
