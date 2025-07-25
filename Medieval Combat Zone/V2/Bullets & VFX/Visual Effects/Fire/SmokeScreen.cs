using UnityEngine;

namespace CombatZone.VisualEffects
{
    public class SmokeScreen : BaseEffects
    {

        [SerializeField] private float destroyDelay = 5f; 

        private void Start()
        {
            PlayAudioOnce();
            Destroy(gameObject, destroyDelay);
        }

    }
}