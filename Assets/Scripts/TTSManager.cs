using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;
using Debug = UnityEngine.Debug;

public class TTSManager : MonoBehaviour
{
    private OpenAIWrapper openAIWrapper;
    [SerializeField] private APICallTimeManager apiCallTimeManager;
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private TTSModel model = TTSModel.TTS_1;
    [SerializeField] private TTSVoice voice = TTSVoice.Alloy;
    [SerializeField, Range(0.25f, 4.0f)] private float speed = 1f;


    private void OnEnable()
    {
        if (!openAIWrapper) this.openAIWrapper = FindObjectOfType<OpenAIWrapper>();
        if (!audioPlayer) this.audioPlayer = GetComponentInChildren<AudioPlayer>();
        if (!apiCallTimeManager) this.apiCallTimeManager = FindObjectOfType<APICallTimeManager>();
    }

    public async void SynthesizeAndPlay(string text)
    {
        Debug.Log("Trying to synthesize :" + text);

        Stopwatch stopwatch = Stopwatch.StartNew(); // Start measuring time

        byte[] audioData = await openAIWrapper.RequestTextToSpeech(text, model, voice, speed);

        stopwatch.Stop(); // Stop measuring time

        if (audioData != null)
        {
            apiCallTimeManager.AddCallDuration_TextToSpeech(stopwatch.Elapsed.TotalSeconds);

            Debug.Log("Playing the synthesized audio now.");
            audioPlayer.ProcessAudioBytes(audioData);
        }
        else
        {
            Debug.LogError("Failed to get audio data from OpenAI.");
        }
    }

    public async void SynthesizeAndPlay(string text, TTSModel model, TTSVoice voice, float speed)
    {
        this.model = model;
        this.voice = voice;
        this.speed = speed;
        SynthesizeAndPlay(text);
    }
}