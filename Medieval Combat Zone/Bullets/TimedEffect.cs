using UnityEngine;

public class TimedEffect : MonoBehaviour
{

    [SerializeField] private float effectDuration = 10f;

    void Update()
    {

        if (effectDuration > 0) { effectDuration -= Time.deltaTime; }

        else if (effectDuration <= 0f) { Destroy(gameObject); }

    }

}
