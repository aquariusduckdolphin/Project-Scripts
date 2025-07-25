using UnityEngine;

namespace CombatZone.Bullet
{

    public class AirBullet : BaseBullet, IBullet
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
            //InitializeBulletAudio(bulletData, 3);
            PlayLoopingBulletAudio(3, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleImpact(other, 3);
        }

        /**************** Validate ****************/

        private void OnValidate()
        {
            _elementType = IBullet.ElementType.Air;
        }

    }

}