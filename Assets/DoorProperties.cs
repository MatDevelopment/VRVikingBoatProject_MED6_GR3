using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorProperties : MonoBehaviour
{
    public LevelChanger levelChanger;
    // Start is called before the first frame update
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorOpeningClosingSound;
    
    void Awake()
    {
        levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (levelChanger.Scene1Active == true)
        {
            if (levelChanger.WoodStacked == true && levelChanger.WoodChopped == true)
            {
                doorAudioSource.clip = doorOpeningClosingSound;
                doorAudioSource.Play();
                levelChanger.OpeningDoor = true;
            }
        }

        if (levelChanger.Scene2Active == true)
        {
            if (levelChanger.AllItemGathered == true)
            {
                doorAudioSource.clip = doorOpeningClosingSound;
                doorAudioSource.Play();
                levelChanger.OpeningDoor = true;
            }
        }
        
    }
}
