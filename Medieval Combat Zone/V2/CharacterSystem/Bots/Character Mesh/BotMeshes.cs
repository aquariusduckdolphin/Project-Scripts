using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

namespace CombatZone.Character.Bot
{

    public enum AttackType { None, Melee, Shooting }

    [CreateAssetMenu(fileName = "Bot Meshes", menuName = "Bots/Mesh")]
    public class BotMeshes : ScriptableObject 
    {

        [System.Serializable]
        public struct Characters
        {
            [Tooltip("This is just to find the character easier in the inspector.")]
            public string characterName;
            public GameObject characterMesh;
            public RuntimeAnimatorController characterAnimationController;
            public AttackType attackType;
            [Tooltip("x = capsuale radious, y = capsule hieght")]
            public Vector2 capsuleSizes;
        }

        public Characters[] characters;

    }

}

