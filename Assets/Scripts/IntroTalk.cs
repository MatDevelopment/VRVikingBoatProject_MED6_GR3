using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTalk : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    public AudioClip audioClip;

    [SerializeField]
    float DelayInSeconds = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenPlay(DelayInSeconds));
    }

    IEnumerator WaitThenPlay(float Wait)
    {
        yield return new WaitForSeconds(Wait);

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
