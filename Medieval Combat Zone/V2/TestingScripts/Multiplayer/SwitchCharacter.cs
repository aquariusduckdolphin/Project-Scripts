using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CombatZone.Testing
{

    public class SwitchCharacter : MonoBehaviour
    {

        int index = 0;

        [SerializeField] List<GameObject> characters = new List<GameObject>();

        PlayerInputManager manager;

        // Start is called before the first frame update
        void Start()
        {

            manager = GetComponent<PlayerInputManager>();

            index = Random.Range(0, characters.Count);

            manager.playerPrefab = characters[index];

        }

        public void SwitchNextSpawnCharacter(PlayerInput input)
        {

            index = Random.Range(0, characters.Count);

            manager.playerPrefab = characters[index];

        }

    }

}