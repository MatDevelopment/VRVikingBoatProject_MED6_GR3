using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialSpace : MonoBehaviour
{
    public ProximityCounter proximityCounter;
    public LevelChanger levelChanger;

    void Awake()
    {
        proximityCounter = GameObject.Find("Proximity").GetComponent<ProximityCounter>();
        //levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }

    private void Start()
    {
        levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInSocialZone = true;
        }

        if (levelChanger.Scene4Active == true)
        {
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInSocialZone = true;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInSocialZone = true;
            }
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInSocialZone = true;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInSocialZone = true;
        }

        if (levelChanger.Scene4Active == true)
        {
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInSocialZone = true;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInSocialZone = true;
            }
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInSocialZone = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Erik"))
        {
            proximityCounter.ErikInSocialZone = false;
        }

        if (levelChanger.Scene4Active == true)
        {
            if (other.gameObject.CompareTag("Arne"))
            {
                proximityCounter.ArneInSocialZone = false;
            }
            if (other.gameObject.CompareTag("Ingrid"))
            {
                proximityCounter.IngridInSocialZone = false;
            }
            if (other.gameObject.CompareTag("Frida"))
            {
                proximityCounter.FridaInSocialZone = false;
            }
        }
       
    }
}
