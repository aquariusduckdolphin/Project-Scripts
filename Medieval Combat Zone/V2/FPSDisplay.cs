using TMPro;
using UnityEngine;

namespace CombatZone.Settings
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private float pollingTime = 1f;
        private float time = 1f;
        private int frameCount;

        void Update()
        {
            time += Time.deltaTime;
            frameCount++;
            if(time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                text.text = frameCount.ToString() + " FPS";
                time -= pollingTime;
                frameCount = 0;
            }
        }
    }
}