using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatZone.Testing
{

    public class PlayerSetupController : MonoBehaviour
    {

        private int PlayerIndex;

        [SerializeField] private TextMeshProUGUI titleText;

        [SerializeField] private GameObject readyPanel;

        [SerializeField] private GameObject menuPanel;

        [SerializeField] private Button readyButton;

        private float ignoreInputTime = 1.5f;

        private bool inputEnabled;

        public void SetPlayerIndex(int pi)
        {

            PlayerIndex = pi;

            titleText.SetText("Player " + (pi + 1).ToString());

            ignoreInputTime = Time.time + ignoreInputTime;

        }

        // Update is called once per frame
        void Update()
        {

            if (Time.time > ignoreInputTime)
            {

                inputEnabled = true;

            }

        }

        public void SetCharacter(GameObject characters)
        {

            if (!inputEnabled) { return; }

            PlayerConfigurationManager._instance.SetPlayerCharacter(PlayerIndex, characters);

            readyPanel.SetActive(true);

            readyButton.Select();

            menuPanel.SetActive(false);

        }

        public void RedayPlayer()
        {

            if (!inputEnabled) { return; }

            PlayerConfigurationManager._instance.ReadyPlayer(PlayerIndex);

            readyButton.gameObject.SetActive(false);

        }

    }

}