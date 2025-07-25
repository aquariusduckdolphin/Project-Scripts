using System.Linq;
using UnityEngine;

namespace CombatZone.ClothSim
{
    public class SetClothColliders : MonoBehaviour
    {

        private Cloth cloth;
        [SerializeField] private int numberOfColliderToSkip;
        [SerializeField] private CapsuleCollider[] capsuleColliders;

        void Start()
        {
            cloth = GetComponent<Cloth>();
            if(cloth == null) { return; }
            capsuleColliders = transform.root.GetComponentsInChildren<CapsuleCollider>();
            if(capsuleColliders.Length <= 0 || numberOfColliderToSkip >= capsuleColliders.Length) { return; }
            capsuleColliders = capsuleColliders.Skip(numberOfColliderToSkip).ToArray();
            cloth.capsuleColliders = capsuleColliders;
        }

    }
}