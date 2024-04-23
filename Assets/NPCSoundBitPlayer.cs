using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCSoundBitPlayer : MonoBehaviour
{
    public AudioClip[] currentNpcThinkingSoundsArray;
    private AudioClip clip;
    [FormerlySerializedAs("hmmThinkingSound")][SerializeField] private AudioClip ErikHmmSound;
    public AudioSource audioSource;

  
    public void StopAudioSource()
    {
        audioSource.Stop();
    }

    public IEnumerator SayThinkingSoundAfterPlayerTalked()      //Gets called in Whisper.cs after the user stops talking (context.cancelled)
    {
        yield return new WaitForSeconds(0.2f);
        PickThinkingSoundToPlay();
    }

    private AudioClip PickThinkingSoundToPlay()
    {

        int arrayThinkingSoundsMax = currentNpcThinkingSoundsArray.Length;
        int pickedThinkingSoundToPlay = Random.Range(0, arrayThinkingSoundsMax);
        return currentNpcThinkingSoundsArray[pickedThinkingSoundToPlay];

    }

    public IEnumerator PlayHmmThinkingSound(float timeTillPlaySound)
    {
        yield return new WaitForSeconds(timeTillPlaySound);

        audioSource.clip = PickThinkingSoundToPlay();
        audioSource.Play();
    }
}