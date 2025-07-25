using UnityEngine;

namespace CombatZone.Player
{

    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;

        private void Update()
        {
            transform.position = cameraPosition.position;
        }

    }

}