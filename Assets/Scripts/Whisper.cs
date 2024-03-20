using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace OpenAI
{
    public class Whisper : MonoBehaviour
    {
        //[SerializeField] private Button recordButton;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private ChatTest chatTest;
        [SerializeField] private TextToSpeech textToSpeechScript;
        //[SerializeField] private LevelChanger levelChangerScript;
        //[SerializeField] private LLMversionPlaying LLMversionPlayingScript;
        [SerializeField] private MicInputDetection MicInputDetectionScript;
        [SerializeField] private NPCInteractorScript npcInteractorScript;
        [SerializeField] private TTSManager ttsManagerScript;
        [SerializeField] private GestureManagerNew gestureManagerNew;
        
        [SerializeField] private Image progress;
        [SerializeField] private InputActionReference buttonHoldReference = null;

        private readonly string deleteThisObligatedStringFromWhisperCreditString = "Subs by www.zeoranger.co.uk";
        private readonly string fileName = "output.wav";
        private readonly int duration = 12;
        public string npcResponse;
        public string userRecordingString;

        private AudioClip clip;
        [FormerlySerializedAs("hmmThinkingSound")] [SerializeField] private AudioClip ErikHmmSound;
        /*[SerializeField] private AudioClip ArneHmmSound;
        [SerializeField] private AudioClip FridaHmmSound;
        [SerializeField] private AudioClip IngridHmmSound;*/
        
        public bool isRecording = false;
        public bool isDoneTalking = true;
        public bool contextIsPerformed = false;

        private float time;
        public float timeToInterruptTalk = 0.05f;

        private OpenAIApi openai = new OpenAIApi();

        private void Awake()
        {
            //LLMversionPlayingScript = GameObject.FindWithTag("LLMversionGameObject").GetComponent<LLMversionPlaying>();
        }

        /*private void OnDestroy()
        {
            if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                /*buttonHoldReference.action.Disable();
                buttonHoldReference.action.started -= StartRecording;
                buttonHoldReference.action.performed -= StartRecording;
                buttonHoldReference.action.canceled -= StartRecording;#1#
            }
            
        }*/

        private void Start()
        {
            //levelChangerScript = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
            
            /*if (LLMversionPlayingScript.LLMversionIsPlaying == true)
            {
                buttonHoldReference.action.Enable();
                buttonHoldReference.action.performed += StartRecording;
                buttonHoldReference.action.canceled += StartRecording;
            }*/

            /*foreach (var device in Microphone.devices)
            {
                dropdown.options.Add(new Dropdown.OptionData(device));
            }
            //recordButton.onClick.AddListener(StartRecording);
            dropdown.onValueChanged.AddListener(ChangeMicrophone);
            
            var index = PlayerPrefs.GetInt("user-mic-device-index");
            dropdown.SetValueWithoutNotify(index);*/
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }
        
        public async void StartRecording()
        {
            if(npcInteractorScript.erikSpeakable)
            {
                if(isRecording == false && isDoneTalking == true)
                {
                    //StartCoroutine(InterruptNpcTalkingAfterDuration(timeToInterruptTalk));
                    Debug.Log("Start recording...");
                    isRecording = true;
                    
                    //var index = PlayerPrefs.GetInt("user-mic-device-index");
                    //clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
                    //contextIsPerformed = true;
                }
                            
                if (isRecording == true)
                {
                    //time = 0;
                    //progress.fillAmount = 1;
                    Debug.Log("Stop recording...");
                    
                    //Microphone.End(MicInputDetectionScript.microphoneName);
                    //Microphone.End(null);
                                
                    byte[] data = SaveWav.Save(fileName, MicInputDetectionScript.userSpeechClip);
                    //SaveWav.TrimSilence(new List<float>())
                            
                    var req = new CreateAudioTranscriptionsRequest
                    {
                        FileData = new FileData() {Data = data, Name = "audio.wav"},
                        // File = Application.persistentDataPath + "/" + fileName,
                        //Prompt = "The transcript is the dialogue of a person speaking english with a danish accent.",
                        Model = "whisper-1",
                        Language = "da"
                    };
                    var res = await openai.CreateAudioTranscription(req);

                    /*if ((res.Text.Contains("Hello") && res.Text.Length < 11) || (res.Text.Contains("Hi") && res.Text.Length < 11))      //If the user's input to the NPC is Hello or Hi, and what they say is less than 11 characters long, including spaces, 
                    {                                                                                       //then the NPC only says "Hmm" and not "Let me think", which is AN audio clip contained within the sound clip array with
                        StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource));         //thinking sounds. Let me think would be an unusual response to someone saying hello to you.
                    }*/
                                
                    if(res.Text.Contains("Hello") || res.Text.Contains("Hi"))
                    {
                        StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource, 0.2f, ErikHmmSound));
                    }
                    else if ((res.Text.Length >= 12 && !res.Text.Contains("Hello")) || (res.Text.Length >= 12 && !res.Text.Contains("Hi")))          //If what the user says is longer than 12 characters (including spaces), then the current NPC will say a thinking sound like "Hmm" or "Hmm, let me think" or "Hmm let me think for a second".
                    {
                        StartCoroutine(SayThinkingSoundAfterPlayerTalked());
                    }
                    else if (res.Text.Contains(deleteThisObligatedStringFromWhisperCreditString))
                    {
                        res.Text = res.Text.Replace(deleteThisObligatedStringFromWhisperCreditString, "");
                    }
                    else if (res.Text == deleteThisObligatedStringFromWhisperCreditString)
                    {
                        res.Text = "";
                    }

                    userRecordingString = res.Text;

                    isRecording = false;
                                
                    if (string.IsNullOrEmpty(res.Text) == false || string.IsNullOrWhiteSpace(res.Text) == false)
                    {
                        Debug.Log("Recording: " + userRecordingString);
                                    
                        chatTest.AddPlayerInputToChatLog(userRecordingString);
                        isDoneTalking = false;
                        // Debug.Log($"isDoneTalking: {isDoneTalking}");
                        string chatGptResponse = await chatTest.SendRequestToChatGpt(chatTest.messages);
                        
                        npcResponse = chatGptResponse;
                        
                        chatTest.AddNpcResponseToChatLog(chatGptResponse);
                        
                        //Check for current NPC emotion in order to play animation
                        foreach (string primaryEmotion in npcInteractorScript.npcPrimaryEmotions)
                        {
                            npcInteractorScript.CheckErikPrimaryEmotion(primaryEmotion);
                        }
                        /*foreach (var secondaryEmotion in npcInteractorScript.npcSecondaryEmotions)
                        {
                            npcInteractorScript.AnimateOnSecondaryEmotion_Erik(secondaryEmotion);
                        }*/

                        foreach (string action in npcInteractorScript.npcActionStrings)
                        {
                            if (chatGptResponse.Contains(action))
                            {
                                string responseTillActionString = CreateStringUntilKeyword(inputString: chatGptResponse, actionToCheck: action);
                                Debug.Log("ActionString: " + responseTillActionString);
                                
                                int punctuationsCount = CountCharsUsingLinqCount(responseTillActionString, '.'); //Counts amount of punctuations in chatGptResponse
                                Debug.Log("Punctuations: " + punctuationsCount);
                                
                                int wordInStringCount = CountWordsInString(responseTillActionString);    //Counts the amount of words in chatGptResponse
                                Debug.Log("Word count: " + wordInStringCount);
                                
                                float estimatedTimeTillAction = EstimatedTimeTillAction(wordCount: wordInStringCount,
                                                            wordWeight: 0.2f, punctuationCount: punctuationsCount, punctuationWeight: 1f);
                                
                                Debug.Log("ETA of action: " + estimatedTimeTillAction);
                                
                                npcInteractorScript.AnimateBodyResponse_Erik(action, estimatedTimeTillAction);
                            }
                        }


                        /*int primaryEmotionInt = npcInteractorScript.npcEmotion.Length;
                        int secondEmotionInt = npcInteractorScript.npcSecondaryEmotion.Length;

                        int indexesToRemove = primaryEmotionInt + secondEmotionInt + 1;*/
                        npcInteractorScript.AnimateFacialExpressionResponse_Erik(npcInteractorScript.npcEmotion, 0.5f);


                        Debug.Log(npcResponse);
                        //AWS POLLY (English):
                        //textToSpeechScript.MakeAudioRequest(npcResponse);
                        
                        //OpenAI TTS (Danish):
                        ttsManagerScript.SynthesizeAndPlay(chatGptResponse); //https://github.com/mapluisch/OpenAI-Text-To-Speech-for-Unity?tab=readme-ov-file
                        isDoneTalking = true;
                        // Debug.Log($"isDoneTalking: {isDoneTalking}");
                        res.Text = res.Text.Replace(res.Text, "");
                        userRecordingString = res.Text;
                    }
                                
                    //contextIsPerformed = false;
                }
            }
            
        }
        
        /*private void Update()
        {
            if (isRecording)
            {
                time += Time.deltaTime;
                progress.fillAmount = time / duration;      //Meant for showing how much time you have left to talk in through a fill amount of a UI progress bar etc. Not being used currently.
            }
            
            if(time >= duration)
            {
                time = 0;
                progress.fillAmount = 0;        //Meant for showing how much time you have left to talk in through a fill amount of a UI progress bar etc. Not being used currently.
            }
        }*/

        public IEnumerator InterruptNpcTalkingAfterDuration(float interruptDuration)
        {
            yield return new WaitForSeconds(interruptDuration);
            
            textToSpeechScript.audioSource.Stop();
            StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource, interruptDuration, ErikHmmSound));
        }
        
        
        public IEnumerator SayThinkingSoundAfterPlayerTalked()      //Gets called in Whisper.cs after the user stops talking (context.cancelled)
        {
            yield return new WaitForSeconds(0.2f);
            PickThinkingSoundToPlay(textToSpeechScript.audioSource);
        }
    
        private void PickThinkingSoundToPlay(AudioSource audioSourceToPlayOn)
        {
            if (chatTest.currentNpcThinkingSoundsArray.Length > 0)
            {
                int arrayThinkingSoundsMax = chatTest.currentNpcThinkingSoundsArray.Length;
                int pickedThinkingSoundToPlay = Random.Range(0, arrayThinkingSoundsMax);
                audioSourceToPlayOn.clip = chatTest.currentNpcThinkingSoundsArray[pickedThinkingSoundToPlay];
                
                audioSourceToPlayOn.Play();
            }
        }

        private IEnumerator PlayHmmThinkingSound(AudioSource audioSourceToPlayOn, float timeTillPlaySound, AudioClip thinkingSound)
        {
            yield return new WaitForSeconds(timeTillPlaySound);
            switch (chatTest.nameOfCurrentNPC)
            {
                case "Erik":
                    audioSourceToPlayOn.clip = thinkingSound;
                    break;
                default:
                    audioSourceToPlayOn.clip = ErikHmmSound;
                    break;
            }
            
            audioSourceToPlayOn.Play();
        }

        
        public string CreateStringUntilKeyword(string inputString, string actionToCheck)
        {
            string responseTillAction = inputString;
            
            int endCharIndex = inputString.IndexOf(actionToCheck);
            responseTillAction = inputString.Substring(0, endCharIndex - 1);
            
            /*foreach (string action in npcInteractorScript.npcActionStrings)
            {
                if (inputString.Contains(action))
                {
                    int endCharIndex = inputString.IndexOf(action);
                    responseTillAction = inputString.Substring(0, endCharIndex - 1);
                }
            }*/
            return responseTillAction;
        }
        
        
        public int CountCharsUsingLinqCount(string sourceString, char charToFind)
        {
            return sourceString.Count(t => t == charToFind);
        }

        
        public int CountWordsInString(string sourceString)
        {
            int NumberOfWords = sourceString.Split().Length;        //Counts the numbers of words in sourceString... apparently: https://stackoverflow.com/a/26794798
            
            return NumberOfWords;
        }

        private float EstimatedTimeTillAction(int punctuationCount, int wordCount, float punctuationWeight, float wordWeight)
        {
            //Punctuation time estimation
            float punctuationTime = punctuationCount * punctuationWeight;
            
            //Word time estimation
            float wordTime = (wordCount - 2) * wordWeight;      //Subtract 2 from wordcount cause ChatGPT adds two new lines which are counted as words.
            
            //Calculate estimated time till action by adding individual times.
            float estimatedTimeTillAction = punctuationTime + wordTime;

            if (estimatedTimeTillAction < 0)
            {
                estimatedTimeTillAction = 0;
            }
            
            return estimatedTimeTillAction;
        }

    }
}
