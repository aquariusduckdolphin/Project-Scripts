using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashRaycast : MonoBehaviour
{

    public GameObject[] enemyChange;

    public Transform spawnLocation;

    public float delay = 10f;

    #region Raycast Info
    [Header("Raycast Info")]
    public float raycastLength = 20f;

    private RaycastHit hit;

    private bool dectectedEnemy;

    public GameObject centerPoint;

    public bool human = true;
    #endregion

    #region Enemy References
    [Header("Enemies")]
    public Grunt grunt;

    public HumanHealth cowboy;

    public Vampire vamp;

    public float brutalKillMinHealth = 40f;
    #endregion

    #region Player Reference
    [Header("Player Reference")]
    public PlayerMovement currentlyDashing;

    public AbilitiesScript abilities;
    #endregion

    private void Start()
    {

        abilities = GetComponentInParent<AbilitiesScript>();

    }

    private void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 20f, Color.cyan);

        dectectedEnemy = Physics.Raycast(transform.position, transform.forward * raycastLength, out hit, 20f);

        if (dectectedEnemy)
        {

            grunt = hit.transform.GetComponent<Grunt>();

            vamp = hit.transform.GetComponent<Vampire>();

            cowboy = hit.transform.GetComponent<HumanHealth>();

            if (grunt != null)
            {

                if (grunt.currentHealth <= 40f && currentlyDashing.dashing)
                {

                    abilities.killing = true;

                    GameObject dashKill = Instantiate(enemyChange[0], spawnLocation.position, hit.transform.rotation);

                    Destroy(grunt.gameObject);

                    StartCoroutine(DashKill(delay));

                }

            }
            else if (cowboy != null)
            {

                if (cowboy.currentHealth <= 40f && currentlyDashing.dashing)
                {

                    abilities.killing = true;

                    GameObject dashKill = Instantiate(enemyChange[1], spawnLocation.position, hit.transform.rotation);

                    Destroy(cowboy.gameObject);

                    StartCoroutine(DashKill(delay));

                }

            }
            else if (vamp != null)
            {

                if (vamp.currentHealth <= 40f && currentlyDashing.dashing)
                {

                    abilities.killing = true;

                    GameObject dashKill = Instantiate(enemyChange[2], spawnLocation.position, hit.transform.rotation);

                    Destroy(vamp.gameObject);

                    StartCoroutine(DashKill(delay));

                }

            }

        }

    }

    #region Detecting Enemies
    IEnumerator DashKill(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);

        abilities.killing = false;

    }
    #endregion

    #region Check for Insta Kill
    /// <summary>
    /// This will check enemies and kill them.
    /// </summary>
    /// <param name="enemyHealth"> The health of an enemy. </param>
    /// <param name="kill"> The health that will insta kill. </param>
    /// <param name="player"> The player input for dashing. </param>
    void CheckForDeathKill(float enemyHealth, float kill, PlayerMovement player)
    {

        if(enemyHealth <= kill && player.dashing)
        {

            enemyHealth = 0f;

            //Play Animation Kill

        }

    }
    #endregion

}
