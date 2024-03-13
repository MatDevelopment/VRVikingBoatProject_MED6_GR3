using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntimateSpace : MonoBehaviour
{
    public ProximityCounter proximityCounter;
    public LevelChanger levelChanger;

    void Awake()
    {
        proximityCounter = GameObject.Find("Proximity").GetComponent<ProximityCounter>();
    }

    private void Start()
    {
        levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInIntimateZone = true;
        }

        if(levelChanger.Scene4Active == true)
        {
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInIntimateZone = true;
            }
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInIntimateZone = true;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInIntimateZone = true;
            }
        } 
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInIntimateZone = true;
        }

        if(levelChanger.Scene4Active == true)
        {
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInIntimateZone = true;
            }
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInIntimateZone = true;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInIntimateZone = true;
            }
        }   
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInIntimateZone = false;
        }

        if(levelChanger.Scene4Active == true)
        {
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInIntimateZone = false;
            }
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInIntimateZone = false;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInIntimateZone = false;
            }
        } 
    }
}
