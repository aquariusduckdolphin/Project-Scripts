using UnityEngine;

namespace CombatZone.Character.Player
{

    [CreateAssetMenu(fileName = "Movement Settings", menuName = "Player/Player Movement Settings")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [Header("Movement Speeds")]
        public float defaultWalkSpeed = 7f;
        public float defaultSprintSpeed = 10f;
        public float slideSpeed = 30f;

        [Header("Jump Settings")]
        public float jumpForce = 12f;
        public float jumpCooldownDuration = 0.25f;
        public float airMultiplier = 0.4f;

        [Header("Crouch Settings")]
        public float crouchSpeed = 3.5f;
        public float crouchHeightScale = 0.5f;

        [Header("Player Dimensions")]
        
        [Header("Ground Detection")]
        public float playerHeight = 2f;
        public LayerMask groundLayer;

        [Header("Slope Settings")]
        public float maxSlopeAngle = 40f;
    }

}