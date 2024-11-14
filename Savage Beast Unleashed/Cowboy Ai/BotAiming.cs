using System;
using System.Collections;
using System.Collections.Generic;
using TetraCreations.Attributes;
using UnityEngine;
using UnityEngine.AI;

public class BotAiming : MonoBehaviour
{

    #region References
    [Title("References", TitleColor.Aqua, TitleColor.Orange)]
    private NavMeshAgent agent;

    public Animator anim;

    private Transform player;
    #endregion

    //Remove
    private PlayerHealth health;

    #region Bot References
    [Title("Bot References", TitleColor.Aqua, TitleColor.Orange)]

    [Tooltip("Indactes if it has the player.")]
    public Transform aimTarget;

    [Tooltip("This is the head of the bot.")]
    public Transform headAimTarget;

    [Tooltip("Same transform as the head aim target.")]
    public Transform eyeLocation;

    [Tooltip("This is the vertial aim on the bot. This is the parent of the gun.")]
    public Transform verticalAim;

    [Tooltip("The player's center area.")]
    public Transform aimTargetChest;
    #endregion

    //Tells us if it is visible to the ai
    public bool targetVisible = false;

    #region Raycast
    [Title("Raycast", TitleColor.Aqua, TitleColor.Orange)]
    public LayerMask ignoreVisbility;

    private RaycastHit visibleObject;
    #endregion

    #region Bot Aiming
    [Title("Bot Aiming", TitleColor.Aqua, TitleColor.Orange)] 
    public float rotationalSpeed = 1f;

    private Vector3 aimDirection;

    private Vector3 verticalAimDirection;

    private Vector3 aimDirectionNoY;

    private Vector3 verticalAimDirectionNoY;

    private Vector3 aimDirectionYOnly;

    private Vector3 cancelY = new Vector3(1f, 0f, 1f);
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        anim = GetComponent<Animator>();

        agent = GetComponentInParent<NavMeshAgent>();

        //SetTarget(player);

        aimTarget = GameObject.FindGameObjectWithTag("Player").transform;

        aimTargetChest = aimTarget;

        //verticalAim = player.transform;

        verticalAim.rotation = aimTarget.rotation;

    }

    // Update is called once per frame
    void Update()
    {

        headAimTarget = aimTarget.transform;

        //aimTargetChest = player.transform;

        

        

        //LookAtAimTarget();

    }

    private void LookAtAimTarget()
    {
        
        if(aimTarget != null)
        {

            if(targetVisible) { aimDirection = aimTargetChest.position - transform.position; }

            aimDirectionNoY = Vector3.Scale(aimDirection, cancelY);

        }

        transform.parent.rotation = Quaternion.LookRotation(
            Vector3.RotateTowards(transform.parent.forward, aimDirectionNoY,
            rotationalSpeed, 0f));

        HeadAim();

        VerticalAim();

    }

    private void VerticalAim()
    {

        if (targetVisible)
        {

            verticalAimDirection = aimTargetChest.position - verticalAim.position;

            verticalAimDirectionNoY = Vector3.Scale(verticalAimDirection,
                                                    cancelY);

            aimDirectionYOnly = new Vector3(
                verticalAimDirectionNoY.magnitude * transform.forward.x,
                verticalAimDirectionNoY.y,
                verticalAimDirectionNoY.magnitude * transform.forward.z);

            verticalAim.rotation = Quaternion.LookRotation(
                Vector3.RotateTowards(
                verticalAim.forward, aimDirection, rotationalSpeed, 0f
                ));

        }

    }

    private void HeadAim()
    {
        
        if (targetVisible) { headAimTarget.position = aimTargetChest.position; }

    }

    bool SetTarget(Transform target)
    {
        
        if(target != null)
        {

            aimTarget = target;

            health = aimTarget.GetComponent<PlayerHealth>();

            //aimTargetChest = health.chest;

            return true;

        }
        else
        {

            return false;

        }

    }

}
