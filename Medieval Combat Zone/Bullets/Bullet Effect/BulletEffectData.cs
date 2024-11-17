using UnityEngine;

namespace CombatZone.Bullet
{

    [CreateAssetMenu(fileName = "BulletData", menuName = "BulletData")]
    public class BulletEffectData : ScriptableObject
    {

        [Header("Bullet Properties")]
        public float bulletDamage = 10f;

        public float destroyAfterSeconds = 5f;

        [Header("Bullet Configuration")]
        public GameObject[] bulletPrefabs = new GameObject[4];

        [Header("Impact Effect Variations")]
        public GameObject[] impactEffects = new GameObject[16];

    }

}
