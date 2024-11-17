using UnityEngine;

namespace CombatZone.Abilities
{

    public class AssassinOpeningArea : MonoBehaviour
    {

        public AssassinAbilities assassin;

        private void OnTriggerEnter(Collider other)
        {

            assassin = other.transform.parent.GetComponent<AssassinAbilities>();

            if (assassin != null)
            {

                print("Can pick the lock");

                assassin.canLockPick = true;

                assassin.colliders = gameObject.GetComponent<BoxCollider>();

            }

        }

        private void OnTriggerExit(Collider other)
        {

            assassin = other.transform.parent.GetComponent<AssassinAbilities>();

            if (assassin != null)
            {

                print("No longer can pick the lock");

                assassin.canLockPick = false; ;

                assassin.colliders = null;

            }

        }

    }

}