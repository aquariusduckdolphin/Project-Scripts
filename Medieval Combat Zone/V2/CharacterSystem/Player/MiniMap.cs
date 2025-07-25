using UnityEngine;

namespace CombatZone.Character.Player
{

    public class MiniMap : MonoBehaviour
    {

        [SerializeField] private Transform player;

        private void LateUpdate()
        {
            Vector3 newPostion = player.position;
            newPostion.y = transform.position.y;
            transform.position = newPostion;
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }

    }

}