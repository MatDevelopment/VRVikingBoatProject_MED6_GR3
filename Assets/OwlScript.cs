using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlScript : MonoBehaviour
{
 public AudioClip[] soundArray;
    public float minInterval = 2f;
    public float maxInterval = 5f;

    private AudioSource audioSource;
    private float timer;

    void Start()
    {
        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();

        // Initialize the timer
        timer = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        // Update the timer
        timer -= Time.deltaTime;

        // Check if it's time to play a random sound
        if (timer <= 0f)
        {
            // Play a random sound from the array
            if (soundArray.Length > 0)
            {
                int randomIndex = Random.Range(0, soundArray.Length);
                audioSource.clip = soundArray[randomIndex];
                audioSource.Play();
            }

            // Reset the timer to a new random interval
            timer = Random.Range(minInterval, maxInterval);
        }
    }
}