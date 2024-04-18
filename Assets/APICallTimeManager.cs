using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class APICallTimeManager : MonoBehaviour // Responsible for holding callTimes for each of APIs and saving a history of call times.
{
    public List<double> CallDurations_SpeechToText = new List<double>();
    public List<double> CallDurations_ChatGPT = new List<double>();
    public List<double> CallDurations_TextToSpeech = new List<double>();

    public List<double> CombinedCallTimes = new List<double>();

    private void CombineLastEntries()
    {
        if (CallDurations_TextToSpeech.Count == 0) return;

        int entryID = CallDurations_SpeechToText.Count - 1;
        CombinedCallTimes.Add(CallDurations_SpeechToText[entryID] + CallDurations_ChatGPT[entryID] + CallDurations_TextToSpeech[entryID]);
    }


    public void AddCallDuration_SpeechToText(double callTime)
    {
        CallDurations_SpeechToText.Add(callTime);

        Debug.Log("SpeechToText API call took: " + callTime + " seconds."); // Log time taken
    }

    public void AddCallDuration_ChatGPT(double callTime)
    {
        CallDurations_ChatGPT.Add(callTime);

        Debug.Log("ChatGPT API call took: " + callTime + " seconds.");
    }

    public void AddCallDuration_TextToSpeech(double callTime)
    {
        CallDurations_TextToSpeech.Add(callTime);
        CombineLastEntries();

        Debug.Log("TextToSpeech API call took: " + callTime + " seconds.");

    }
}