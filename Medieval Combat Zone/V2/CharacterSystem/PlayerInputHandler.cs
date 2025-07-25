using UnityEngine;

namespace CombatZone
{
    public class PlayerInputHandler : MonoBehaviour
    {

        public PlayerControls controls { get; private set; }

        void Awake()
        {
            controls = new PlayerControls();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

    }
}