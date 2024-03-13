using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; //Skaber et array kaldet sounds som indeholder alle sounds
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();     //Tager fat i Audio Source komponenten på et gameObject
            s.source.clip = s.clip;         //Sætter audio klippet fra audio source component til s.clip

            s.source.volume = s.volume;     //Sætter volume fra audio source component til s.volume
            s.source.pitch = s.pitch;       //Sætter pitch fra audio source til s.pitch
        }
    }

    public void Play (string name)       //Tilføjer en måde at spille en specifik sound ud fra den string name
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);     //Finder den sound i 'sounds' array som har det samme navn som string name (i denne void) og opbevarer den fundne sound i variablen 's'
        s.source.Play();
    }
}
