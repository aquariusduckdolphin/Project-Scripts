using UnityEngine;

namespace CombatZone.Character.Player
{

    public class MoveCamera : MonoBehaviour
    {

        [SerializeField] private Transform cameraPosition;

        private void Update()
        {
            transform.position = cameraPosition.position;
        }

    }

}