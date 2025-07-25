using UnityEngine;
using UnityEngine.UI;

namespace CombatZone.Testing
{

    public class SelectCharacter : MonoBehaviour
    {
        private Button button;
        private float lastClickTime;
        private float doubleClickThreshold = 0.3f;

        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private GameObject playerSelect;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            if (Time.time - lastClickTime < doubleClickThreshold)
            {
                SpawnCharacter(playerSelect);
            }
            lastClickTime = Time.time;
        }

        public void SpawnCharacter(GameObject character)
        {
            Instantiate(character, spawnPoint, Quaternion.identity);
            playerSelect.SetActive(false);
        }

    }

}