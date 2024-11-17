using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FixAtenna : MonoBehaviour
{

    private const string player = "Player";

    [Header("Repair Reference")]
    public bool repaired;

    public bool collected;

    [SerializeField] private bool turnOn;

    [Header("Collider Reference")]
    private BoxCollider repairAntenna;

    [Header("Animation References")]
    private Animator anim;

    [Header("UI Reference")]
    public GameObject repairText;

    #region Gather Info
    private void Awake()
    {

        repairText = GameObject.FindGameObjectWithTag("Repair Text");

    }
    

    void Start()
    {

        anim = GetComponentInChildren<Animator>();

        repairAntenna = GetComponent<BoxCollider>();

        repairText.SetActive(false);

    }
    #endregion

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.E) && turnOn && !collected)
        {

            repaired = true;

            repairAntenna.enabled = false;

            RepairText(false);

            turnOn = false;
  
        }

        if (repaired)
        {

            anim.SetBool("Under Repaired", true);

            anim.SetBool("Repaired", true);

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if( other.CompareTag(player))
        {

            RepairText(true);

            repairText.SetActive(true);

            turnOn = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag(player))
        {

            RepairText(false);

            turnOn = false;

        }

    }

    void RepairText(bool state)
    {

        repairText.SetActive(state);

    }

}
