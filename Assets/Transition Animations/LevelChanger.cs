using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelChanger : MonoBehaviour
{
    // Call for animator object in Unity (insert the animator called 'LevelChanger' into inspector)
    public Animator animator;

    // Variable for loading certain level (based on level index)
    private int levelToLoad;
    
    // bool to check if you are playing the AI version of the experience (AI or scripted dialogue version)
    //public static bool LLM_VersionPlaying;
    //[SerializeField] public bool SetLLM_VersionPlaying;
    [SerializeField] private LLMversionPlaying LLMversionPlayingScript;
    [SerializeField] private ChatTest chatTestScript;
    
    // bool to unlock test conditions
    [Header ("Test Load Scene Function:")]
    [SerializeField] bool ReloadSceneTest = true;
    [SerializeField] bool NewSceneTest = false;

    //Determine current scene
        [Header ("Active Scene Bool:")]
        [SerializeField] public bool IntroSceneActive = true;
        [SerializeField] public bool Scene1Active = false;
        [SerializeField] public bool Scene2Active = false;
        [SerializeField] public bool Scene3Active = false;
        [SerializeField] public bool Scene4Active = false;

        [SerializeField] public bool disableNPCdialogueStarters = false;
        
    // Button press to start definition
        [Header ("Button Reference:")]
        [SerializeField] private InputActionReference buttonPressToStart = null;

            // Scene 1
            // Catch goal bools from other script
            [Header ("Test Scene 1 goals:")]
            [SerializeField] public bool WoodChopped;
            [SerializeField] public bool WoodStacked;
            private bool woodStackedPromptSent;
            [HideInInspector] public bool OpeningDoor;

            // Scene 2
            // Catch goal bools from other script
            [Header ("Test Scene 2 goals:")]
            [SerializeField] public bool AllItemGathered;

            // Scene 3 
            // Catch goal bools from other script
            [Header ("Test Scene 3 goals:")]
            [SerializeField] public bool StonesPlaced;
            [SerializeField] public bool WoodPlacedOnPyre;
            [FormerlySerializedAs("TorchLit")] [SerializeField] public bool FuneralPyreLit;
            
            public int countChoppedWood = 0;
            public int countStackedWood = 0;
            private int countStonesPlaced;
            private int countWoodPlaced;

            [SerializeField] private AudioSource ambienceMusicAudioSource;

            public static Vector3 xrOriginRig_Transform;
            public static Quaternion xrOriginRig_Rotation;

            [SerializeField] private GameObject xrOriginGameobject;
            [SerializeField] private NPCInteractorScript erikInteractorScript;

            [SerializeField] private Transform set_XROriginToThisTransform;

    void Awake()
    {
        LLMversionPlayingScript = GameObject.FindWithTag("LLMversionGameObject").GetComponent<LLMversionPlaying>();
        
        if (IntroSceneActive == true)
        {
            buttonPressToStart.action.Enable();
            buttonPressToStart.action.performed += StartExperienceOnButtonPress;
        }
        
        // Managing bools when loading LevelChanger object in new scenes.
        if (Scene1Active == true)
        {
            //SetXROriginPosRotation();
            SetXROriginToTransformInMoveableArea();
            WoodStacked = false;
            WoodChopped = false;
            OpeningDoor = false;
            Debug.Log("Scene 1 Is Now Active!");
            
        }

        if (Scene2Active == true)
        {
            //SetXROriginPosRotation();
            SetXROriginToTransformInMoveableArea();
            OpeningDoor = false;
            AllItemGathered = false;
            Debug.Log("Scene 2 Is Now Active!");
        }

        if (Scene3Active == true)
        {
            //SetXROriginPosRotation();
            SetXROriginToTransformInMoveableArea();
            StonesPlaced = false;
            WoodPlacedOnPyre = false;
            Debug.Log("Scene 3 Is Now Active!");
        }
        
        if (Scene4Active == true)
        {
            //SetXROriginPosRotation();
            SetXROriginToTransformInMoveableArea();
            FuneralPyreLit = false;
            Debug.Log("Scene 4 Is Now Active!");
        }
    }

    public void StartExperienceOnButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed && IntroSceneActive == true)
        {
            buttonPressToStart.action.performed -= StartExperienceOnButtonPress;
            buttonPressToStart.action.Disable();
            FadeToNextLevel();
            IntroSceneActive = false;
            Scene1Active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Testing();

        // Conditions for interactive experience
        
        if (Scene1Active == true)
        {
            if (OpeningDoor == true)
            {
                FadeToNextLevel();
                Scene2Active = true;
                Scene1Active = false;
            }
        }

        if (Scene2Active == true)
        {
            if (OpeningDoor == true)
            {
                FadeToNextLevel();
                Scene3Active = true;
                Scene2Active = false;
            }
        }

        if (Scene3Active == true)
        {
            if(StonesPlaced == true && WoodPlacedOnPyre == true)
            {
                FadeToNextLevel();
                Scene4Active = true;
                Scene3Active = false;
            }
        }

        if (Scene4Active == true)
        {
            if (FuneralPyreLit == true)
            {
                //Add coroutine her som først fader til næste scene efter lidt tid så man kan nå at høre musikken osv. ved slutningen af Thorsten's begravelse
                StartCoroutine(EndSceneAfterWaitTime());
                StartCoroutine(PlayMusicAfterWaitTime());
                Scene4Active = false;
                FuneralPyreLit = false;
                disableNPCdialogueStarters = true;
                Debug.Log("Experience Is Over!");
            }
        }

    }

    public void FadeToNextLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Method for running animation and setting the level index to load.
    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    // Function used as animation event
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void Testing()
    {
        // These conditions only apply when testing. 
        if (Input.GetMouseButtonDown(0) && ReloadSceneTest == true && NewSceneTest == false)
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Reloading Same Scene!");
        }

        if (Input.GetMouseButtonDown(0) && NewSceneTest == true && ReloadSceneTest == false)
        {
            FadeToNextLevel();
            Debug.Log("Entering New Scene!");
        }

        else if (NewSceneTest == true && ReloadSceneTest == true && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Can't Reload And Start New Scene At The Same Time!");
        }

    }
    
    public void CountWooodChopped()
    {
        countChoppedWood++;
        Debug.Log("Wood chopped: " + countChoppedWood);
        
        erikInteractorScript.InformAndInitiateNpcTalk("The Traveller has now chopped all the wood needed for Thorsten's funeral pyre. Tell the Traveller that they did a good job.\n");
        //WoodChopped = true;
    }

    public void CountWoodStacked()
    {
        Debug.Log("Wood stacked: " + countStackedWood);
        if (countStackedWood >= 2 && WoodStacked == false)
        {
            if (LLMversionPlayingScript.LLMversionIsPlaying == true && woodStackedPromptSent == false)
            {
                chatTestScript.AddSystemInstructionToChatLog("The Traveller has now stacked all the wood needed for Thorsten's funeral pyre unto the wagon nearby. The NPC, Erik, will be with the Traveller inside the house of Thorsten and Ingrid AFTER the Traveller has entered the house. Keep this in mind.\n");

                if (erikInteractorScript.NPCaudioSource.isPlaying == false)
                {
                    erikInteractorScript.InformAndInitiateNpcTalk("The Traveller has now stacked all the wood needed for Thorsten's funeral pyre unto the wagon nearby. The NPC, Erik, will be with the Traveller inside the house of Thorsten and Ingrid AFTER the Traveller has entered the house. Convey this to the Traveller.\n");
                }

                woodStackedPromptSent = true;
            }
            WoodStacked = true;
        }
    }
    
    
    public void CountStonePlacements()
    {
        countStonesPlaced++;
        Debug.Log("Stones placed: " + countStonesPlaced);

        if (countStonesPlaced == 12 && StonesPlaced == false)
        {
            if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                chatTestScript.AddSystemInstructionToChatLog("The Traveller have now placed the stones in the stone formation of a longship around Thorsten's funeral pyre. Keep this in mind.\n");

                if (erikInteractorScript.NPCaudioSource.isPlaying == false)
                {
                    erikInteractorScript.InformAndInitiateNpcTalk("The Traveller have now placed the stones in the stone formation of a longship around Thorsten's funeral pyre. Tell the Traveller that they did a good job.\n");
                }
            }
            StonesPlaced = true;
        }
    }
    
    public void CountWooodPlacements()
    {
        countWoodPlaced++;
        Debug.Log("Wood placed: " + countWoodPlaced);
        
        if (countWoodPlaced == 7 && WoodPlacedOnPyre == false)
        {
            if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                chatTestScript.AddSystemInstructionToChatLog("The Traveller have now placed all the wood pieces unto Thorsten's funeral pyre. Keep this in mind.\n");
                
                if (erikInteractorScript.NPCaudioSource.isPlaying == false)
                {
                    erikInteractorScript.InformAndInitiateNpcTalk("The Traveller have now placed all the wood pieces unto Thorsten's funeral pyre. Tell the Traveller that they did a good job.\n");
                }
            }
            WoodPlacedOnPyre = true;
        }
    }

    public void DecrementStoneCount()
    {
        countStonesPlaced--;
    }
    
    public void DecrementWoodCount()
    {
        countWoodPlaced--;
    }
    
    /*IEnumerator CountUpGazeTime()
    {
        
        currentGazeTime += countingGazeTime;
        yield return new WaitUntil(lookingAtNpc);
        
    }*/

    private IEnumerator PlayMusicAfterWaitTime()
    {
        yield return new WaitForSeconds(1f);
        ambienceMusicAudioSource.Play();
    }
    
    private IEnumerator EndSceneAfterWaitTime()
    {
        yield return new WaitForSeconds(21);
        FadeToNextLevel();
    }
    
    private void SetXROriginPosRotation()
    {
        xrOriginGameobject.transform.localPosition = xrOriginRig_Transform;
        xrOriginGameobject.transform.localRotation = xrOriginRig_Rotation;
    }

    private void SaveXR0riginPosRotation()
    {
        xrOriginRig_Transform = xrOriginGameobject.transform.localPosition;
        xrOriginRig_Rotation = xrOriginGameobject.transform.localRotation;
    }

    private void SetXROriginToTransformInMoveableArea()
    {
        xrOriginGameobject.transform.localPosition = set_XROriginToThisTransform.localPosition;
        xrOriginGameobject.transform.localRotation = set_XROriginToThisTransform.localRotation;
    }

    /*private void OnDestroy()
    {
        //SaveXR0riginPosRotation();
    }*/
}
