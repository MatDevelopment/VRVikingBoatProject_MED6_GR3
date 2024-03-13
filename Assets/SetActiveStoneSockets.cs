using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Object = UnityEngine.Object;

//XRInteractionGroup socketGroup = gameObject.GetComponent<XRInteractionGroup>();

public class SetActiveStoneSockets : MonoBehaviour
{
    private List<Object> socketGroup;
    
    private void Awake()
    {
        socketGroup = gameObject.GetComponent<XRInteractionGroup>().startingGroupMembers;
    }

    private void Start()
    {
        foreach (var socket in socketGroup)
        {
            socket.GetComponent<XRSocketInteractor>().socketActive = false;
        }
    }

    public void SetStoneSocketsToActive()
    {
        foreach (var socket in socketGroup)
        {
            socket.GetComponent<XRSocketInteractor>().socketActive = true;
        }
    }
    
    private IEnumerator SetStoneSocketsToDisabled()
    {
        foreach (var socket in socketGroup)
        {
            yield return new WaitForSeconds(0.01f);
            socket.GetComponent<XRSocketInteractor>().socketActive = false;
        }
    }

    public void StartCoroutine_SetStoneSocketsToDisabled()
    {
        SetStoneSocketsToDisabled();
    }

}
