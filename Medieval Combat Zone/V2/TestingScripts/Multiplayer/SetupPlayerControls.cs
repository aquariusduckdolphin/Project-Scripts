using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatZone.Testing
{

    public class SetupPlayerControls : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private GameObject readyPanel;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button readyButton;

        private float ignoreInputTime = 1.5f;
        private bool inputEnabled;
        public bool chosen = false;

        private void Start()
        {
            PlayerInfo._instance.control.Add(this);
        }

        void Update()
        {
            if (Time.time > ignoreInputTime) { inputEnabled = true; }
        }

        public void LockIn()
        {
            if (!inputEnabled) { return; }
            readyPanel.SetActive(true);
            readyButton.Select();
            menuPanel.SetActive(false);
        }

        public void ReadyPlayer()
        {
            if (!inputEnabled) { return; }
            readyButton.gameObject.SetActive(false);
        }

        public void SelectCharacter()
        {
            chosen = true;
            PlayerInfo._instance.playerCount += 1;
        }

    }

}