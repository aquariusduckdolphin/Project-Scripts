using UnityEngine;

namespace CombatZone.Bot
{

    public class AssignCharacterType : MonoBehaviour
    {

        #region Variables

        #region Character Type
        [Header("Character Type")]
        private CharacterType character = CharacterType.None;

        private enum CharacterType { Assassin, Wizard, None }
        #endregion

        #region Character Properties
        [Header("Character Properties")]
        [SerializeField] private GameObject[] characters = new GameObject[2];

        private BotBehavior behavior;
        #endregion

        #endregion

        /***************************************************************/

        #region Start
        void Start()
        {

            behavior = GetComponent<BotBehavior>();

            TurnOffCharacterMeshes();

            ChooseRandomCharacter();

            SetCharacterComponents();

        }
        #endregion

        #region Turn Off Character Meshes
        private void TurnOffCharacterMeshes()
        {

            for (int i = 0; i < characters.Length; i++)
            {

                characters[i].SetActive(false);

            }

        }
        #endregion

        #region Choose Random Character
        private void ChooseRandomCharacter()
        {

            int skinType = Random.Range(0, characters.Length);

            CharacterType choosingCharacater = (CharacterType)skinType;

            character = choosingCharacater;

            characters[skinType].SetActive(true);

        }
        #endregion

        /***************************************************************/

        #region Set Character Components
        private void SetCharacterComponents()
        {

            for (int i = 0; i < characters.Length; i++)
            {

                behavior.anim = characters[i].GetComponent<Animator>();

                behavior.health = characters[i].GetComponent<Health>();

            }

        }
        #endregion

    }

}

