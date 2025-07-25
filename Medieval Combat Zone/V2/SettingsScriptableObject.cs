using UnityEngine;

namespace CombatZone
{
    [CreateAssetMenu(menuName = "Settings", fileName = "Settings")]
    public class SettingsScriptableObject : ScriptableObject
    {

        [SerializeField] private bool fpsCounter = false;
        public GameObject fpsGameObject;

        public void FPSCounterVisibility(GameObject visiblity)
        {
            fpsCounter = visiblity;
            fpsGameObject.SetActive(fpsCounter);
        }

    }

}