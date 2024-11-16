#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Editor
{

    public class GroupAndFinder : ScriptableObject
    {

        protected GameObject[] selectedObjects = new GameObject[0];

        protected string defaultName = "Enter Name";

    }

    public class Group : GroupAndFinder
    {

        #region Group Variables
        [Header("Group Variables")]
        public string wantedName = string.Empty;

        private GameObject parentGameObject;

        public int currentSelectedAmount;
        #endregion

        /***************************************************************/

        #region Group
        public void GroupSelectedObjects(SceneManagerUIUtilities uiUtility)
        {

            GameObject groupGO = new GameObject(wantedName);

            Vector3 center = Vector3.zero;

            foreach (GameObject go in selectedObjects)
            {

                center += go.transform.position;

                if (go.transform.parent != null)
                { parentGameObject = go.transform.parent.gameObject; }

            }
        
            center /= selectedObjects.Length;

            groupGO.transform.position = center;

            foreach(GameObject go in selectedObjects)
            {

                go.transform.SetParent(groupGO.transform);

            }

            if (parentGameObject != null)
            { PermanentlyDestroyGameobject(); }

        }
        #endregion

        #region Ungroup
        public void UngroupGameObjects(SceneManagerUIUtilities uiUtility)
        {

            foreach (GameObject go in selectedObjects)
            {

                if (go.transform.parent == null) { return; }

                parentGameObject = go.transform.parent.gameObject;

                go.transform.parent = null;

            }

            PermanentlyDestroyGameobject();

        }
        #endregion

        #region Permanently Destroy Gameobject
        private void PermanentlyDestroyGameobject()
        {

            if (parentGameObject.transform.childCount == 0)
            { DestroyImmediate(parentGameObject); }

        }
        #endregion

        #region Destroy Gameobject(s) Option Box
        public void DestroyGameObject(SceneManagerUIUtilities uiUtility)
        {

            int cool = EditorUtility.DisplayDialogComplex(
                "Destroy GameObject",
                "This will permanently destory the GameObject. Undo will NOT bring back the object.",
                "Continue", 
                "Cancel", 
                "");

            switch(cool)
            {

                case 0:
                    foreach (GameObject go in selectedObjects)
                    { DestroyImmediate(go); }
                    break;

                case 1:
                    return;

                case 2:
                    return;

                default:
                    Debug.LogError("Destroying GameObject(s) failed");
                    break;

            }

        }
        #endregion

        /***************************************************************/

        #region Display Group
        public void DisplayGroup(SceneManagerUIUtilities uiUtility, GUIStyle subHeaderStyle, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            #region Selection Count
            {

                selectedObjects = Selection.gameObjects;
                currentSelectedAmount = selectedObjects.Length;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Selected Count: " + currentSelectedAmount.ToString(), subHeaderStyle);
            
            }
            #endregion

            #region Name
            {
                wantedName = EditorGUILayout.TextField("Group Name", wantedName, textFieldsStyle);
                wantedName = uiUtility.UserInputCheck(ref wantedName);
            }
            #endregion

            #region Butttons
            EditorGUILayout.BeginHorizontal();
            {

                EditorGUI.BeginDisabledGroup(wantedName == string.Empty || wantedName == defaultName);
                {

                    uiUtility.DisplayButton("Group Selected", () => GroupSelectedObjects(uiUtility));

                }
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(1);

                EditorGUI.BeginDisabledGroup(selectedObjects.Length == 0f);
                {

                    uiUtility.DisplayButton("Ungroup Selected", () => UngroupGameObjects(uiUtility));

                }
                GUILayout.EndHorizontal();

                uiUtility.DisplayButton("Destroy GameObject", () => DestroyGameObject(uiUtility));

            }
            EditorGUI.EndDisabledGroup();
            #endregion

        }
        #endregion

    }

    public class Header : GroupAndFinder
    {

        #region Header Variables
        public string wantedName = string.Empty;

        public bool useEqualSigns = false;

        public int totalDashCount = 0;

        private const string hyphenSymbol = "-";

        private const string equalSymbol = "=";
        #endregion

        /***************************************************************/

        #region Spawn Header Object
        public void SpawnHeaderGameObject(SceneManagerUIUtilities uiUtility)
        {

            int halfDashCount = totalDashCount / 2;

            string leftSideDash = GenerateDashString(halfDashCount);

            string rightSideDash = GenerateDashString(totalDashCount % 2 == 0 ? halfDashCount : halfDashCount + 1);

            if(wantedName == defaultName) { wantedName = " "; }

            GameObject go = new GameObject(leftSideDash + wantedName + rightSideDash);

        }
        #endregion

        #region Generate Dash String
        private string GenerateDashString(int dashCount)
        {

            char dashChar = useEqualSigns ? equalSymbol[0] : hyphenSymbol[0];

            return new string(dashChar, dashCount);

        }
        #endregion

        /***************************************************************/

        #region Display
        public void DisplayHeader(SceneManagerUIUtilities uiUtility, GUIStyle subHeaderStyle, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            wantedName = EditorGUILayout.TextField("Header Name", wantedName, textFieldsStyle);

            wantedName = uiUtility.UserInputCheck(ref wantedName);

            #region Dash
            totalDashCount = EditorGUILayout.IntField("Number of dashes", totalDashCount, textFieldsStyle);

            totalDashCount = (int)Mathf.Clamp(totalDashCount, 10, Mathf.Infinity);

            useEqualSigns = EditorGUILayout.BeginToggleGroup("Use equal sign", useEqualSigns);
            {

                if (useEqualSigns) { EditorGUILayout.LabelField("Equal Sign: " + equalSymbol.ToString(), subHeaderStyle); }
                else { EditorGUILayout.LabelField("Equal Sign: " + equalSymbol.ToString(), subHeaderStyle); }

            }
            EditorGUILayout.EndToggleGroup();
            #endregion

            uiUtility.DisplayButton("Header Object", () => SpawnHeaderGameObject(uiUtility));

        }
        #endregion

    }

    public class Finder : GroupAndFinder
    {

        #region Finder Variables
        [Header("Finder Variable")]
        public string userInput = string.Empty;

        public int selectedIndex = 0;

        public FilterTypes filter;

        public List<GameObject> selectedGameObjects = new List<GameObject>();
        #endregion

        /***************************************************************/

        #region Select all Gameobjects
        public void SelectAllGameObjects()
        {

            UpdateSelection();

            Selection.objects = selectedGameObjects.ToArray();

        }

        private void UpdateSelection()
        {

            selectedGameObjects.Clear();

            selectedObjects = FindObjectsOfType<GameObject>();

            FilterStates(selectedObjects);

        }

        private void OnHierarchyChange()
        {

            UpdateSelection();

        }
        #endregion

        #region Cycle Selection

        #region Select Next Gameobject
        public void SelectNextGameObject()
        {

            if (selectedGameObjects.Count <= 0) { return; }

            if (selectedIndex >= selectedGameObjects.Count - 1) { selectedIndex = 0; }
            else { selectedIndex++; }

            HighlightSelection();

        }
        #endregion

        #region Select Previous Gameobject
        public void SelectPreviousGameObject()
        {

            if (selectedGameObjects.Count <= 0) { return; }

            if (selectedIndex <= 0) { selectedIndex = selectedGameObjects.Count - 1; }
            else { selectedIndex--; }

            HighlightSelection();

        }
        #endregion

        #region Highlight Selection
        private void HighlightSelection()
        {

            GameObject nextObject = selectedGameObjects[selectedIndex];

            if (nextObject != null)
            {

                Selection.activeGameObject = nextObject;

                SceneView.lastActiveSceneView.FrameSelected();

            }

        }
        #endregion

        #endregion

        #region Filter Options

        #region Filter States
        private void FilterStates(GameObject[] gameObject)
        {

            switch (filter)
            {

                case FilterTypes.GameObjectName:
                    GameObjectName(gameObject);
                    break;

                case FilterTypes.Tag:
                    TagName(gameObject);
                    break;

                case FilterTypes.Layer:
                    LayerName(gameObject);
                    break;

                case FilterTypes.Component:
                    ComponentName(gameObject);
                    break;

            }

        }
        #endregion

        #region Gameobject Name
        private void GameObjectName(GameObject[] gameObjects)
        {

            foreach (GameObject go in gameObjects)
            {

                string selectedName = null;

                if (userInput.Length <= go.name.Length)
                { selectedName = go.name.Substring(0, userInput.Length); }

                if (selectedName == userInput)
                { selectedGameObjects.Add(go); }

            }

        }
        #endregion

        #region Tag Name
        private void TagName(GameObject[] gameObjects)
        {

            foreach (GameObject go in gameObjects)
            {

                if (go.transform.tag == userInput)
                { selectedGameObjects.Add(go); }

            }

        }
        #endregion

        #region Layer Name
        private void LayerName(GameObject[] gameObjects)
        {

            LayerMask mask = LayerMask.NameToLayer(userInput);

            foreach (GameObject go in gameObjects)
            {

                if (go.layer == mask)
                { selectedGameObjects.Add(go); }

            }

        }
        #endregion

        #region Component Name
        private void ComponentName(GameObject[] gameObjects)
        {

            string componentName = userInput.Replace(" ", string.Empty);

            foreach (GameObject go in gameObjects)
            {

                Component[] components = go.transform.GetComponents<Component>();

                foreach (Component comp in components)
                {

                    if (comp.GetType().Name.Equals(componentName, System.StringComparison.OrdinalIgnoreCase))
                    { selectedGameObjects.Add(go); }

                }

            }

        }
        #endregion

        #endregion

        /***************************************************************/

        #region Display
        public void DisplayFinder(SceneManagerUIUtilities uiUtility, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            userInput = EditorGUILayout.TextField("GameObject Name", userInput, textFieldsStyle);
            userInput = uiUtility.UserInputCheck(ref userInput);

            filter = (FilterTypes)EditorGUILayout.EnumPopup("Filter type: ", filter);

            #region Buttons
            {

                EditorGUI.BeginDisabledGroup(userInput == defaultName);
                {

                    uiUtility.DisplayButton("Select All", () => SelectAllGameObjects());

                    EditorGUILayout.BeginHorizontal();
                    {

                        GUILayout.Label("Cycle Selection:");

                        uiUtility.DisplayButton("Previous", () => SelectPreviousGameObject());
                        uiUtility.DisplayButton("Next", () => SelectNextGameObject());

                    }
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUI.EndDisabledGroup();

            }
            #endregion

        }
        #endregion

    }

    public class Renaming : GroupAndFinder
    {

        #region Renaming Variables
        private string wantedRename = string.Empty;

        private string previousName = string.Empty;

        private bool appendIDToName = true;

        private int id = 0;

        private IDStyle idType;
        #endregion

        /***************************************************************/

        #region ID Types
        private void IDType()
        {

            switch (idType)
            {

                case IDStyle.Parentheses:
                    NamingObjects(wantedRename, " (", ")");
                    break;

                case IDStyle.Underscore:
                    NamingObjects(wantedRename, "_", string.Empty);
                    break;

                case IDStyle.Period:
                    NamingObjects(wantedRename, ".", string.Empty);
                    break;

            }

        }
        #endregion

        #region Naming Objects
        private void NamingObjects(string newName, string startType, string endType)
        {

            for (int i = 0; i < selectedObjects.Length; i++)
            {

                selectedObjects[i].transform.name = newName + startType + id + endType;

                id++;

            }

        }
        #endregion

        #region Rename Method
        private void RenameObjects()
        {

            selectedObjects = Selection.gameObjects;

            if(wantedRename == defaultName) { wantedRename = " "; }

            if (previousName != wantedRename)
            {

                previousName = wantedRename;

                id = 0;

            }

            if (!appendIDToName)
            {

                foreach (GameObject go in selectedObjects) { go.name = wantedRename; }

            }
            else
            {

                IDType();

            }

        }
        #endregion

        /***************************************************************/

        #region Display
        public void Display(SceneManagerUIUtilities uiUtility, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            wantedRename = EditorGUILayout.TextField("Name", wantedRename, textFieldsStyle);

            wantedRename = uiUtility.UserInputCheck(ref wantedRename);

            #region ID
            {

                appendIDToName = EditorGUILayout.BeginToggleGroup("Append ID", appendIDToName);

                idType = (IDStyle)EditorGUILayout.EnumPopup("ID type: ", idType);

                id = EditorGUILayout.IntField("Object ID", id, textFieldsStyle);

                id = (int)Mathf.Clamp(id, 0, Mathf.Infinity);

                EditorGUILayout.EndToggleGroup();

            }
            #endregion

            uiUtility.DisplayButton("Rename", () => RenameObjects());

        }
        #endregion

    }

}
#endif