using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LLMversionPlaying : MonoBehaviour
{

    public bool LLMversionIsPlaying;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    
}
