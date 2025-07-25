using CombatZone.Ragdolling;
using System.Collections.Generic;
using UnityEngine;

namespace CombatZone.Character.Bot
{

    public class AssignCharacterType : MonoBehaviour
    {

        [SerializeField] private BotBlackboard blackboard;
        [SerializeField] private BotMeshes meshes;

        private CapsuleCollider capsuleCollider;

        [SerializeField] private int skinType;
        [SerializeField] private List<int> currentSkins;

        /**************** Skin Mesh Methods ****************/

        private void Gather()
        {
            blackboard = GetComponent<BotBlackboard>();
            capsuleCollider = blackboard.CapusleCollider.transform.GetComponent<CapsuleCollider>();
        }

        #region Choose Random Character
        public void ChooseRandomCharacter()
        {
            skinType = Random.Range(0, meshes.characters.Length);
            SpawnNewSkin();
        }
        #endregion

        #region Spawn New Skin
        private void SpawnNewSkin()
        {
            Gather(); 
            currentSkins.Add(skinType);
            Vector3 spawn = capsuleCollider.transform.position - new Vector3(0f, 1f, 0f);
            Instantiate(meshes.characters[skinType].characterMesh, spawn, Quaternion.identity, capsuleCollider.transform);
            AttackTypeState(meshes.characters[skinType].attackType);
            SetParameter(skinType);

            blackboard.RagdollController.GatherComponents();
            blackboard.RagdollController.ToggleRagdoll(false);
        }
        #endregion

        #region Set Parameters
        private void SetParameter(int value)
        {
            //animator.runtimeAnimatorController = meshes.characterAnimatorController[value];
            //blackboard.CharacterAnimator.runtimeAnimatorController = meshes.characters[value].characterAnimationController;
            //capsuleCollider.radius = meshes.capsualeSize[value].x;
            capsuleCollider.radius = meshes.characters[value].capsuleSizes.x;
            //capsuleCollider.height = meshes.capsualeSize[value].y;
            capsuleCollider.height =meshes.characters[value].capsuleSizes.y;
        }
        #endregion

        #region Set New Character Skin
        public void SetNewCharacterSkin()
        {
            Skin(false);
            skinType = Random.Range(0, meshes.characters.Length);
            //skinType = Random.Range(0, meshes.characters.Length);
            Skin();
            SpawnNewSkin();
        }
        #endregion

        #region Skin
        private void Skin(bool enabled = true)
        {
            for (int i = 0; i < currentSkins.Count; i++)
            {
                if (currentSkins[i] == skinType)
                {
                    Transform go = transform.GetChild(0).gameObject.transform;
                    go.GetChild(currentSkins[i]).gameObject.SetActive(enabled);
                    SetParameter(skinType);
                    return;
                }
            }
        }
        #endregion

        #region Attack Type State
        private void AttackTypeState(AttackType type)
        {

            if(blackboard.BotBehavior == null)
            {
                Debug.LogWarning("Bot Behavior is not set");
                return;
            }

            switch (type)
            {
                case AttackType.Melee:
                    blackboard.BotBehavior.isMeleeBot = true;
                    break;

                case AttackType.Shooting:
                    blackboard.BotBehavior.isMeleeBot = false;
                    break;

                case AttackType.None:
                    Debug.LogWarning("No AI character attack type set");
                    break;
            }
        }
        #endregion

    }

}