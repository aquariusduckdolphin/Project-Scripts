using UnityEngine;

namespace CombatZone.Character.Abilities
{

    public class AssassinOpeningArea : MonoBehaviour
    {

        public AssassinAbilities assassin;

        private void OnTriggerEnter(Collider other)
        {

            assassin = other.transform.transform.GetComponent<AssassinAbilities>();

            if (assassin != null)
            {
                assassin.canLockPick = true;
                assassin.colliders = gameObject.GetComponent<BoxCollider>();
            }

        }

        private void OnTriggerExit(Collider other)
        {

            assassin = other.transform.transform.GetComponent<AssassinAbilities>();

            if (assassin != null)
            {
                assassin.canLockPick = false; ;
                assassin.colliders = null;
            }

        }

    }

}