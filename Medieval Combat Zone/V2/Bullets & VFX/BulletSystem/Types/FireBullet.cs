using UnityEngine;

namespace CombatZone.Bullet
{

    public class FireBullet : BaseBullet, IBullet
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
            InitializeBulletAudio(0);
        }

        private void OnTriggerEnter(Collider other)
        {
            HandleImpact(other, 0);
        }

        /**************** Valdiate ****************/

        private void OnValidate()
        {
            _elementType = IBullet.ElementType.Fire;
        }

    }

}