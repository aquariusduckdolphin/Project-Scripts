using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTie : MonoBehaviour
{

    public TieFighterExplosion tieFighter;

    public Animator[] animate;

    public float time;

    private void OnTriggerEnter(Collider other)
    {
        
        StartCoroutine(FlyOut());

    }

    IEnumerator FlyOut()
    {

        animate[0].SetBool("Idle to Idle", true);

        yield return new WaitForSeconds(time);

        foreach(Animator animation in animate)
        {

            animation.SetBool("Fly", true);

            yield return new WaitForSeconds(time);

        }

    }

}
