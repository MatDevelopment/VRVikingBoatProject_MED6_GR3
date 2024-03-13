using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class handPresencePhysics : MonoBehaviour
{

    public Transform target;
    private Rigidbody rb;

    public Renderer nonPhyiscalHand;

    public float showNonPhysicalHandDistance = 0.10f;

    private Collider[] handColliders;

    private Boolean isGrabbed = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();
        
    }

    public void EnableHandCollider()
    {
        if (!isGrabbed)

        {
            foreach (var item in handColliders)
            {
                item.enabled = true;
            }
        }

    }


    public void EnableHandColliderDelay(float delay)
    {
        Invoke("EnableHandCollider", delay);
        isGrabbed = false;
    }

    public void DisableHandCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = false;
            isGrabbed = true;
        }

    }


    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > showNonPhysicalHandDistance)
        {
            nonPhyiscalHand.enabled = true;
        }
        else
        {
            nonPhyiscalHand.enabled = false;  
        }


    }



    void FixedUpdate()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }
}
