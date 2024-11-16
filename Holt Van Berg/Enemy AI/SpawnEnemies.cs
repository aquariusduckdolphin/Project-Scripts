using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    //An array to store the enemies
    public GameObject[] enemies;

    //A number for the enemy
    [Range(0, 5)]
    public int enemyType = 0;

    //A number to spawn in that many enemies
    [Range(0, 100)]
    public int enemyCount = 30;

    //Store the player's transform componenet
    public Transform player;

    public float initialDelay = 0.1f;

    public float delay = 10f;

    public float range = 100f;

    public int count;

    public Vector3 spawnLocation;

    private void Awake()
    {

        spawnLocation = transform.position;

    }

    // Start is called before the first frame update
    void Start()
    {

        //Store the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spawnLocation = transform.position; 

        StartCoroutine(SpawnDelay(initialDelay));
        
    }

    // Update is called once per frame
    void Update()
    {

        //Check if the enemy type is greater than the arrays length
        if(enemyType >= enemies.Length)
        {

            //Set the number bak to zero
            enemyType = 0;

        }

    }

    IEnumerator SpawnDelay(float timeDelay)
    {

        yield return new WaitForSeconds(timeDelay);

        SpawnCreatures(enemies[enemyType], enemyCount);

        StartCoroutine(SpawnDelay(delay));

    }

    //Custom Funtion - spawn in the enemies
    void SpawnCreatures(GameObject creature, int number)
    {

        if(count <= 50)
        {

            Vector3 spawnLocation = transform.position;

            //Loop for the amount of need enemies
            for (int i = 0; i < number; i++)
            {

                //Store a generated value
                float xRange = Random.Range(-range, range);

                xRange = xRange + spawnLocation.x;

                //Store a generated value
                float zRange = Random.Range(-range, range);

                zRange = zRange + spawnLocation.z;

                if (enemyType == 2)
                {

                    //Store a generated value
                    float yRange = Random.Range(-range, range);

                    yRange = yRange + spawnLocation.y;

                    //Store the bots location to spawn at
                    spawnLocation = new Vector3(xRange, yRange, zRange);
                }

                //Store the bots location to spawn at
                spawnLocation = new Vector3(xRange, 0, zRange);

                //Generate the ai at the location facing towards the player
                Instantiate(creature, spawnLocation, player.rotation);

                count++;

            }

        }

    }

}
