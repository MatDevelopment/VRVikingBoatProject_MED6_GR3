using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayCryingAudio : MonoBehaviour
{
    private AudioSource cryingAudioSource;
    [SerializeField] private AudioClip[] arrayCryingSounds;

    private int cryingSoundsMax;
    private int pickedCryingSound;

    private int randomInterval;
    
    // Start is called before the first frame update
    void Start()
    {
        cryingAudioSource = gameObject.GetComponent<AudioSource>();

        StartCoroutine(PlayCryingSoundWithInterval());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayCryingSoundWithInterval()
    {
        
        if (arrayCryingSounds.Length > 0)
        {
            randomInterval = Random.Range(8, 13);
            yield return new WaitForSeconds(randomInterval);
            //arrayConversationSoundsMax = arrayNPCsounds.Length;
            pickedCryingSound = Random.Range(0, cryingSoundsMax);
            cryingAudioSource.clip = arrayCryingSounds[pickedCryingSound];
            
            cryingAudioSource.Play();

            yield return new WaitForSeconds(cryingAudioSource.clip.length);
            
            StartCoroutine(PlayCryingSoundWithInterval());
        }
        
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
