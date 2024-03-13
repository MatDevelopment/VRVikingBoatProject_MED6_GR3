using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotLookingAtReset : MonoBehaviour
{
    [SerializeField] private Transform resetPosition;
    [SerializeField] private Collider playerZone;

    private bool objectOutsideZone;

    private void Start()
    {
        /*int ChoppedWooodLayer = LayerMask.NameToLayer("ChoppedWood");
        if (gameObject.layer == ChoppedWooodLayer)
        {
            resetPosition = GameObject.Find("WoodResetPosition").transform;
            playerZone = GameObject.Find("Outside Zone Trigger").GetComponent<Collider>();
        }*/
    }

    private void OnBecameInvisible()
    {
        if (objectOutsideZone == true)
        {
            gameObject.transform.position = resetPosition.position;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerZone"))
        {
            objectOutsideZone = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerZone"))
        {
            objectOutsideZone = true;
        }
    }
}
