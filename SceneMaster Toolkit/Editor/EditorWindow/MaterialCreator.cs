#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Editor
{

    public class MaterialCreator : ScriptableObject
    {

        #region Material Variables

        #region Shared Info

        [Header("Material Name")]
        private string materialName = string.Empty;

        [Header("Folder Path")]
        private static string previousFolderPath = "Assets";

        [Header("References")]
        public MaterialPreview materialPreviewWindow;

        public SceneManagerUIUtilities uiUtility;

        [Header("Texture Map")]
        private Material material;
        private Texture2D albedoTextureMap;
        private Texture2D metallicTextureMap;
        private Texture2D roughnessTextureMap;
        private Texture2D normalTextureMap;
        private Texture2D heightTextureMap;
        private Texture2D occlusionTextureMap;
        private Texture2D emissionTextureMap;

        [Header("Material Options")]
        private MaterialType type = MaterialType.Object_Shader;

        private bool useRoughnessMap = false;

        private float smoothness = 0.5f;

        private bool useEmissionMap;

        private Color emissionColor = Color.black;

        [Header("Material Property References")]
        private const string BaseMap = "_BaseMap";
        private const string MetallicGlossMap = "_MetallicGlossMap";
        private const string UseRoughnessMap = "_UseRoughnessMap";
        private const string RoughnessMap = "_Roughness";
        private const string SmoothnessProperty = "_Glossiness";
        private const string OcclusionMap = "_OcclusionMap";
        private const string HeightMap = "_ParallaxMap";
        private const string NormalMap = "_BumpMap";
        private const string EmissionMap = "_EmissionMap";
        private const string EmissionColorProperty = "_EmissionColor";

        #endregion

        #region Unity Shaders

        [Header("Shader Paths")]
        private const string standard = "Standard";
        private const string urp = "Universal Render Pipeline";
        private const string hdrp = "High Definition Renderer Pipeline";

        #endregion

        #region Custom Shader

        [Header("Shader Paths")]
        private const string objectShader = "Object Shader";
        private const string transparentObjectShader = "Transparent Object Shader";

        [Header("Texture Map")]
        private Texture2D alpha;

        [Header("Material Options")]
        private Color baseColor = Color.white;

        private bool useMetallic = false;

        private float metallicness = 0.5f;

        private float normalStrength = 1f;

        private bool useHeightMap = false;

        private float heightStrength = 0.005f;

        private bool hasOpacityMap = false;

        [Header("Material Property References")]
        private const string BaseColorProperty = "_BaseColor";
        private const string HasMetalllicMap = "_HasMetallicMap";
        private const string MetallicProperty = "_MetallicValue";
        private const string NormalMapStrength = "_NormalMapStrength";
        private const string HasHeightMap = "_HasHeightMap";
        private const string HeightStrength = "_HeightStrength";
        private const string HasOpacity = "_HasOpacity";
        private const string AlphaMap = "_Alpha";

        #endregion

        #endregion

        /************************ Button ************************/
        
        #region Create the Material
        private void CreateMaterial(System.Action saveType)
        {

            MaterialState(type);
            saveType.Invoke();
            SetTextures();

        }
        #endregion

        #region Set Textures
        private void SetTextures()
        {

            CheckTexture(BaseMap, albedoTextureMap);
            material.SetColor(BaseColorProperty, baseColor);

            if(type != MaterialType.Object_Shader && type != MaterialType.Transparent_Object_Shader)
            {

                Texture2D tempTexture = useRoughnessMap ? roughnessTextureMap : metallicTextureMap;
                CheckTexture(MetallicGlossMap, tempTexture);

            }
            else
            {

                float hasMetallicness = useMetallic ? 1 : 0;
                material.SetFloat(HasMetalllicMap, hasMetallicness);
                CheckTexture(MetallicGlossMap, metallicTextureMap);
                material.SetFloat(MetallicProperty, metallicness);
                CheckTexture(RoughnessMap, roughnessTextureMap);

            }

            material.SetFloat(SmoothnessProperty, smoothness);

            NormalMaps(normalTextureMap, TextureImporterType.NormalMap);
            CheckTexture(NormalMap, normalTextureMap);
            material.SetFloat(NormalMapStrength, normalStrength);

            float shouldEnableHeight = useHeightMap ? 1 : 0;
            material.SetFloat(HasHeightMap, shouldEnableHeight);
            CheckTexture(HeightMap, heightTextureMap);
            material.SetFloat(HeightStrength, heightStrength);

            CheckTexture(OcclusionMap, occlusionTextureMap);

            float shouldBeEnabled = hasOpacityMap ? 1 : 0;
            material.SetFloat(HasOpacity, shouldBeEnabled);          
            material.SetTexture(AlphaMap, alpha);

            if (!useEmissionMap)
            {
                DisableEmission();
                return;
            }

            EnableEmission();
            CheckTexture(EmissionMap, emissionTextureMap);
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            material.SetColor(EmissionColorProperty, emissionColor);

        }
        #endregion

        #region Reset Material Creator
        private void ClearMaterialCreator()
        {

            materialName = "Enter Name";
            albedoTextureMap = null;
            metallicTextureMap = null;
            useRoughnessMap = false;
            roughnessTextureMap = null;
            normalTextureMap = null;
            heightTextureMap = null;
            occlusionTextureMap = null;
            useEmissionMap = false;
            emissionTextureMap = null;
            smoothness = 0.5f;
            emissionColor = Color.black;

        }
        #endregion
        
        /************************ Clean Code ************************/
        
        #region Check Textures
        private void CheckTexture(string textureMapName, Texture2D textureMap)
        {

            if (textureMap == null) { return; }
            material.SetTexture(textureMapName, textureMap);

        }
        #endregion

        #region Set Material Preview Window Texture
        private void SetMaterialPreviewWindow(string textureMapName, Texture2D textureMap)
        {

            materialPreviewWindow.previewMaterial.SetTexture(textureMapName, textureMap);

        }
        #endregion

        #region Automatically convert to normal map
        private void NormalMaps(Texture2D normaMap, TextureImporterType mapType)
        {

            if (normaMap == null) { return; }
            string assetPath = AssetDatabase.GetAssetPath(normaMap);

            TextureImporter import = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (import.textureType == mapType) { return; }
            import.textureType = mapType;
            import.SaveAndReimport();

        }
        #endregion

        #region Material Shader State
        private void MaterialState(MaterialType typesOfMaterial)
        {

            switch (typesOfMaterial)
            {

                case MaterialType.Standard:
                    CreateMaterialShader(standard, standard);
                    break;

                case MaterialType.Universal_Renderer_Pipeline:
                    CreateMaterialShader(urp, urp + "/Lit");
                    break;

                case MaterialType.High_Definition_Renderer_Pipeline:
                    CreateMaterialShader(hdrp, "HDRP/Lit");
                    break;

                case MaterialType.Object_Shader:
                    CreateMaterialShader(objectShader, "Shader Graphs/" + objectShader);
                    break;

                case MaterialType.Transparent_Object_Shader:
                    CreateMaterialShader(transparentObjectShader, "Shader Graphs/" + transparentObjectShader);
                    break;


            }

        }
        #endregion

        #region Create Material Shader
        private void CreateMaterialShader(string shaderLabel, string shaderPath)
        {

            EditorGUILayout.LabelField("Material Shader: " + shaderLabel);
            Shader shader = Shader.Find(shaderPath);
            if (shader == null) { return; }
            material = new Material(shader);
            materialPreviewWindow.previewMaterial.shader = shader;

        }
        #endregion
        
        /************************ Save Material Option Methods ************************/

        #region Allow the user to place the material and name
        private void OpenFolderForMaterialDisplay()
        {

            string path = EditorUtility.SaveFilePanelInProject("Save Material", 
                uiUtility.UserInputCheck(ref materialName),
                "mat", 
                "Save Material");
            if (!string.IsNullOrEmpty(path)) { return; }

            AssetDatabase.CreateAsset(material, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }
        #endregion

        #region Get Current Location & Save at Current Location

        #region Get Current Location
        private string GetCurrentLocation()
        {

            string folderPath = "Assets";

            Object selectedObject = Selection.activeObject;
            if (selectedObject == null) { return previousFolderPath; }
            string selectedPath = AssetDatabase.GetAssetPath(selectedObject);

            if (AssetDatabase.IsValidFolder(selectedPath)) { folderPath = selectedPath; }
            else { folderPath = Path.GetDirectoryName(selectedPath); }

            return folderPath;

        }
        #endregion

        #region Save at Current Location
        private void SaveAtCurrentLocation()
        {

            string folderPath = GetCurrentLocation();
            string tempName = uiUtility.UserInputCheck(ref materialName) + ".mat";
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/" + tempName); ;

            AssetDatabase.CreateAsset(material, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = material;

        }
        #endregion

        #endregion

        /************************ Display Material ************************/
        
        #region Display Texture Field
        private void DisplayTextureField(string label, string textureProperty, ref Texture2D texture)
        {

            texture = EditorGUILayout.ObjectField(label, texture, typeof(Texture2D), false) as Texture2D;
            materialPreviewWindow.previewMaterial.SetTexture(textureProperty, texture);

        }
        #endregion

        #region Display Roughness Section
        private void DisplayRoughnessSection()
        {

            roughnessTextureMap = EditorGUILayout.ObjectField("Roughness Map", roughnessTextureMap, typeof(Texture2D), false) as Texture2D;
            materialPreviewWindow.previewMaterial.SetTexture(MetallicGlossMap, 
                useRoughnessMap ? roughnessTextureMap : metallicTextureMap);
            
            uiUtility.DisplayHelpBox("Having the roughness map checked means that the metallic map will not be used.", MessageType.Info, true);

        }
        #endregion

        #region Display Emission Section
        private void DisplayEmissionSection()
        {

            if (useEmissionMap)
            {
                EnableEmission();
                ConfigureEmission();
            }
            else
            {
                DisableEmission();
                ConfigureEmission();
            }

        }
        #endregion

        #region Emission Configuration
        private void ConfigureEmission()
        {

            DisplayTextureField("Emission Map", EmissionMap, ref emissionTextureMap);
            uiUtility.DisplayColorField(new GUIContent("Emission Color"), ref emissionColor);

            materialPreviewWindow.previewMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
            materialPreviewWindow.previewMaterial.SetColor(EmissionColorProperty, emissionColor);

        }
        #endregion

        #region Enable & Disable Emission

        #region Enable Emission
        private void EnableEmission()
        {

            material.EnableKeyword("_EMISSION");
            materialPreviewWindow.previewMaterial.EnableKeyword("_EMISSION");

        }
        #endregion

        #region Disable Emission
        private void DisableEmission()
        {

            material.DisableKeyword("_EMISSION");
            materialPreviewWindow.previewMaterial.DisableKeyword("_EMISSION");

        }
        #endregion

        #endregion

        #region Display Button
        private void DisplayButton()
        {

            uiUtility.DisplayButton("Save at Current Location",
                () => CreateMaterial(
                    () => SaveAtCurrentLocation()
                    )
                );

            uiUtility.DisplayButton("File Save",
                () => CreateMaterial(
                    () => OpenFolderForMaterialDisplay()
                    )
                );

            uiUtility.DisplayButton("Clear Material Creator",
                () => ClearMaterialCreator()
                );

        }
        #endregion
        
        /************************ Two Display GUI Optopns ************************/
        
        #region Unity Default Material Display
        private void DisplayUnityDefaultMaterialCreator()
        {

            DisplayTextureField("Albedo Map", BaseMap, ref albedoTextureMap);
            SetMaterialPreviewWindow(BaseMap, albedoTextureMap);

            DisplayTextureField("Metallic Map", MetallicGlossMap, ref metallicTextureMap);
            SetMaterialPreviewWindow(MetallicGlossMap, metallicTextureMap);

            uiUtility.DisplayToggleGroup("Use Roughness Map", ref useRoughnessMap, () => DisplayRoughnessSection() );

            uiUtility.DisplayFloatValue("Smoothness: ", ref smoothness, 0f, 1f);
            materialPreviewWindow.previewMaterial.SetFloat(SmoothnessProperty, smoothness);

            DisplayTextureField("Normal Map", NormalMap, ref normalTextureMap);
            NormalMaps(normalTextureMap, TextureImporterType.NormalMap);
            SetMaterialPreviewWindow(NormalMap, normalTextureMap);

            DisplayTextureField("Height Map", HeightMap, ref heightTextureMap);
            SetMaterialPreviewWindow(HeightMap, heightTextureMap);

            DisplayTextureField("Ambient Occlusion", OcclusionMap, ref occlusionTextureMap);
            SetMaterialPreviewWindow(OcclusionMap, occlusionTextureMap);

            uiUtility.DisplayToggleGroup("Use Emissive Map", ref useEmissionMap, () => DisplayEmissionSection() );

            DisplayButton();

        }
        #endregion

        #region Display Custom Shader Material Creator
        private void DisplayCustomShaderMaterialCreator()
        {

            DisplayTextureField("Albedo Map", BaseMap, ref albedoTextureMap);
            uiUtility.DisplayColorField(new GUIContent("Base Color"), ref baseColor);

            useMetallic = EditorGUILayout.BeginToggleGroup("Use Metallic Map", useMetallic);
            DisplayTextureField("Metallic Map", MetallicGlossMap, ref metallicTextureMap);
            material.SetFloat(HasMetalllicMap, 1f);
            materialPreviewWindow.previewMaterial.SetFloat(HasMetalllicMap, 1f);
            EditorGUILayout.EndToggleGroup();

            uiUtility.DisplayFloatValue("Metallic: ", ref metallicness, 0f, 1f);
            materialPreviewWindow.previewMaterial.SetFloat(MetallicProperty, metallicness);

            uiUtility.DisplayToggleGroup("Use Roughness Map", 
                ref useRoughnessMap, 
                () => DisplayTextureField("Roughness Map", 
                    RoughnessMap, 
                    ref roughnessTextureMap)
                );

            uiUtility.DisplayFloatValue("Smoothness", ref smoothness, 0f, 1f);
            materialPreviewWindow.previewMaterial.SetFloat(SmoothnessProperty, smoothness);

            DisplayTextureField("Noraml Map", NormalMap, ref normalTextureMap);
            uiUtility.DisplayFloatValue("Normal Strength: ", ref normalStrength, -100, 100);
            materialPreviewWindow.previewMaterial.SetFloat(NormalMapStrength, normalStrength);

            useHeightMap = EditorGUILayout.BeginToggleGroup("Has Height Map", useHeightMap);
            DisplayTextureField("Height Map", HeightMap, ref heightTextureMap);
            int turnOnHeightMap = useHeightMap ? 1 : 0;
            material.SetFloat(HasHeightMap, turnOnHeightMap);
            uiUtility.DisplayFloatValue("Height Strenght: ", ref heightStrength, 0.005f, 0.08f);
            materialPreviewWindow.previewMaterial.SetFloat(HeightStrength, heightStrength);

            EditorGUILayout.EndToggleGroup();

            DisplayTextureField("Ambient Occlusion", OcclusionMap, ref occlusionTextureMap);

            uiUtility.DisplayToggleGroup("Has Opacity Map", 
                ref hasOpacityMap, 
                () => DisplayTextureField("Opacity", AlphaMap, ref alpha)
                );
            materialPreviewWindow.previewMaterial.SetFloat(HasOpacity, 1f);

            uiUtility.DisplayToggleGroup("Emission Property", 
                ref useEmissionMap, 
                () => DisplayEmissionSection()
                );
            materialPreviewWindow.previewMaterial.SetTexture(EmissionMap, emissionTextureMap);
            materialPreviewWindow.previewMaterial.SetColor(EmissionColorProperty, emissionColor);
            
            DisplayButton();

        }
        #endregion
        
        /************************ Material Creator GUI ************************/
        
        #region Display Material Creator GUI
        public void DisplayMaterialCreatorGUI(EditorWindow editorWindow)
        {

            materialName = EditorGUILayout.TextField("Material Name", materialName, uiUtility.textFieldStyle);
            materialName = uiUtility.UserInputCheck(ref materialName);

            type = (MaterialType)EditorGUILayout.EnumPopup("Filter type: ", type);
            MaterialState(type);

            if (type != MaterialType.Object_Shader && type != MaterialType.Transparent_Object_Shader)
            { DisplayUnityDefaultMaterialCreator(); }
            else
            { DisplayCustomShaderMaterialCreator(); }

            editorWindow.Repaint();

        }
        #endregion
        
    }

}
#endif