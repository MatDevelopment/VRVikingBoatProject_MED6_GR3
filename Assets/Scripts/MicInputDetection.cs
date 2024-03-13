using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenAI;
using Unity.VisualScripting;
using UnityEngine;

public class MicInputDetection : MonoBehaviour
{
    [SerializeField] private Whisper whisperScript;
    [SerializeField] private NPCInteractorScript npcInteractorScript;
    
    public int sampleWindow = 64;

    public AudioSource source;
    private AudioClip microphoneClip;
    public AudioClip userSpeechClip;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    private int startedSpeakingPosition;
    private int stoppedSpeakingPosition;
    private float speechPauseCounter;
    private float[] speechWaveData;

    private bool isListening;

    public Vector3 minScale;

    public Vector3 maxScale;
    // Start is called before the first frame update
    void Start()
    {
        MicroPhoneToAudioClip();
    }

    // Update is called once per frame
    void Update()
    {
        speechPauseCounter += Time.deltaTime;
        
        float loudness = GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
        {
            //The speechPauseCounter variable is so that the user can have natural breaks inbetween words they say, or so-called thinking pauses.
            loudness = 0;

            if (speechPauseCounter >= 1.2 && isListening && whisperScript.isRecording == false && whisperScript.isDoneTalking && npcInteractorScript.erikSpeakable)        //If the user has not spoken in 2 seconds or more AFTER they initially started talking, then save an audio clip to be used.
            {
                if (whisperScript.userRecordingString.Length > 0)
                {
                    whisperScript.userRecordingString =
                                        whisperScript.userRecordingString.Replace(whisperScript.userRecordingString, "");
                }
                
                //Array.Clear(speechWaveData);      //Attempt to clear array, since overflow exception error
                Debug.Log("Understood");
                stoppedSpeakingPosition = Microphone.GetPosition(Microphone.devices[0]);
                //Debug.Log(stoppedSpeakingPosition); //Debugging
                //int samplesUserSpeech = stoppedSpeakingPosition - startedSpeakingPosition;        //The sample length of the audioclip to be created that stores what the user says to the NPC.
                
                int channels = microphoneClip.channels;     
                int frequency = microphoneClip.frequency;
                float[] speechWaveData = new float[microphoneClip.samples * microphoneClip.channels];
                //Samples skal muligvis have skruet op for gain, da lyden er meget lav i unity
                microphoneClip.GetData(speechWaveData, startedSpeakingPosition);
                
                CreateAndPlay(speechWaveData,channels,frequency);
                
                //speechWaveData = new float[samplesUserSpeech];
                //.GetData (speechWaveData, startedSpeakingPosition);     //Stores the user speech audio data in the float speechWaveData array.
                //Debug.Log(speechWaveData.Length);
                //userSpeechClip = AudioClip.Create("UserSpeechClip", samplesUserSpeech, 1, AudioSettings.outputSampleRate, false);
                //userSpeechClip.SetData(speechWaveData, 0);
                //userSpeechClip er det audioclip som skal gives til cloud services så vi kan få Speech To Text til at lave et transcript.
                
                whisperScript.StartRecording(); //Method responsible for transcribing the audioclip
                //userSpeechClip = null;
                isListening = false;
            }
        }

        if (loudness > threshold) //Husk at add en condition her som gør at der lige bliver ventet på at ChatGPT har genereret et svar
                                    //ELLER: Afbryd ChatGPT i at svare og append hvad end brugeren siger til samtalen, hvorefter ChatGPT kan svare på ny.
        {
            speechPauseCounter = 0;
            if (isListening == false && whisperScript.isRecording == false && whisperScript.isDoneTalking && npcInteractorScript.erikSpeakable)
            {
                userSpeechClip = null;
                StartCoroutine(whisperScript.InterruptNpcTalkingAfterDuration(whisperScript.timeToInterruptTalk));  //Runs a method to interrupt the NPC and play a "Hmm" thinking sound sample
                Debug.Log("LISTENING");
                startedSpeakingPosition = Microphone.GetPosition(Microphone.devices[0]);        //Saves the starting position of the user speech audio
                
                //Debug.Log(startedSpeakingPosition);
                isListening = true;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            source.Play();
        }
        
    }
    
    public void CreateAndPlay(float[] samples, int channels, int frequency)
    {
        userSpeechClip = AudioClip.Create("ClipName", samples.Length, channels, frequency, false);
        userSpeechClip.SetData(samples, 0);
        source.clip = userSpeechClip;
        //source.Play();
        // This code implemented through the help of Unity discussion forums: https://forum.unity.com/threads/create-audioclip-from-byte-in-unity.1205572/
        //return userSpeechClip;
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
    
    
    public float GetLoudnessFromAudioClip (int clipPosition, AudioClip clip)
    {
        var startPosition = clipPosition - sampleWindow;    //Start measuring loudness from this point in the audio clip
        if ( startPosition < 0 ) return 0;              //Failsafe that if startPosition is less than 0, then 0 is returned
    
        var waveData = new float[sampleWindow];     //The wave data from the audio clip to be looked at
        clip.GetData ( waveData, startPosition );   //Gets sample data from the given audio clip (clip) and stores them in waveData, which has a defined sample length of 64 samples.
    
        //compute loudness
        var totalLoudness = 0f;         
    
        for ( var i = 0; i < sampleWindow; ++i )            //The value of wave data ranges from -1 to 1, with 0 meaning there is no sound.
            totalLoudness += Mathf.Abs ( waveData [i]);     //We go through every sample and add it to the totalLoudness...
    
        return totalLoudness / sampleWindow;        //We then compute the totalLoudness average from the samplewindow, by dividing the totalLoudness calculated above with the ammount of samples (sampleWindow)
    }
}
