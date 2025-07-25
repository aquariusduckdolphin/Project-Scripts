using UnityEngine;

namespace CombatZone.VisualEffects
{

    public class TestingExplosive : BaseEffects
    {

        public float force = 5f;
        public Rigidbody[] rb;

        void Start()
        {
            rb = GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rigid in rb)
            {

                Vector3 randomDirection = Random.onUnitSphere * force;
                randomDirection.y = 0f;
                rigid.AddForce(randomDirection);
            }
        }

        private void Update()
        {
            DestructionTimer(ref remainingTime, growthDuration, gameObject);
        }

    }

}