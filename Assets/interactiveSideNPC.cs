using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using WaterSystem.Data;

public class interactiveSideNPC : MonoBehaviour
{
    [SerializeField] private DynamicGestureManager _dynamicGestureManager;
    
    private bool currentlyLookingAtNpc = false;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _dynamicGestureManager = GameObject.FindWithTag("DynamicGestureManager").GetComponent<DynamicGestureManager>();
    }

    public void StartCoroutine_currentlyLookingAtNpcDuration_Active()
    {
        StopAllCoroutines();
        StartCoroutine(currentlyLookingAtNpcDuration_Active(3f));
    }

    public void doWaveAnimation()
    {
        if (_dynamicGestureManager.justWaved && currentlyLookingAtNpc)      //USE SOMETHING ELSE THAN A STATIC. UNITY IS CRASHING:( Or maybe it is the TestLogs being produced? (Memory leak)
        {
            Debug.Log("*NPC waves back*");      //This is where to play a wave animation on the NPC
        }
        
    }

    /*public void SetFalse_currentlyLookingAtNpc()
    {
        currentlyLookingAtNpc = false;
    }*/
    
    public IEnumerator currentlyLookingAtNpcDuration_Active(float duration)
    {
        currentlyLookingAtNpc = true;
        yield return new WaitForSeconds(duration);
        currentlyLookingAtNpc = false;
    }
    
}
