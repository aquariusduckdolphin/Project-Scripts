using UnityEngine;

namespace CombatZone.Character.Player
{
    [System.Serializable]
    public class TextureSound
    {
        public Texture Albedo;
        public AudioClip[] Clips;
    }

    [CreateAssetMenu(fileName = "Foot Sound", menuName = "Player/Foot Sound")]
    public class FootstepSound : ScriptableObject
    {

        /**************** Variables ****************/

        public LayerMask floorLayer;
        public LayerMask houseLayer;
        public TextureSound[] textureSounds;
        public bool blendTerrainSounds;
        public AudioClip roofTopSound;

        /**************** Functions ****************/

        #region Get Clip From Texture
        public AudioClip GetClipFromTextureSound(TextureSound TextureSound)
        {
            int clipIndex = Random.Range(0, TextureSound.Clips.Length);
            return TextureSound.Clips[clipIndex];
        }
        #endregion

    }
}