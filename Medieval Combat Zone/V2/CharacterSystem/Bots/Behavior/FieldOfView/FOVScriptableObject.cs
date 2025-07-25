using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace CombatZone.Character.Bot
{

    [CreateAssetMenu(fileName = "FOV", menuName = "Bots/FOV")]
    public class FOVScriptableObject : ScriptableObject
    {

        [Header("FOV")]
        public float viewRadius;
        public float viewAngle;

        public Color teamColor = Color.black;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

    }

}