#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SceneManagerToolkit.Editor
{

    #region Material Preview Window Creator
    public class MaterialPreviewWindow : MonoBehaviour
    {

        public static void PreviewWindow()
        {

            MaterialPreview.CreateWindow();

        }

    }
    #endregion

    /***************************************************************/

    #region Material Preview Editor Window
    public class MaterialPreview : EditorWindow
    {

        #region Show Window
        public static void CreateWindow()
        {

            var window = GetWindow<MaterialPreview>();

            window.Show();

        }
        #endregion

        /***************************************************************/

        #region Variables
        public Mesh previewMesh;

        public Material previewMaterial;

        private PreviewRenderUtility previewRenderUtility;

        private Vector2 previewDir = new Vector2(120f, -20f);

        private Vector2 dragStartPos;

        private bool isDragging = false;

        private float zoomFactor = 5f;

        private const float zoomSpeed = 0.5f;

        private const float minZoom = 2f;

        private const float maxZoom = 15f;

        private MaterialCreator creator;
        #endregion

        /***************************************************************/

        #region On Enable & Disable
        private void OnEnable()
        {

            creator = ScriptableObject.CreateInstance<MaterialCreator>();

            if (previewRenderUtility == null)
            {

                previewRenderUtility = new PreviewRenderUtility();

                previewRenderUtility.cameraFieldOfView = 30f;

                if (previewMesh == null) { previewMesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx"); }

            }

            if (previewMaterial == null)
            { previewMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit")); }

        }

        private void OnDisable()
        { previewRenderUtility.Cleanup(); }
        #endregion

        #region OnGUI
        private void OnGUI()
        {

            previewMesh = EditorGUILayout.ObjectField("Model", previewMesh, typeof(Mesh), false) as Mesh;

            Rect previewRect = GUILayoutUtility.GetRect(position.width, position.height);

            if (previewMaterial == null) { return; }
            DrawMaterialPreview(previewRect, previewMaterial);

        }
        #endregion

        #region Display the material preview
        private void DrawMaterialPreview(Rect rect, Material material)
        {
            if (material == null || previewMesh == null)
                return;

            HandleMouseInput(rect);

            // Set up the camera and lighting
            previewRenderUtility.BeginPreview(rect, GUIStyle.none);

            // Adjust the camera to fit the mesh
            AdjustCameraToFitMesh();

            previewRenderUtility.camera.transform.rotation = Quaternion.identity;

            previewRenderUtility.lights[0].intensity = 1.4f;
            previewRenderUtility.lights[0].transform.rotation = Quaternion.Euler(50f, 50f, 0);
            previewRenderUtility.lights[1].intensity = 1.4f;

            Quaternion rotation = Quaternion.Euler(previewDir.y, previewDir.x, 0);
            previewRenderUtility.DrawMesh(previewMesh, Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one * zoomFactor), material, 0);
            previewRenderUtility.camera.Render();

            Texture previewTexture = previewRenderUtility.EndPreview();
            GUI.DrawTexture(rect, previewTexture, ScaleMode.StretchToFill, false);
        }
        #endregion

        #region Camera Adjustment
        private void AdjustCameraToFitMesh()
        {
            if (previewMesh == null) return;

            // Calculate the bounds of the mesh
            Bounds meshBounds = previewMesh.bounds;
            Vector3 meshSize = meshBounds.size;
            float maxMeshSize = Mathf.Max(meshSize.x, Mathf.Max(meshSize.y, meshSize.z));

            // Calculate the distance from the object so it fits within the camera view
            float distance = maxMeshSize * 15f; // Adjust multiplier as needed

            // Set the camera position and parameters
            previewRenderUtility.camera.transform.position = new Vector3(0, 0, -distance);
            previewRenderUtility.camera.transform.LookAt(Vector3.zero);

            // Adjust the camera's field of view based on the size of the mesh
            float fov = Mathf.Lerp(30f, 90f, maxMeshSize / 5f); // Adjust range as needed
            previewRenderUtility.camera.fieldOfView = Mathf.Clamp(fov, 30f, 90f);

            // Optional: Adjust the near and far clip planes to fit the distance
            previewRenderUtility.camera.nearClipPlane = 0.1f;
            previewRenderUtility.camera.farClipPlane = distance * 2f;
        }
        #endregion

        #region Mouse Input
        private void HandleMouseInput(Rect rect)
        {

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown && rect.Contains(currentEvent.mousePosition))
            {

                isDragging = true;

                dragStartPos = currentEvent.mousePosition;

                currentEvent.Use();

            }
            else if (currentEvent.type == EventType.MouseDrag && isDragging)
            {

                Vector2 dragCurrentPos = currentEvent.mousePosition;

                Vector2 delta = dragCurrentPos - dragStartPos;

                dragStartPos = dragCurrentPos;

                previewDir.x -= delta.x;

                previewDir.y += delta.y;

                currentEvent.Use();

                Repaint();

            }
            else if (currentEvent.type == EventType.MouseUp && isDragging)
            {

                isDragging = false;

                currentEvent.Use();

            }

            if (currentEvent.type == EventType.ScrollWheel && rect.Contains(currentEvent.mousePosition))
            {

                float scrollDelta = currentEvent.delta.y * zoomSpeed;

                zoomFactor = Mathf.Clamp(zoomFactor + scrollDelta, minZoom, maxZoom);

                currentEvent.Use();

                Repaint();

            }

        }
        #endregion

    }
    #endregion

}
#endif