using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityCounter : MonoBehaviour
{
    public GameObject ProximitySpaces;
    public LevelChanger levelChanger;

    [Header ("Zone Management:")]
        //Erik
        [Header ("Erik Zones")]
        // Spaces
        public bool ErikInIntimateZone = false;
        public bool ErikInPersonalZone = false;
        public bool ErikInSocialZone = false;

        // Timers
        private static float ErikTimerIntimate = 0.0f;
        private static float ErikTimerPersonal = 0.0f;
        private static float ErikTimerSocial = 0.0f;
        public int ErikTimeInIntimate = 0;
        public int ErikTimeInPersonal = 0;
        public int ErikTimeInSocial = 0;

        //Frida
        [Header ("Frida Zones")]
        // Spaces
        public bool FridaInIntimateZone = false;
        public bool FridaInPersonalZone = false;
        public bool FridaInSocialZone = false;
        
        // Timers
        private static float FridaTimerIntimate = 0.0f;
        private static float FridaTimerPersonal = 0.0f;
        private static float FridaTimerSocial = 0.0f;
        public int FridaTimeInIntimate = 0;
        public int FridaTimeInPersonal = 0;
        public int FridaTimeInSocial = 0;

        //Ingrid
        [Header ("Ingrid Zones")]
        // Spaces
        public bool IngridInIntimateZone = false;
        public bool IngridInPersonalZone = false;
        public bool IngridInSocialZone = false;

        // Timers
        private static float IngridTimerIntimate = 0.0f;
        private static float IngridTimerPersonal = 0.0f;
        private static float IngridTimerSocial = 0.0f;
        public int IngridTimeInIntimate = 0;
        public int IngridTimeInPersonal = 0;
        public int IngridTimeInSocial = 0;

        //Arne
        [Header ("Arne Zones")]
        // Spaces
        public bool ArneInIntimateZone = false;
        public bool ArneInPersonalZone = false;
        public bool ArneInSocialZone = false;

        // Timers
        private static float ArneTimerIntimate = 0.0f;
        private static float ArneTimerPersonal = 0.0f;
        private static float ArneTimerSocial = 0.0f;
        public int ArneTimeInIntimate = 0;
        public int ArneTimeInPersonal = 0;
        public int ArneTimeInSocial = 0;

    void Awake()
    {
        levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
        // Find Proximity Object
        ProximitySpaces = GameObject.Find("Proximity");

        GetProximityValues();
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ErikCounter();

        if (levelChanger.Scene4Active == true)
        {
            FridaCounter();
            IngridCounter();
            ArneCounter();
        }
    }

    void OnDestroy()
    {
        StoreProximityValues();
    }

    void ErikCounter()
    {
        //When in social zone, add time to social timer
        if (ErikInSocialZone == true && ErikInPersonalZone == false && ErikInIntimateZone == false)
        {
            ErikTimerSocial += Time.deltaTime; 
            ErikTimeInSocial = (int) ErikTimerSocial;
        }

        //When in Personal zone, add time to Personal timer
        else if (ErikInSocialZone == true && ErikInPersonalZone == true && ErikInIntimateZone == false)
        {
            ErikTimerPersonal += Time.deltaTime;
            ErikTimeInPersonal = (int) ErikTimerPersonal;
        }

        //When in intimate zone, add time to intimate timer
        else if (ErikInSocialZone == true && ErikInPersonalZone == true && ErikInIntimateZone == true)
        {
            ErikTimerIntimate += Time.deltaTime;
            ErikTimeInIntimate = (int) ErikTimerIntimate;
        }
    }
    void FridaCounter()
    {
        //When in social zone, add time to social timer
        if (FridaInSocialZone == true && FridaInPersonalZone == false && FridaInIntimateZone == false)
        {
            FridaTimerSocial += Time.deltaTime; 
            FridaTimeInSocial = (int) FridaTimerSocial ;
        }

        //When in Personal zone, add time to Personal timer
        else if (FridaInSocialZone == true && FridaInPersonalZone == true && FridaInIntimateZone == false)
        {
            FridaTimerPersonal += Time.deltaTime;
            FridaTimeInPersonal = (int) FridaTimerPersonal ;
        }

        //When in intimate zone, add time to intimate timer
        else if (FridaInSocialZone == true && FridaInPersonalZone == true && FridaInIntimateZone == true)
        {
            FridaTimerIntimate += Time.deltaTime;
            FridaTimeInIntimate = (int) FridaTimerIntimate ;
        }
    }
    void IngridCounter()
    {
        //When in social zone, add time to social timer
        if (IngridInSocialZone == true && IngridInPersonalZone == false && IngridInIntimateZone == false)
        {
            IngridTimerSocial += Time.deltaTime; 
            IngridTimeInSocial = (int) IngridTimerSocial ;
        }

        //When in Personal zone, add time to Personal timer
        else if (IngridInSocialZone == true && IngridInPersonalZone == true && IngridInIntimateZone == false)
        {
            IngridTimerPersonal += Time.deltaTime;
            IngridTimeInPersonal = (int) IngridTimerPersonal;
        }

        //When in intimate zone, add time to intimate timer
        else if (IngridInSocialZone == true && IngridInPersonalZone == true && IngridInIntimateZone == true)
        {
            IngridTimerIntimate += Time.deltaTime;
            IngridTimeInIntimate = (int)IngridTimerIntimate;
        }
    }
    void ArneCounter()
    {
        //When in social zone, add time to social timer
        if (ArneInSocialZone == true && ArneInPersonalZone == false && ArneInIntimateZone == false)
        {
            ArneTimerSocial += Time.deltaTime; 
            ArneTimeInSocial = (int) ArneTimerSocial;
        }

        //When in Personal zone, add time to Personal timer
        else if (ArneInSocialZone == true && ArneInPersonalZone == true && ArneInIntimateZone == false)
        {
            ArneTimerPersonal += Time.deltaTime;
            ArneTimeInPersonal = (int) ArneTimerPersonal;
        }

        //When in intimate zone, add time to intimate timer
        else if (ArneInSocialZone == true && ArneInPersonalZone == true && ArneInIntimateZone == true)
        {
            ArneTimerIntimate += Time.deltaTime;
            ArneTimeInIntimate = (int) ArneTimerIntimate;
        }
    }


    void StoreProximityValues()
    {
        // Erik
            //Intimate Values
            PlayerPrefs.SetFloat("ErikIntimate", ErikTimerIntimate);
            PlayerPrefs.SetInt("ErikIntimateTime", ErikTimeInIntimate);

            //Personal Values
            PlayerPrefs.SetFloat("ErikPersonal", ErikTimerPersonal);
            PlayerPrefs.SetInt("ErikPersonalTime", ErikTimeInPersonal);

            //Social Values
            PlayerPrefs.SetFloat("ErikSocial", ErikTimerSocial);
            PlayerPrefs.SetInt("ErikSocialTime", ErikTimeInSocial);

        //Arne
            //Intimate Values
            PlayerPrefs.SetFloat("ArneIntimate", ArneTimerIntimate);
            PlayerPrefs.SetInt("ArneIntimateTime", ArneTimeInIntimate);

            //Personal Values
            PlayerPrefs.SetFloat("ArnePersonal", ArneTimerPersonal);
            PlayerPrefs.SetInt("ArnePersonalTime", ArneTimeInPersonal);

            //Social Values
            PlayerPrefs.SetFloat("ArneSocial", ArneTimerSocial);
            PlayerPrefs.SetInt("ArneSocialTime", ArneTimeInSocial);
            
        //Frida
            //Intimate Values
            PlayerPrefs.SetFloat("FridaIntimate", FridaTimerIntimate);
            PlayerPrefs.SetInt("FridaIntimateTime", FridaTimeInIntimate);

            //Personal Values
            PlayerPrefs.SetFloat("FridaPersonal", FridaTimerPersonal);
            PlayerPrefs.SetInt("FridaPersonalTime", FridaTimeInPersonal);

            //Social Values
            PlayerPrefs.SetFloat("FridaSocial", FridaTimerSocial);
            PlayerPrefs.SetInt("FridaSocialTime", FridaTimeInSocial);

        //Ingrid
            //Intimate Values
            PlayerPrefs.SetFloat("IngridIntimate", IngridTimerIntimate);
            PlayerPrefs.SetInt("IngridIntimateTime", IngridTimeInIntimate);

            //Personal Values
            PlayerPrefs.SetFloat("IngridPersonal", IngridTimerPersonal);
            PlayerPrefs.SetInt("IngridPersonalTime", IngridTimeInPersonal);

            //Social Values
            PlayerPrefs.SetFloat("IngridSocial", IngridTimerSocial);
            PlayerPrefs.SetInt("IngridSocialTime", IngridTimeInSocial);
    }

    void GetProximityValues()
    {
        // Erik
            // Intimate Values
            ErikTimerIntimate = PlayerPrefs.GetFloat("ErikIntimate", ErikTimerIntimate);
            ErikTimeInIntimate = PlayerPrefs.GetInt("ErikIntimateTime", ErikTimeInIntimate);

            // Personal Values
            ErikTimerPersonal = PlayerPrefs.GetFloat("ErikPersonal", ErikTimerPersonal);
            ErikTimeInPersonal = PlayerPrefs.GetInt("ErikPersonalTime", ErikTimeInPersonal);

            // Social Values
            ErikTimerSocial= PlayerPrefs.GetFloat("ErikSocial", ErikTimerSocial);
            ErikTimeInSocial = PlayerPrefs.GetInt("ErikSocialTime", ErikTimeInSocial);

        //Arne
            // Intimate Values
            ArneTimerIntimate = PlayerPrefs.GetFloat("ArneIntimate", ArneTimerIntimate);
            ArneTimeInIntimate = PlayerPrefs.GetInt("ArneIntimateTime", ArneTimeInIntimate);
    
            // Personal Values
            ArneTimerPersonal = PlayerPrefs.GetFloat("ArnePersonal", ArneTimerPersonal);
            ArneTimeInPersonal = PlayerPrefs.GetInt("ArnePersonalTime", ArneTimeInPersonal);
    
            // Social Values
            ArneTimerSocial= PlayerPrefs.GetFloat("ArneSocial", ArneTimerSocial);
            ArneTimeInSocial = PlayerPrefs.GetInt("ArneSocialTime", ArneTimeInSocial);
        
        //Frida
            // Intimate Values
            FridaTimerIntimate = PlayerPrefs.GetFloat("FridaIntimate", FridaTimerIntimate);
            FridaTimeInIntimate = PlayerPrefs.GetInt("FridaIntimateTime", FridaTimeInIntimate);
    
            // Personal Values
            FridaTimerPersonal = PlayerPrefs.GetFloat("FridaPersonal", FridaTimerPersonal);
            FridaTimeInPersonal = PlayerPrefs.GetInt("FridaPersonalTime", FridaTimeInPersonal);
    
            // Social Values
            FridaTimerSocial= PlayerPrefs.GetFloat("FridaSocial", FridaTimerSocial);
            FridaTimeInSocial = PlayerPrefs.GetInt("FridaSocialTime", FridaTimeInSocial);

        //Ingrid
            // Intimate Values
            IngridTimerIntimate = PlayerPrefs.GetFloat("IngridIntimate", IngridTimerIntimate);
            IngridTimeInIntimate = PlayerPrefs.GetInt("IngridIntimateTime", IngridTimeInIntimate);
    
            // Personal Values
            IngridTimerPersonal = PlayerPrefs.GetFloat("IngridPersonal", IngridTimerPersonal);
            IngridTimeInPersonal = PlayerPrefs.GetInt("IngridPersonalTime", IngridTimeInPersonal);
    
            // Social Values
            IngridTimerSocial= PlayerPrefs.GetFloat("IngridSocial", IngridTimerSocial);
            IngridTimeInSocial = PlayerPrefs.GetInt("IngridSocialTime", IngridTimeInSocial);
    }
}
