using UnityEngine;

namespace CombatZone.Bullet
{

    public class WaterBullet : BaseBullet, IBullet
    {

        private IBullet.ElementType _elementType;

        public IBullet.ElementType elementType
        {
            get => _elementType;
            private set => _elementType = value;
        }

        /**************** Start, Update, Etc. ****************/

        private void Start()
        {
            PlayLoopingBulletAudio(1, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleImpact(other, 1);
        }

        /**************** Validate ****************/

        private void OnValidate()
        {
            _elementType = IBullet.ElementType.Water;
        }

    }

}