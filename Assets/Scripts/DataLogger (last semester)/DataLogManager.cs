using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Threading;
using System.IO;
using UnityEngine.InputSystem;

public class DataLogManager : MonoBehaviour
{
    [Header ("Start Logging")]
    public bool AllowLogging = true;

    [Header ("Find Objects")]
    public LevelChanger levelChanger;
    public ProximityCounter proximityCounter;
    public GameObject gameObject;

    // Prompt button
    [SerializeField] private InputActionReference PromptButtonPress;

    [Header ("File Related")]
    // Variables for outputting the log file
    [SerializeField] string newFileName = "";
    private StreamWriter sw;
    static string Filename = "";
    string path;
    DateTimeOffset localTime;

    // Variables to catch from other scripts
    [Header ("Logged Variables")]
    public static int NPCGazeTime;
    public static int PromptAmount;

    // Proximity Time
     //Erik
     public static int ErikSocialTime = 0;
     public static int ErikPersonalTime = 0;
     public static int ErikIntimateTime = 0;

     //Frida
     public static int FridaSocialTime = 0;
     public static int FridaPersonalTime = 0;
     public static int FridaIntimateTime = 0;

     //Ingrid
     public static int IngridSocialTime = 0;
     public static int IngridPersonalTime = 0;
     public static int IngridIntimateTime = 0;

     //Arne
     public static int ArneSocialTime = 0;
     public static int ArnePersonalTime = 0;
     public static int ArneIntimateTime = 0;

    // Gaze Time Variables

    public static float Erik_GazeTime;
    public static float Arne_GazeTime;
    public static float Frida_GazeTime;
    public static float Ingrid_GazeTime;
    

    // Keep script intact
    static DataLogManager dataLogManager;

    void Awake()
    {       
        // Get Prompt Amount across scenes
        PromptAmount = PlayerPrefs.GetInt("PromptAmount", PromptAmount);

        // Get the current date and time in GMT+2
        localTime = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(2));

        

        // Get ProximityCounter
        proximityCounter = GameObject.Find("Proximity").GetComponent<ProximityCounter>();

        // Enable prompt count on press
        PromptButtonPress.action.Enable();
        PromptButtonPress.action.performed += CountTalkPrompts;

        // Don't overwrite file
        if (PlayerPrefs.GetString("Filename", Filename) != "" && PlayerPrefs.GetString("Filename", Filename) != null)
        {
        Filename = PlayerPrefs.GetString("Filename", Filename);
        }
        else
        {
            Filename = "emergencyLog";
        }

        if (newFileName != "")
        {
            if (newFileName != Filename)
            {
                Filename = newFileName;
            }
        }

        if (AllowLogging == true)
        {
            CreateLogText();
        }
        else
        {
            Debug.Log("Logging Not Allowed!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get LevelChanger
        levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();

        if (AllowLogging == true)
        {
            // Creating a new Streamwriter object with desired path
            sw = new StreamWriter(path);
        }     
    }

    // Update is called once per frame
    void Update()
    {
        // Proximity Times
            //Erik
            ErikSocialTime = proximityCounter.ErikTimeInSocial;
            ErikPersonalTime = proximityCounter.ErikTimeInPersonal;
            ErikIntimateTime = proximityCounter.ErikTimeInIntimate;

            //Frida
            FridaSocialTime = proximityCounter.FridaTimeInSocial;
            FridaPersonalTime = proximityCounter.FridaTimeInPersonal;
            FridaIntimateTime = proximityCounter.FridaTimeInIntimate;

            //Ingrid
            IngridSocialTime = proximityCounter.IngridTimeInSocial;
            IngridPersonalTime = proximityCounter.IngridTimeInPersonal;
            IngridIntimateTime = proximityCounter.IngridTimeInIntimate;

            //Arne
            ArneSocialTime = proximityCounter.ArneTimeInSocial;
            ArnePersonalTime = proximityCounter.ArneTimeInPersonal;
            ArneIntimateTime = proximityCounter.ArneTimeInIntimate;
            
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("PromptAmount", PromptAmount);
    }

    void OnApplicationQuit()
    {
        if (AllowLogging == true)
        {
            LogUpdate();
            sw.Close();
        }
    }

    private void CreateLogText()
    {
        // Get current time and date
        string filedate = localTime.ToString(" yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture) + ".txt";

        // Define the path of the log file
        path = Application.dataPath + "/TestLogs" + "/" + Filename + filedate;

        // Write to the file
        File.WriteAllText(path, "");
    }

    public void LogUpdate()
    {
        int currentTime = (int)Mathf.Round(Time.time);
        // Defining what is written in the log text
        string SensorLogText = "Experience Time = " + currentTime + ", Prompt Amount = " + PromptAmount + 
        ", Social Proximity Time To Erik = " + ErikSocialTime + ", Personal Proximity Time To Erik = " + ErikPersonalTime + ", Intimate Proximity Time To Erik = " + ErikIntimateTime + 
        ", Social Proximity Time To Frida = " + FridaSocialTime + ", Personal Proximity Time To Frida = " + FridaPersonalTime + ", Intimate Proximity Time To Frida = " + FridaIntimateTime + 
        ", Social Proximity Time To Ingrid = " + IngridSocialTime + ", Personal Proximity Time To Ingrid = " + IngridPersonalTime + ", Intimate Proximity Time To Ingrid = " + IngridIntimateTime + 
        ", Social Proximity Time To Arne = " + ArneSocialTime + ", Personal Proximity Time To Arne = " + ArnePersonalTime + ", Intimate Proximity Time To Arne = " + ArneIntimateTime +
        ", Erik Gaze time = " + Erik_GazeTime + ", Arne Gaze time = " + Arne_GazeTime + ", Frida Gaze time = " + Frida_GazeTime + ", Ingrid Gaze time = " + Ingrid_GazeTime;

        // Appending the string to the textfile which means it is written behind the current text
        sw.WriteLine(SensorLogText);
    }

    
    /*public void StartCountGaze()        //Called on action event of Hover Enter on NPC gaze collider
    {
        startGazeTime = Time.fixedTime;         //The time in seconds since the start of the game saved in startGazeTime float variable
    }

    public void StopGazeCount()
    {
        stopGazeTime = Time.fixedTime;          //The time in seconds since the start of the game stored in stopGazeTime, when the user stops looking at an NPC
        totalGazeTime += (stopGazeTime - startGazeTime);        //The time that the user has JUST spent looking at an NPC is added to the totalGazeTime float variable,
                                                                //by subtracting the time from when the user started looking at the NPC, from the current time when
                                                                //the user stopped looking at the NPC.
    }*/

    private void CountTalkPrompts(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PromptAmount += 1;
        }
    } 
}
