using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{

    private Rigidbody[] rigidbodies;

    private Collider[] colliders;

    private Animator animator;

    [Tooltip("Read-Only. This will indate if the ragdoll is on or off.")]
    public bool ragdollState;

    [Tooltip("Set this to true of there is a collider on the parent.")]
    public bool removeFirstCollider = false;

    #region Gather Info
    void Start()
    {

        rigidbodies = GetComponentsInChildren<Rigidbody>();

        colliders = GetComponentsInChildren<Collider>();

        animator = GetComponentInChildren<Animator>();

        //Set the ragdoll to be off
        DeactivateRagdooll();

    }
    #endregion

    #region Deactive Ragdoll State
    public void DeactivateRagdooll()
    {

        ragdollState = true;

        foreach(var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = ragdollState;

        }

        foreach(var collider in colliders)
        {

            collider.enabled = !ragdollState;

        }
        
        colliders[0].enabled = removeFirstCollider;

        animator.enabled = ragdollState;

    }
    #endregion

    #region Active Ragdoll State
    public void ActivateRagdoll()
    {

        ragdollState = false;

        foreach (var rigidbody in rigidbodies)
        {

            rigidbody.isKinematic =  ragdollState;

        }

        foreach(var collider in colliders)
        {

            if(collider != null)
            {

                collider.enabled = !ragdollState;

            }

        }

        colliders[0].enabled = !removeFirstCollider;

        animator.enabled = ragdollState;

    }
    #endregion

}
