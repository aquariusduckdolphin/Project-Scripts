#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit
{

    [CustomPropertyDrawer(typeof(RequiredAttribute))]
    public class RequiredPropertyDrawer : PropertyDrawer
    {

        #region Property Height
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            if (IsFieldEmpty(property))
            {

                float height = EditorGUIUtility.singleLineHeight * 2f;

                height += base.GetPropertyHeight(property, label);

                return height;

            }
            else
            {

                return base.GetPropertyHeight (property, label);

            }

        }
        #endregion

        #region On GUI
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (!IsFieldSupported(property))
            {

                Debug.LogError("Required Attribute place on incompatible field type.");

                return;

            }

            if (IsFieldEmpty(property))
            {

                Rect helpBoxPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight * 2f);

                position.height = EditorGUIUtility.singleLineHeight * 2.14f;

                position.height += base.GetPropertyHeight(property, label);

                //Color highlightColor = Functions.SetColor();

                Color newColor = Color.red;

                newColor.a = 0.2f;

                EditorGUI.DrawRect(position, newColor);

                EditorGUI.HelpBox(helpBoxPosition, "Required", MessageType.Error);

                position.height += base.GetPropertyHeight(property, label);

                position.height = EditorGUIUtility.singleLineHeight * 2.14f;

                position.y += EditorGUIUtility.singleLineHeight * 2.1f;

                position.height = EditorGUIUtility.singleLineHeight;

            }

            EditorGUI.PropertyField(position, property, label);

            GUILayout.Space(3f);

        }
        #endregion

        #region Check if the field is empty
        bool IsFieldEmpty(SerializedProperty property)
        {

            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null)
            {

                return true;

            }

            if (property.propertyType == SerializedPropertyType.String && string.IsNullOrEmpty(property.stringValue))
            {

                return true;

            }

            return false;

        }
        #endregion

        #region Is field supported
        bool IsFieldSupported(SerializedProperty property)
        {

            if(property.propertyType == SerializedPropertyType.ObjectReference)
            {

                return true;

            }

            if(property.propertyType == SerializedPropertyType.String)
            {

                return true;

            }

            return false;

        }
        #endregion

    }

}
#endif