using CombatZone.Bullet;
using UnityEngine;

namespace CombatZone.VisualEffects
{

    public class WaterShield : BaseEffects
    {

        #region Update
        void Update()
        {
            DestructionGrowthEffect();
        }
        #endregion

        #region On Trigger Enter
        private void OnTriggerEnter(Collider other)
        {          
            BaseBullet bullet = other.GetComponent<BaseBullet>();
            Destroy(bullet?.gameObject);
        }
        #endregion

    }

}