using UnityEngine;

namespace CombatZone.Bot
{

    [CreateAssetMenu(fileName = "BotData", menuName = "Bots/Data")]
    public class BotData : ScriptableObject
    {

        #region Properties
        [Header("Properties")]
        public float rotationSpeed = 10f;

        public float agentSpeed = 100f;

        public float agentStopDist = 2f;
        #endregion
         
        /***************************************************************/

        #region Shooting Properties
        [Header("Static Shooting Properties")]
        public GameObject[] bulletPrefab = new GameObject[4];
        
        public float timeBetweenShots = 0.25f;

        public float currentDelayShot;

        public float reloadTime = 2f;

        public float bulletForce = 100f;

        public int magazineSize = 1;

        public Vector3 shootOffset;
        #endregion

        /***************************************************************/

    }

}
