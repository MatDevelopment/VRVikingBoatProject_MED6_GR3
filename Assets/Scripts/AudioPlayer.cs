using System;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private NPCSoundBitPlayer nPCSoundBitPlayer;
    [SerializeField] APIStatus apiStatus;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private MicInputUI micInputUI;

    private bool deleteCachedFile = true;

    private void OnEnable()
    {
        nPCSoundBitPlayer = FindObjectOfType<NPCSoundBitPlayer>();
        apiStatus = FindObjectOfType<APIStatus>();
        micInputUI = FindObjectOfType<MicInputUI>();

    } 

    public void ProcessAudioBytes(byte[] audioData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "audio.mp3");
        File.WriteAllBytes(filePath, audioData);

        StartCoroutine(LoadAndPlayAudio(filePath));
    }

    private IEnumerator LoadAndPlayAudio(string filePath)
    {
        using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + filePath, AudioType.MPEG);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            AudioClip audioClip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = audioClip;

            audioSource.Play();
            apiStatus.isTalking = true;
            apiStatus.StartCoroutine(apiStatus.SetIsTalkingFalseAfterTime(audioClip.length));
        }
        else
        {
            Debug.LogError("Audio file loading error: " + www.error);
        }

        if (deleteCachedFile) File.Delete(filePath);
    }
    public IEnumerator InterruptNpcTalkingAfterDuration(float interruptDuration) //TODO: move this to another class
    {
        Debug.Log("Interrupted Erik while speaking!");
        micInputUI.SetText("Interrupted Erik while speaking!");
        yield return new WaitForSeconds(interruptDuration);

        audioSource.Stop();
        StartCoroutine(nPCSoundBitPlayer.PlayHmmThinkingSound(interruptDuration+interruptDuration));
    }

    public bool AudioSourcePlaying()
    {
        if (audioSource.isPlaying)
            return true;
        else
            return false;
    }
}