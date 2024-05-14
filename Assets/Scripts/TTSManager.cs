using OpenAI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using Debug = UnityEngine.Debug;

public class TTSManager : MonoBehaviour
{
    private OpenAIWrapper openAIWrapper;
    [SerializeField] private APIStatus apiStatus;
    [SerializeField] private APICallTimeManager apiCallTimeManager;
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private Whisper whisper;
    [SerializeField] private NewDataLogManager newDataLogManager;
    [SerializeField] private MicInputUI micInputUI;
    [SerializeField] private TTSModel model = TTSModel.TTS_1;
    [SerializeField] private TTSVoice voice = TTSVoice.Alloy;
    [SerializeField, Range(0.25f, 4.0f)] private float speed = 1f;


    private void OnEnable()
    {
        apiStatus = FindObjectOfType<APIStatus>();
        if (!openAIWrapper) this.openAIWrapper = FindObjectOfType<OpenAIWrapper>();
        if (!audioPlayer) this.audioPlayer = GetComponentInChildren<AudioPlayer>();
        if (!apiCallTimeManager) this.apiCallTimeManager = FindObjectOfType<APICallTimeManager>();
    }

    public async Task SynthesizeAndPlay(string text)
    {
        Debug.Log("Trying to synthesize :" + text);

        Stopwatch stopwatch = Stopwatch.StartNew(); // Start measuring time
        apiStatus.isGeneratingAudio = true;

        byte[] audioData = await openAIWrapper.RequestTextToSpeech(text, model, voice, speed);

        micInputUI.SetText("");
        stopwatch.Stop(); // Stop measuring time
        apiStatus.isGeneratingAudio = false;
        
        if (audioData != null)
        {
            apiCallTimeManager.AddCallDuration_TextToSpeech(stopwatch.Elapsed.TotalSeconds);

            Debug.Log("Playing the synthesized audio now.");
            audioPlayer.ProcessAudioBytes(audioData);

            newDataLogManager.SendStringToDataLogger("Erik: " + "[API: " + apiCallTimeManager.GetLastCombinedCallTime() + "] " + whisper.uneditedNPCResponse);
        }
        else
        {
            Debug.LogError("Failed to get audio data from OpenAI.");
        }
    }

    /*public async void SynthesizeAndPlay(string text, TTSModel model, TTSVoice voice, float speed)
    {
        this.model = model;
        this.voice = voice;
        this.speed = speed;
        SynthesizeAndPlay(text);
    }*/
}