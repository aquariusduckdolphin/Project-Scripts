using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGap : MonoBehaviour
{

    public GameObject border;

    public Animator animateGap;

    private bool canMove;

    public ParticleSystem[] metal;

    public float stop;

    void Start()
    {

        border.SetActive(false);

        foreach(ParticleSystem particle in metal)
        {

            particle.Stop();

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            border.SetActive(true);

            canMove = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            border.SetActive(false);

            canMove = false;

        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && canMove)
        {

            animateGap.SetBool("MovetoGap", true);

            StartCoroutine(Particle(metal, stop));

        }
        
    }

    IEnumerator Particle(ParticleSystem[] particles, float time)
    {

        foreach(ParticleSystem particle in particles)
        {

            particle.Play();

        }

        yield return new WaitForSeconds(time);

        foreach(ParticleSystem particle in particles)
        {

            particle.Stop();

        }

    }

}
