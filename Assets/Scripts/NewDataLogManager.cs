using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class NewDataLogManager : MonoBehaviour
{
    [Header("Start Logging")]
    public bool AllowLogging = true;

    [Header("Needed Scripts")]
    [SerializeField] Whisper whisper;

    [Header("File Related")]
    // Variables for outputting the log file
    [SerializeField] string newFileName = "";
    private StreamWriter sw;
    static string Filename = "";
    string path;
    DateTimeOffset localTime;

    private void Awake()
    {
        // Get the current date and time in GMT+2
        localTime = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(2));

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
        if (AllowLogging == true)
        {
            // Creating a new Streamwriter object with desired path
            sw = new StreamWriter(path);
        }
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
        string SensorLogText = "Experience Time = " + currentTime;
        // Appending the string to the textfile which means it is written behind the current text
        sw.WriteLine(SensorLogText);
    }
}
