using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindHowlVolume : MonoBehaviour
{

    public AudioSource windHowl;

    public float newVolume = 0.720f;
    
    // Start is called before the first frame update
    void Start()
    {
       windHowl.volume = newVolume; 
    }

  
}
