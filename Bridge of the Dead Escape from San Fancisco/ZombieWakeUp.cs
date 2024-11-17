using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieWakeUp : MonoBehaviour
{

    public float time;

    public float delayBorder;

    public float delay = 1f;

    public int numberOfUse = 1;

    public bool chase = false;

    public bool useWater = false;

    [Header("GameObjects")]
    public GameObject zombies;

    public GameObject Tsunamiwave;

    public GameObject waterBorder;

    public GameObject busZombies;

    public GameObject police;

    public GameObject scout;

    public GameObject femaleZombie;

    [Header("Animators")]
    public Animator wave;

    public Animator[] zombiesMove;

    private void Awake()
    {

        //zombiesMove = GameObject.FindGameObjectWithTag("Zombie").GetComponent<Animator>();

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.transform.tag == "Player")
        {

            foreach(Animator anim in zombiesMove)
            {

                anim.SetBool("GetUp", true);

            }

            chase = true;

            StartCoroutine(WaterBending(useWater, delayBorder, waterBorder));

            StartCoroutine(SpawnzZombies(police, busZombies.transform, delay));

            StartCoroutine(SpawnzZombies(scout, busZombies.transform, delay));

            StartCoroutine(SpawnzZombies(femaleZombie, busZombies.transform, delay));

        }

    }

    IEnumerator SpawnzZombies(GameObject zombie, Transform spawnPoint, float delay)
    {

        for(int i = 0; i < 5; i++)
        {

            Instantiate(zombie, spawnPoint.position, spawnPoint.rotation);

            yield return new WaitForSeconds(delay);

        }

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && chase && useWater)
        {

            UseAbility(waterBorder, numberOfUse);

            StartCoroutine(KillZombies(time));

        }
        
    }

    IEnumerator KillZombies(float time)
    {

        yield return new WaitForSeconds(time);

        zombies.SetActive(false);

        GameObject[] zombie = GameObject.FindGameObjectsWithTag("Zombie");

        for(int i = 0; i < zombie.Length; i++)
        {

            Destroy(zombie[i]);

        }

        yield return new WaitForSeconds(time - 0.5f);

        Destroy(gameObject);

    }

    void UseAbility(GameObject border, int num)
    {

        if(num > 0)
        {

            border.SetActive(false);

            num -= 1;

            wave.SetBool("MoveWave", true);

        }

    }

    IEnumerator WaterBending(bool canBend, float time, GameObject border)
    {

        yield return new WaitForSeconds(time);

        useWater = true;

        border.SetActive(true);

    }

}
