using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace CombatZone.Testing
{

    public class SpawnPlayerSetupMenu : MonoBehaviour
    {

        public GameObject playerSetupMenuPrefab;

        public PlayerInput input;

        private void Awake()
        {

            var rootMenu = GameObject.Find("MainLayout");

            if (rootMenu != null)
            {

                var menu = Instantiate(playerSetupMenuPrefab, rootMenu.transform);

                input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();

                menu.GetComponent<PlayerSetupController>().SetPlayerIndex(input.playerIndex);

            }

        }

    }

}