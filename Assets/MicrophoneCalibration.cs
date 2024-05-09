using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneCalibration : MonoBehaviour// Calibrating the maxAmplitude for the microphone.
{
    [SerializeField] private Tutorial tutorial;
    [SerializeField] private MicInputDetection micInputDetection;
    public float loudnessSection1, loudnessSection2;

    public float currentLoudness, maxLoudnessHeard;
    public bool detectLoudness;
    private const float microphoneThreshold = 2f; // Adjust as needed

    public float averageLoudness;

    void Start()
    {
        tutorial = FindObjectOfType<Tutorial>();
        micInputDetection = FindObjectOfType<MicInputDetection>();
    }

    void Update()
    {
        if (!detectLoudness) return;

        currentLoudness = micInputDetection.loudness;

        if (currentLoudness > maxLoudnessHeard)
        {
            maxLoudnessHeard = currentLoudness;
        }
    }

    public IEnumerator ListenForLoudnessForDuration(float listenDuration)
    {
        detectLoudness = true;
        yield return new WaitForSeconds(listenDuration);

        averageLoudness = maxLoudnessHeard;
        if (averageLoudness >= microphoneThreshold)
        {
            tutorial.UserTalkedLoudEnough();
            SetMIcrophoneCalibratedValues();
        }
        else
        {
            tutorial.UserDidNotTalkLoudEnough();
        }
        detectLoudness = false;
    }

    private void SetMIcrophoneCalibratedValues()
    {
        micInputDetection.threshold = averageLoudness - 3.2f;
        micInputDetection.maxAmplitudeWithCurrentMicrophone = averageLoudness + 6;
    }

}
