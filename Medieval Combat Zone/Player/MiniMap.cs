using UnityEngine;

namespace CombatZone.Player
{

    public class MiniMap : MonoBehaviour
    {

        public Transform player;

        private void LateUpdate()
        {

            Vector3 newPostion = player.position;

            newPostion.y = transform.position.y;

            transform.position = newPostion;

            //Camera rotate with the player
            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);

        }

    }

}