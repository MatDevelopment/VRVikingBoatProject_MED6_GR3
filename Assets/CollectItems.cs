using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

public class CollectItems : MonoBehaviour
{
    public LevelChanger levelChanger;
    public LLMversionPlaying LLMversionPlayingScript;
    public NPCInteractorScript npcInteractorScript;
    public TextToSpeech textToSpeechScript;
    public Whisper whisperScript;

    public GameObject Door;
    public GameObject DoorOpen;
        
    public int itemsCollected = 0;

    private string countPrompt_1 = "The Traveller just chose the first personal belonging of Thorsten for Thorsten's funeral. The Traveller needs to choose one more personal belonging. Convey this to the Traveller without describing the item.";
    private string countPrompt_2 = "The Traveller just chose the last personal belonging of Thorsten for Thorsten's funeral. The Traveller is now ready to go out of the door of the house and go to Thorsten's funeral site. Convey this to the Traveller without describing the item.";

    void Awake()
    {
        levelChanger = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
        LLMversionPlayingScript = GameObject.FindWithTag("LLMversionGameObject").GetComponent<LLMversionPlaying>();
    }

    private void Start()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PersonalItem"))
        {
            itemsCollected += 1;  

            if (itemsCollected == 2)
            {
                Debug.Log("Door Open"+ itemsCollected);
                Door.SetActive(false);
                DoorOpen.SetActive(true);
                levelChanger.AllItemGathered = true;
            }
        }
        

        if (other.gameObject.CompareTag("PersonalItem") && textToSpeechScript.isGeneratingSpeech == false && whisperScript.isDoneTalking == true && whisperScript.isRecording == false && textToSpeechScript.audioSource.isPlaying == false && LLMversionPlayingScript.LLMversionIsPlaying == true)
        {  
            switch (itemsCollected)
            {
                case 1:         //If the player has put a single personal belonging in the basket, then a prompt will be sent to the API instructing it to count the amount of picked items.
                    npcInteractorScript.InformAndInitiateNpcTalk(countPrompt_1);
                    break;
                case 2:
                    npcInteractorScript.InformAndInitiateNpcTalk(countPrompt_2);
                    
                    break;
            }

            
        }
        
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PersonalItem"))
        {
            itemsCollected -= 1;
        }
    }
}
