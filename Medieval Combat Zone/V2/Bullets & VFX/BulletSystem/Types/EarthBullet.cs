using UnityEngine;

namespace CombatZone.Bullet
{

    public class EarthBullet : BaseBullet, IBullet
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
            //PlayLoopingBulletAudio(2, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleImpact(other, 2);
        }

        /**************** Validate ****************/

        private void OnValidate()
        {
            _elementType = IBullet.ElementType.Earth;
        }

    }

}