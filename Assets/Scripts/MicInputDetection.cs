using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenAI;
using Unity.VisualScripting;
using UnityEngine;

public class MicInputDetection : MonoBehaviour
{

    private APIStatus apiStatus;
    [SerializeField] private TextToSpeech textToSpeech;
    [SerializeField] private AudioPlayer audioPlayer;

    [SerializeField] private Whisper whisperScript;
    [SerializeField] private NPCInteractorScript npcInteractorScript;
    [SerializeField] private MicInputUI micInputUI;
    public int sampleWindow = 64;

    public AudioSource source;
    private AudioClip microphoneClip;
    public AudioClip userSpeechClip;   //userSpeechClip er det audioclip som skal gives til cloud services så vi kan få Speech To Text til at lave et transcript.

    [Range(1f, 30f)]
    public float maxAmplitudeWithCurrentMicrophone = 15f;
    public float loudnessSensibility = 100;
    public float threshold = 6f;
    private int startedSpeakingPosition;
    private int stoppedSpeakingPosition;
    private float speechPauseCounter, speechPauseCounterThreshold = 1.5f;
    private float[] speechWaveData;

    public bool isListening = false;

    public Vector3 minScale;

    public Vector3 maxScale;

    public float loudness;

    void Start()
    {
        micInputUI = FindObjectOfType<MicInputUI>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        textToSpeech = FindObjectOfType<TextToSpeech>();
        apiStatus = FindObjectOfType<APIStatus>();
        MicroPhoneToAudioClip();
    }

    void Update()
    {
        speechPauseCounter += Time.deltaTime;

        loudness = GetLoudnessFromMicrophone() * loudnessSensibility;

        if (!npcInteractorScript.erikSpeakable) return;

        if (loudness < threshold)       //What you say will sometimes be SPAMMED in the same response, which is possibly due to this Update check... Find other way to do it...
        {
            //The speechPauseCounter variable is so that the user can have natural breaks inbetween words they say, or so-called thinking pauses.
           // loudness = 0;

            if (speechPauseCounter >= speechPauseCounterThreshold && isListening == true && apiStatus.isTranscribing == false && !apiStatus.isTalking)        //If the user has not spoken in 2 seconds or more AFTER they initially started talking, then save an audio clip to be used.
            {
                if (whisperScript.userRecordingString.Length > 0)
                {
                    whisperScript.userRecordingString =
                                        whisperScript.userRecordingString.Replace(whisperScript.userRecordingString, "");
                }

                Debug.Log("speech pause went over threshold of: " + speechPauseCounterThreshold + " seconds");

                int channels = microphoneClip.channels;
                int frequency = microphoneClip.frequency;
                float[] speechWaveData = new float[microphoneClip.samples * microphoneClip.channels];

                microphoneClip.GetData(speechWaveData, startedSpeakingPosition);

                CreateUserSpeechClip(speechWaveData, channels, frequency);

                whisperScript.TranscribeRecordedAudio(); //Method responsible for transcribing the audioclip

                isListening = false;
                speechPauseCounter = 0;
            }
        }

        if (loudness > threshold) //Husk at add en condition her som gør at der lige bliver ventet på at ChatGPT har genereret et svar
                                  //ELLER: Afbryd ChatGPT i at svare og append hvad end brugeren siger til samtalen, hvorefter ChatGPT kan svare på ny.
        {
            //npcInteractorScript.timeCounter = 0;
            speechPauseCounter = 0; // Resets the pause counter so the microphone keep recording

            if (isListening == false)
            {
                Debug.Log("LISTENING");
                isListening = true;
            }

            if (apiStatus.isTranscribing == false
                && apiStatus.isGeneratingText == false
                && apiStatus.isGeneratingAudio == false
                && apiStatus.isTalking == false)
            {
                whisperScript.userRecordingString = "";    
                userSpeechClip = null;

                startedSpeakingPosition = Microphone.GetPosition(Microphone.devices[0]);        //Saves the starting position of the user speech audio
            }
            else if (apiStatus.isTalking)
            {
                whisperScript.userRecordingString = "";   
                userSpeechClip = null;

                startedSpeakingPosition = Microphone.GetPosition(Microphone.devices[0]);       

                StartCoroutine(audioPlayer.InterruptNpcTalkingAfterDuration(whisperScript.timeToInterruptTalk));  //Runs a method to interrupt the NPC and play a "Hmm" thinking sound sample
            }
            else if (apiStatus.isTranscribing == true
                || apiStatus.isGeneratingText == true
                || apiStatus.isGeneratingAudio == true)
            {
                micInputUI.SetText("Erik is thinking");
                Debug.Log("Erik is thinking!!");
                isListening = false;
            }


            //hvis Erik han snakker skal han stoppe med at snakke og sige "hmm" og begynde at lytte igen. Her stopper vi ham bare i at snakke
            //hvik erik han tænker så skal han ikke sige "hmm"  han skal bare begynde at tænke igen. Her skal vi stoppe alle de andre API calls.
            //Hvis Erik ikke snakker skal han bare begynde at lytte.





            // old logic
            // if (isListening == false && apiStatus.isTranscribing == false && !apiStatus.isTalking)
            //{
            //    whisperScript.userRecordingString = "";     //NEWLY ADDED!!!!!!!!!!!!
            //    userSpeechClip = null;

            //    Debug.Log("LISTENING");

            //    StartCoroutine(whisperScript.InterruptNpcTalkingAfterDuration(whisperScript.timeToInterruptTalk));  //Runs a method to interrupt the NPC and play a "Hmm" thinking sound sample

            //    startedSpeakingPosition = Microphone.GetPosition(Microphone.devices[0]);        //Saves the starting position of the user speech audio

            //    isListening = true;
            //}
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            source.Play();
        }
    }

    public void CreateUserSpeechClip(float[] samples, int channels, int frequency)
    {
        userSpeechClip = AudioClip.Create("ClipName", samples.Length, channels, frequency, false);
        userSpeechClip.SetData(samples, 0);
        source.clip = userSpeechClip;
        // This code implemented through the help of Unity discussion forums: https://forum.unity.com/threads/create-audioclip-from-byte-in-unity.1205572/
    }

    public void MicroPhoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 10, AudioSettings.outputSampleRate);    //Sets the audioClip "microphoneClip" to the default microphone.
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }


    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        var startPosition = clipPosition - sampleWindow;    //Start measuring loudness from this point in the audio clip
        if (startPosition < 0) return 0;              //Failsafe that if startPosition is less than 0, then 0 is returned

        var waveData = new float[sampleWindow];     //The wave data from the audio clip to be looked at
        clip.GetData(waveData, startPosition);   //Gets sample data from the given audio clip (clip) and stores them in waveData, which has a defined sample length of 64 samples.

        //compute loudness
        var totalLoudness = 0f;

        for (var i = 0; i < sampleWindow; ++i)            //The value of wave data ranges from -1 to 1, with 0 meaning there is no sound.
            totalLoudness += Mathf.Abs(waveData[i]);     //We go through every sample and add it to the totalLoudness...

        return totalLoudness / sampleWindow;        //We then compute the totalLoudness average from the samplewindow, by dividing the totalLoudness calculated above with the ammount of samples (sampleWindow)
    }
}