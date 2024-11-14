using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveWhirlWind : MonoBehaviour
{

    private Vector3 position;

    public Vector3 offset;

    public float speed = 10f;

    public float time = 0f;

    public float lifetime = 10f;

    private Transform player;

    private const string orientationGO = "Orientation";

    public Grunt grunt;

    public HumanHealth cowyboy;

    public Vampire vampire;

    public float brutalKill = 40f;

    public GameObject[] whirlWindDeath;

    public Transform spawnLocation;

    public ParticleSystem blood;

    public float damage = 10f;

    void Start()
    {
        
        player = GameObject.Find(orientationGO).transform;

        position = player.forward;

    }

    private void Update()
    {
        
        time += Time.deltaTime;

        if(time >= lifetime)
        {

            Destroy(this.transform.parent.gameObject);

        }

    }

    void FixedUpdate()
    {

        transform.position += position * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {

        //spawnLocation = GameObject.Find("Spawn Location").transform;

        grunt = other.GetComponent<Grunt>();

        cowyboy = other.GetComponent<HumanHealth>();

        //vampire = other.GetComponent<Vampire>();

        if (grunt != null)
        {

            if(grunt.currentHealth <= brutalKill)
            {

                GameObject enemy = Instantiate(whirlWindDeath[0], other.transform.position, other.transform.rotation);

                Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);

                //rotation.x = 0f;

                blood = Instantiate(blood, other.transform.position, rotation);

                Destroy(grunt.gameObject);

                StartCoroutine(TurnOffParticle(lifetime));

                Destroy(transform.parent.gameObject);

                grunt = null;

            }
            else
            {

                grunt.currentHealth -= damage;

                grunt = null;

            }

        }
        else if (cowyboy != null)
        {

            if(cowyboy.currentHealth <= brutalKill)
            {

                GameObject enemy = Instantiate(whirlWindDeath[1], other.transform.position, other.transform.rotation);

                Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);

                //rotation.x = 0f;

                blood = Instantiate(blood, other.transform.position, rotation);

                Destroy(cowyboy.gameObject);

                StartCoroutine(TurnOffParticle(lifetime));

                cowyboy = null;

            }
            else
            {

                cowyboy.currentHealth -= damage; 
                
                cowyboy = null;

            }

        }
        else if(vampire != null)
        {

            if (vampire.currentHealth <= brutalKill)
            {

                GameObject enemy = Instantiate(whirlWindDeath[1], other.transform.position, other.transform.rotation);

                Quaternion rotation = new Quaternion(0f, 0f, 0f, 0f);

                //rotation.x = 0f;

                blood = Instantiate(blood, other.transform.position, rotation);

                Destroy(vampire.gameObject);

                StartCoroutine(TurnOffParticle(lifetime));

                vampire = null;

            }
            else
            {

                vampire.currentHealth -= damage;

                vampire = null;

            }

        }



    }

    IEnumerator TurnOffParticle(float delay)
    {

        yield return new WaitForSeconds(delay);

        Destroy(blood);

        yield return new WaitForSeconds(delay);

        Destroy(transform.parent.gameObject);

    }

}
