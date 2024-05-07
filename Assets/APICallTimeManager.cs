using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class APICallTimeManager : MonoBehaviour // Responsible for holding callTimes for each of APIs and saving a history of call times.
{
    public List<double> CallDurations_SpeechToText = new List<double>();
    public List<double> CallDurations_ChatGPT = new List<double>();
    public List<double> CallDurations_TextToSpeech = new List<double>();

    public List<double> CombinedCallTimes = new List<double>();

    private void CombineLastEntries()//Check if there is the same amount of things in the lists. else remove the last
    {
        if (CallDurations_TextToSpeech.Count == 0) return;

        int entryID = CallDurations_SpeechToText.Count - 1;


        int length = CallDurations_SpeechToText.Count;        

        if (CallDurations_ChatGPT.Count == length && CallDurations_TextToSpeech.Count == length) // check if every list has the same number of entries
            CombinedCallTimes.Add(CallDurations_SpeechToText[entryID] + CallDurations_ChatGPT[entryID] + CallDurations_TextToSpeech[entryID]);
        else
        {
            Debug.LogWarning("FIX ME!:D one API call time list has more entries than the others.");

            //Removing the last faulty entry in all of the lists. // This code doesn't work! :o
            //if (CallDurations_SpeechToText.Count != length-1)
            //    CallDurations_SpeechToText.RemoveAt(length-1);
            //if (CallDurations_ChatGPT.Count != length - 1)
            //    CallDurations_ChatGPT.RemoveAt(length-1);
            //if (CallDurations_TextToSpeech.Count != length - 1)
            //    CallDurations_TextToSpeech.RemoveAt(length-1);
        }
    }

    public string GetLastCombinedCallTime()
    {
        if (CombinedCallTimes.Count > 0)
        {
            double lastEntry = CombinedCallTimes[CombinedCallTimes.Count - 1];
            return lastEntry.ToString();
        }
        else
        {
            return "List is empty";
        }
    }

    private void checkIfAllEntriesListsHaveSameAmountOfEntries()
    {
        int max = CallDurations_SpeechToText.Count;

        if (CallDurations_ChatGPT.Count > max)
        {
            max = CallDurations_ChatGPT.Count;
        }

        if (CallDurations_TextToSpeech.Count > max)
        {
            max = CallDurations_TextToSpeech.Count;
        }

        if (CallDurations_SpeechToText.Count < max)
            CallDurations_SpeechToText.Add(0);

        if (CallDurations_ChatGPT.Count < max)
            CallDurations_ChatGPT.Add(0);

        if (CallDurations_TextToSpeech.Count < max)
            CallDurations_TextToSpeech.Add(0);
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
        

        Debug.Log("TextToSpeech API call took: " + callTime + " seconds.");

        checkIfAllEntriesListsHaveSameAmountOfEntries();
        CombineLastEntries();
    }
}