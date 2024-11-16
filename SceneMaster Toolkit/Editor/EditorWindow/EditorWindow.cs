#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace SceneManagerToolkit.Editor
{

    #region Menu Display
    public class QuickFlowObjectSpawner : MonoBehaviour
    {

        [MenuItem("Tools/SceneManagerToolkit #&s")]
        public static void Window()
        {

            SceneManagerToolkitEditorWindow.DisplaySpawnWindow();

        }

    }
    #endregion

    /***************************************************************/

    #region Display Window Object SpawnEditor
    public class SceneManagerToolkitEditorWindow : EditorWindow
    {

        #region Create Window
        private static SceneManagerToolkitEditorWindow window;

        public static void DisplaySpawnWindow()
        {

            window = GetWindow<SceneManagerToolkitEditorWindow>("SceneMaster Toolkit");

            if (window != null) { window.Show(); }

        }
        #endregion

        /***************************************************************/

        #region SceneMaster Toolkit Variables
        private SceneManagerUIUtilities uiUtility;

        private GUISkin quickFlowStyle;

        [Header("Tabs")]
        private string[] tabs = { "Object Spawner", "Group & Finder", "Material Creator" };

        private int tabIndex = 0;

        [Header("Scriptable Objects")]
        private MaterialCreator creator;

        private ObjectSpawner spawner;

        private Group group;

        private Header header;

        private Finder finder;

        private Renaming rename;

        private MaterialPreview materialPreview;
        #endregion

        private bool needsRepaint = false;

        int prevTabIndex = 3;

        /***************************************************************/

        #region On Enable & Disable
        private void OnEnable()
        {

            quickFlowStyle = Resources.Load<GUISkin>("SceneManagerToolkit");

            uiUtility = ScriptableObject.CreateInstance<SceneManagerUIUtilities>();

            spawner = ScriptableObject.CreateInstance<ObjectSpawner>();

            group = ScriptableObject.CreateInstance<Group>();

            header = ScriptableObject.CreateInstance<Header>();

            finder = ScriptableObject.CreateInstance<Finder>();

            rename = ScriptableObject.CreateInstance<Renaming>();

            creator = ScriptableObject.CreateInstance<MaterialCreator>();

            creator.uiUtility = uiUtility;

            needsRepaint = true;

        }

        private void OnDisable() 
        { 
            
            CloseMaterialPreviewWindow(materialPreview);

            SceneView.duringSceneGui -= spawner.OnSceneGUI;

            spawner.enableRandomSphereLocation = false;

            spawner.useDefaultLocation = false;
        
        }
        #endregion

        #region Display Everything in the GUI
        private void DisplayStartUp(GUIStyle subHeaderStyle, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            GUILayout.Label("SceneMaster Toolkit", subHeaderStyle);

            tabIndex = GUILayout.Toolbar(tabIndex, tabs, buttonStyle);

            float width = 310f;

            if(prevTabIndex != tabIndex)
            {

                needsRepaint = true;

                prevTabIndex = tabIndex;

            }

            switch (tabIndex)
            {
                case 0:
                    WindowSize(new Vector2(width, 825f));
                    CloseMaterialPreviewWindow(materialPreview);
                    SceneView.duringSceneGui += spawner.OnSceneGUI;
                    DisplayObjectSpawner(subHeaderStyle, textFieldsStyle, buttonStyle);
                    break;

                case 1:
                    WindowSize(new Vector2(width, 567f));
                    CloseMaterialPreviewWindow(materialPreview);
                    spawner.enableRandomSphereLocation = false;
                    spawner.useDefaultLocation = true;
                    SceneView.duringSceneGui -= spawner.OnSceneGUI;
                    DisplayGroupAndFinder(subHeaderStyle, textFieldsStyle, buttonStyle);
                    break;

                case 2:
                    WindowSize(new Vector2(width, 850f));
                    if (materialPreview == null) { materialPreview = (MaterialPreview)GetWindow<MaterialPreview>(); }
                    creator.materialPreviewWindow = materialPreview;
                    spawner.enableRandomSphereLocation = false;
                    spawner.useDefaultLocation = true;
                    SceneView.duringSceneGui -= spawner.OnSceneGUI;
                    DisplayMaterialCreator(textFieldsStyle);
                    break;
            }

            if (needsRepaint)
            {
                SceneView.RepaintAll();
                Repaint();
                needsRepaint = false;
            }

        }
        #endregion

        /***************************************************************/

        #region Display Object Spawner
        public void DisplayObjectSpawner(GUIStyle subHeaderStyle, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            uiUtility.scroll = GUILayout.BeginScrollView(uiUtility.scroll, false, false);
            {

                uiUtility.SetHeaderToCenter("Spawn New Object", position.width);

                spawner.DisplayObjectSpawner(uiUtility, textFieldsStyle, subHeaderStyle, buttonStyle);

            }
            GUILayout.EndScrollView();

        }
        #endregion

        /***************************************************************/

        #region Display Header, Group, And Finder
        private void DisplayGroupAndFinder(GUIStyle subHeaderStyle, GUIStyle textFieldsStyle, GUIStyle buttonStyle)
        {

            uiUtility.scroll = GUILayout.BeginScrollView(uiUtility.scroll, false, false);
            {

                uiUtility.SetHeaderToCenter("Group", position.width);

                group.DisplayGroup(uiUtility, subHeaderStyle, textFieldsStyle, buttonStyle);

                /**************/

                uiUtility.SetHeaderToCenter("Header", position.width);

                header.DisplayHeader(uiUtility, subHeaderStyle, textFieldsStyle, buttonStyle);

                /**************/

                uiUtility.SetHeaderToCenter("Finder", position.width);

                finder.DisplayFinder(uiUtility, textFieldsStyle, buttonStyle);

                /**************/

                uiUtility.SetHeaderToCenter("Renaming", position.width);

                rename.Display(uiUtility, textFieldsStyle, buttonStyle);

                Repaint();

            }
            GUILayout.EndScrollView();

        }
        #endregion

        /***************************************************************/

        #region Display Material Creator
        public void DisplayMaterialCreator(GUIStyle textFieldsStyle)
        {

            uiUtility.scroll = GUILayout.BeginScrollView(uiUtility.scroll, false, false);
            {

                uiUtility.SetHeaderToCenter("Material Creator", position.width);

                creator.DisplayMaterialCreatorGUI(this);

            }
            GUILayout.EndScrollView();

            Repaint();

        }
        #endregion

        #region Close Material Preview Window
        private void CloseMaterialPreviewWindow(MaterialPreview materialPreviewWindow)
        {

            if (materialPreviewWindow != null) { materialPreviewWindow.Close(); }

        }
        #endregion

        /***************************************************************/

        #region Window Size
        private void WindowSize(Vector2 windowSize)
        {

            if (window != null)
            {

                window.minSize = windowSize;
                window.maxSize = windowSize;

            }
            else
            { 
                //Debug.LogWarning("Window has not been found"); 
            }

        }
        #endregion

        #region Display in Window
        private void OnGUI()
        {

            GUIStyle subHeaderStyle = quickFlowStyle.GetStyle("label");

            GUIStyle textFieldsStyle = quickFlowStyle.GetStyle("textfield");

            uiUtility.textFieldStyle = quickFlowStyle.GetStyle("textfield");

            GUIStyle buttonStyle = quickFlowStyle.GetStyle("button");

            uiUtility.toolkitStyle = quickFlowStyle;

            GUIStyle wordStyle = quickFlowStyle.GetStyle("Words");

            GUIStyle titleStyle = quickFlowStyle.GetStyle("Titles");

            DisplayStartUp(subHeaderStyle, textFieldsStyle, buttonStyle);

        }
        #endregion

    }
    #endregion

}
#endif