using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
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
    [SerializeField] APICallTimeManager apiCallTimeManager;

    [Header("File Related")]
    // Variables for outputting the log file
    [SerializeField] string newFileName = "";
    private StreamWriter sw;
    static string Filename = "";
    string path;
    DateTimeOffset localTime;

    public List<string> StringsToLog = new List<string>();
    public int TotalUserPrompts = 0;
    public int TotalErikResponses = 0;
    public int TotalErikInstigations = 0;

    public float ErikGazeTime = 0;

    public float timeThatTutorialEnded;

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
        // int currentTime = (int)Mathf.Round(Time.time);
        string currentTime = FormatTime(Time.time);
        string tutorialTime = FormatTime(timeThatTutorialEnded);
        string currentTimeMinusTutorialTimes = FormatTime(Time.time - timeThatTutorialEnded) ;

        // Calculating average API call times:
        // STT
        double totalSTTTime = 0;
        foreach (double time in apiCallTimeManager.CallDurations_SpeechToText)
        {
            totalSTTTime += time;
        }
        double STTaverage = totalSTTTime / apiCallTimeManager.CallDurations_SpeechToText.Count;

        // ChatGPT
        double totalGPTTime = 0;
        foreach (double time in apiCallTimeManager.CallDurations_ChatGPT)
        {
            totalGPTTime += time;
        }
        double GPTaverage = totalGPTTime / apiCallTimeManager.CallDurations_ChatGPT.Count;

        // TTS
        double totalTTSTime = 0;
        foreach (double time in apiCallTimeManager.CallDurations_TextToSpeech)
        {
            totalTTSTime += time;
        }
        double TTSaverage = totalGPTTime / apiCallTimeManager.CallDurations_TextToSpeech.Count;

        // Combined
        double totalCombinedTime = 0;
        foreach (double time in apiCallTimeManager.CombinedCallTimes)
        {
            totalCombinedTime += time;
        }
        double combinedAverage = totalCombinedTime / apiCallTimeManager.CombinedCallTimes.Count;

        // Defining what is written in the log text
        string SensorLogText = "Total Time: " + currentTime + " Tutorial Time: " + timeThatTutorialEnded + " Total Time minus tutorial: " + currentTimeMinusTutorialTimes + ", User Prompts: " + TotalUserPrompts + ", Erik Responses: " + TotalErikResponses + ", Erik Instigations: " + TotalErikInstigations + ", Erik Gaze Time: " + ErikGazeTime + ", API Call Time Averages:" + " Combined " + combinedAverage + ", SST: " + STTaverage + ", ChatGPT: " + GPTaverage + ", TTS: " + TTSaverage;
        // Appending the string to the textfile which means it is written behind the current text
        sw.WriteLine(SensorLogText);

        foreach (string LoggedString in StringsToLog)
        {
            sw.WriteLine(LoggedString);
        }
    }

    public void SendStringToDataLogger(string DataToBeLogged)
    {
        StringsToLog.Add("[" + FormatTime(Time.time) + "] " + DataToBeLogged);
    }

    // Method to format time in minutes and seconds
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
