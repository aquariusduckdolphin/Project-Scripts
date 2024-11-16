#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Editor
{

    public class ObjectSpawner : ScriptableObject
    {

        #region Spawn Variables
        [Header("Object Name")]
        private string objectBaseName = string.Empty;

        private string previousObjectBaseName = string.Empty;

        private string defaultName = "Enter Name";

        [Header("Prefab Object to Spawn")]
        private GameObject prefabToSpawn;

        private Transform spawnContainer;

        [Header("Textures")]
        private Texture2D objectIcon;

        [Header("ID")]
        private int objectID = 0;

        private bool shouldAppendID = true;

        [Header("Random Sphere Location")]
        public bool enableRandomSphereLocation = false;

        private bool previousEnableRandomSphereLocation;

        private float sphereSpawnRadius = 5f;

        private Vector3 sphereCenter = Vector3.zero;

        private float minRandomScale = 0.001f;

        private float maxRandomScale = 1.0f;

        [Header("Default Spawn Location")]
        public bool useDefaultLocation = true;

        private bool previousUseDefaultLocation;

        private Vector3 defaultSpawnLocation = Vector3.zero;

        [Header("Random Box Location")]
        private Vector3 randomBoxMinPosition;

        private Vector3 randomBoxMaxPosition;

        private float objectScaleFactor = 1f;

        private float uniformScaleValue = 1f;
        #endregion

        /***************************************************************/

        #region Sphere Box Spawn Position
        private Vector3 SphereBoxSpawnPosition()
        {

            Vector3 spawnPos = Vector3.zero;

            if (!enableRandomSphereLocation)
            {

                if (useDefaultLocation)
                { spawnPos = defaultSpawnLocation; }

                if (!useDefaultLocation)
                { spawnPos = new Vector3(Random.Range(randomBoxMinPosition.x, randomBoxMaxPosition.x), Random.Range(randomBoxMinPosition.y, randomBoxMaxPosition.y), Random.Range(randomBoxMinPosition.z, randomBoxMaxPosition.z)); }

                objectScaleFactor = uniformScaleValue;

            }
            else
            {

                Vector2 spawnCircle = Random.insideUnitCircle * sphereSpawnRadius;

                spawnPos = new Vector3(sphereCenter.x + spawnCircle.x, sphereCenter.y, sphereCenter.z + spawnCircle.y);

                objectScaleFactor = Random.Range(minRandomScale, maxRandomScale);

            }

            return spawnPos;

        }
        #endregion

        #region Spawn Object
        public void SpawnObject(bool objectType)
        {

            Vector3 spawnPos = SphereBoxSpawnPosition();

            CreateObjectAndName(spawnPos, objectType);

        }
        #endregion

        #region Create Object and Name
        private void CreateObjectAndName(Vector3 spawnPos, bool objectType)
        {

            string objName = objectBaseName;

            if (previousObjectBaseName != null && previousObjectBaseName != objectBaseName)
            {

                previousObjectBaseName = objectBaseName;

                objectID = 0;

            }
            else
            { previousObjectBaseName = objectBaseName; }

            if (shouldAppendID)
            {

                objName += objName += objectID.ToString();

                objectID++;
            }

            GameObject newObject = ObjectType(spawnPos, objectType);

            if (objectIcon != null)
            { EditorGUIUtility.SetIconForObject(newObject, objectIcon); }

            newObject.name = objName;

            newObject.transform.localScale = Vector3.one * objectScaleFactor;

        }
        #endregion

        #region Object Type
        private GameObject ObjectType(Vector3 spawnPos, bool spawnPrefab)
        {

            GameObject newObject;

            if (spawnPrefab)
            {

                newObject = (GameObject)
                PrefabUtility.InstantiatePrefab(prefabToSpawn, spawnContainer);

                newObject.transform.position = spawnPos;

            }
            else
            {

                newObject = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, spawnContainer);

            }

            return newObject;

        }
        #endregion

        #region Display Default Spawn Field
        private void DisplayDefaultSpawnField()
        {

            defaultSpawnLocation = EditorGUILayout.Vector3Field(
                        "Default Location",
                        defaultSpawnLocation);

        }
        #endregion

        #region Random Minimum & Maximum Position
        private void RandomMinAndMaxPosition()
        {

            randomBoxMinPosition = EditorGUILayout.Vector3Field("Minimum Location", randomBoxMinPosition);

            randomBoxMaxPosition = EditorGUILayout.Vector3Field("Maximum Location", randomBoxMaxPosition);

        }
        #endregion

        #region Box Spawn Function
        public void BoxSpawn()
        {

            useDefaultLocation = EditorGUILayout.BeginToggleGroup("Random Box Spawn Location", useDefaultLocation);
            {

                if (useDefaultLocation) { DisplayDefaultSpawnField(); }
                else { DisplayDefaultSpawnField(); }

                EditorGUI.BeginDisabledGroup(useDefaultLocation);
                {

                    if (useDefaultLocation) { RandomMinAndMaxPosition(); }

                }
                EditorGUI.EndDisabledGroup();

            }
            EditorGUILayout.EndToggleGroup();

            if (!useDefaultLocation) { RandomMinAndMaxPosition(); }

        }
        #endregion

        #region Show wireframes
        public void OnSceneGUI(SceneView sceneView)
        {

            if (enableRandomSphereLocation)
            {

                Handles.color = Color.yellow;

                Handles.DrawWireDisc(sphereCenter, Vector3.up, sphereSpawnRadius);

            }

            if (!useDefaultLocation)
            {

                Handles.color = Color.yellow;

                Handles.DrawWireCube((randomBoxMinPosition + randomBoxMaxPosition) / 2, randomBoxMaxPosition - randomBoxMinPosition);

            }

            if (enableRandomSphereLocation != previousEnableRandomSphereLocation ||
                useDefaultLocation != previousUseDefaultLocation)
            {

                SceneView.RepaintAll();

                previousEnableRandomSphereLocation = enableRandomSphereLocation;

                previousUseDefaultLocation = useDefaultLocation;

            }

        }
        #endregion

        /***************************************************************/

        #region Display
        public void DisplayObjectSpawner(SceneManagerUIUtilities uiUtility, GUIStyle textFieldsStyle, GUIStyle subHeaderStyle, GUIStyle buttonStyle)
        {

            #region Object Name
            {
                objectBaseName = EditorGUILayout.TextField("Object Name", objectBaseName, textFieldsStyle);

                objectBaseName = uiUtility.UserInputCheck(ref objectBaseName);

                if (objectBaseName == defaultName || objectBaseName == " ")
                {
                    uiUtility.DisplayHelpBox(
                    "Assign a name to the object to be spawned.",
                    MessageType.Error, true);
                }
            }
            #endregion

            EditorGUILayout.Space(2);

            prefabToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", prefabToSpawn, typeof(GameObject), false) as GameObject;
            
            if (prefabToSpawn == null)
            { uiUtility.DisplayHelpBox("Place a GameObject in the 'Prefab to Spawn' field.", MessageType.Error, true); }

            EditorGUILayout.Space(2);

            objectIcon = EditorGUILayout.ObjectField("Object Icon", objectIcon, typeof(Texture2D), false) as Texture2D;

            if (objectIcon == null)
            { uiUtility.DisplayHelpBox("Adds an Object Icon to the gameObject. Is not require to have.", MessageType.Info, false); }

            EditorGUILayout.Space(2);

            #region Spawn Under Parent
            spawnContainer = EditorGUILayout.ObjectField("Spawn Container", spawnContainer, typeof(Transform), true) as Transform;

            if (spawnContainer != null && EditorUtility.IsPersistent(spawnContainer))
            { uiUtility.DisplayHelpBox("Spawn Container must be a scene object.", MessageType.Error, false); }

            uiUtility.DisplayHelpBox("Spawn Container not required.", MessageType.None, false);
            #endregion

            EditorGUILayout.Space(2);

            uiUtility.DisplayToggleGroup("Append ID", ref shouldAppendID, () => uiUtility.DisplayIntValue("Object ID", ref objectID, 0f, Mathf.Infinity));

            EditorGUILayout.Space(2);

            #region Box Spawn
            EditorGUI.BeginDisabledGroup(enableRandomSphereLocation);

            if (enableRandomSphereLocation)
            { BoxSpawn(); }
            else
            { BoxSpawn(); }

            EditorGUI.EndDisabledGroup();
            #endregion

            EditorGUILayout.Space(2);

            #region Use Random Spawn
            enableRandomSphereLocation = EditorGUILayout.BeginToggleGroup("Random Sphere Location", enableRandomSphereLocation);

            sphereCenter = EditorGUILayout.Vector3Field("Default Sphere Location", sphereCenter);

            uiUtility.DisplayFloatValue("Spawn Radius", ref sphereSpawnRadius, 0f, Mathf.Infinity);
            uiUtility.DisplayFloatValue("Minimum Random Scale", ref minRandomScale, 0.001f, maxRandomScale);
            uiUtility.DisplayFloatValue("Maximum Random Scale", ref maxRandomScale, minRandomScale, Mathf.Infinity);

            EditorGUILayout.EndToggleGroup();
            #endregion

            #region Button
            EditorGUI.BeginDisabledGroup(prefabToSpawn == null || objectBaseName == "Enter Name" ||
                (spawnContainer != null && EditorUtility.IsPersistent(spawnContainer)));

            uiUtility.DisplayButton("Spawn GameObject", () => SpawnObject(false));
            uiUtility.DisplayButton("Spawn Prefab GameObject", () => SpawnObject(true));

            EditorGUI.EndDisabledGroup();
            #endregion

        }
        #endregion

    }

}
#endif