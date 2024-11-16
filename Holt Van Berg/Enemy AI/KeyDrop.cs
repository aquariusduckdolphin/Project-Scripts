using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDrop : MonoBehaviour
{

    public Zombie zombie;

    public GameObject keyPrefab;

    private float time = 3f;

    public bool key;

    // Start is called before the first frame update
    void Awake()
    {

        zombie = GetComponent<Zombie>();

        key = true;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(zombie.isDead == true && key == true)
        {

            key = false;

            StartCoroutine(DropKey(time));

        }
        
    }

    IEnumerator DropKey(float time)
    {

        yield return new WaitForSeconds(time);

        Instantiate(keyPrefab, zombie.transform.position, zombie.transform.rotation);

    }

}
