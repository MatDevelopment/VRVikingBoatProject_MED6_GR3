using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSoundPlayer : MonoBehaviour
{
    [Header ("Game Objects")]
    [SerializeField] private GameObject Boat;
    [SerializeField] private GameObject Farm;
    private float DistanceBoatToFarm;
    float counterIndex = 0.0f;

    [Header ("Audio")]
    public AudioSource AnimalAudioSource;
    public AudioClip[] AnimalSounds = new AudioClip [3];

    // IEnumerator PlayChickenSound()
    // {
    //     AnimalAudioSource.PlayOneShot(AnimalSounds[0]);
    //     yield return null;
    // }

    // IEnumerator PlayRoosterSound()
    // {
    //     AnimalAudioSource.PlayOneShot(AnimalSounds[1]);
    //     yield return null;
    // }

    // IEnumerator PlayCowSound()
    // {
    //     AnimalAudioSource.PlayOneShot(AnimalSounds[2]);
    //     yield return null;
    // }

    // IEnumerator PlaySheepSound()
    // {
    //     AnimalAudioSource.PlayOneShot(AnimalSounds[3]);
    //     yield return null;
    // }
    
    void Update()
    {
        DistanceBoatToFarm = Vector3.Distance(Boat.transform.position, Farm.transform.position);

        if (DistanceBoatToFarm < 125)
        {
            counterIndex += Time.deltaTime;

            while (counterIndex >= 12.0f)
            {
                AudioClip randomClip = AnimalSounds[Random.Range(0, AnimalSounds.Length)];
                AnimalAudioSource.PlayOneShot(randomClip);
                Debug.Log("Playing Animal Sound");
                counterIndex = 0.0f;
            }
        }
    }
}
