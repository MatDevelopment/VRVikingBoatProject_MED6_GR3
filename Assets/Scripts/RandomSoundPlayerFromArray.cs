using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSoundPlayerFromArray : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] sounds;

    public float minInterval = 2f;
    public float maxInterval = 5f;

    private float randomInterval;
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();

        StartCoroutine(PlayCryingSoundWithInterval());
    }

    IEnumerator PlayCryingSoundWithInterval()
    {

        if (sounds.Length > 0)
        {
            randomInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomInterval);

            int randomIndex = Random.Range(0, sounds.Length);
            audioSource.clip = sounds[randomIndex];
            audioSource.Play();


            yield return new WaitForSeconds(audioSource.clip.length);

            StartCoroutine(PlayCryingSoundWithInterval());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
