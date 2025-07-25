using UnityEngine;

namespace CombatZone
{
    public class CharacterSelection : MonoBehaviour
    {

        [SerializeField] private GameObject[] characters;
        [SerializeField] private int characterIndex = 0;

        void Start()
        {
            foreach(GameObject go in characters)
            {
                go.SetActive(false);
            }
            characters[characterIndex].SetActive(true);
        }

        public void RightArrow()
        {
            characters[characterIndex].SetActive(false);
            characterIndex++;
            if (characterIndex >= characters.Length) { characterIndex = 0; }
            characters[characterIndex].SetActive(true);
        }

        public void LeftArrow()
        {
            characters[characterIndex].SetActive(false);
            characterIndex--;
            if(characterIndex < 0) { characterIndex = characters.Length - 1; }
            characters[characterIndex].SetActive(true);
        }

        public void ConfirmCharacter()
        {
            print(characters[characterIndex].name);
        }

    }
}
