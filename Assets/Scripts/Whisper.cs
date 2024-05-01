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
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Net.NetworkInformation;

namespace OpenAI
{
    public class Whisper : MonoBehaviour //Reponsible for converting a voice recording into text.
    {
        [SerializeField] private APIStatus apiStatus;
        [SerializeField] private APICallTimeManager apiCallTimeManager;
        [SerializeField] private Dropdown dropdown;
        [SerializeField] private ChatTest chatTest;
        //[SerializeField] private TextToSpeech textToSpeechScript;
        [SerializeField] private NpcAnimationStateController npcAnimationStateController;
        [SerializeField] private MicInputDetection MicInputDetectionScript;
        [SerializeField] private NPCInteractorScript npcInteractorScript;
        [SerializeField] private TTSManager ttsManagerScript;
        [SerializeField] private GestureManager gestureManagerNew;
        [SerializeField] private ChoosePromptGesture choosePromptGestureScript;
        [SerializeField] private PointingManager pointingManagerScript;
        [SerializeField] private NewDataLogManager newDataLogManager;
        private NPCSoundBitPlayer nPCSoundBitPlayer;

        [SerializeField] private Image progress;
        [SerializeField] private InputActionReference buttonHoldReference = null;

        private readonly string deleteThisObligatedStringFromWhisperCreditString = "Subs by www.zeoranger.co.uk";
        private readonly string fileName = "output.wav";
        private readonly int duration = 12;
        public string npcResponse;
        public string uneditedNPCResponse;
        public string userRecordingString;

        public bool contextIsPerformed = false;

        public float timeToInterruptTalk = 0.05f;

        private OpenAIApi openai = new OpenAIApi();

        private void Start()
        {
            apiStatus = FindObjectOfType<APIStatus>();
            apiCallTimeManager = FindObjectOfType<APICallTimeManager>();
            gestureManagerNew = FindAnyObjectByType<GestureManager>();
            nPCSoundBitPlayer = FindObjectOfType<NPCSoundBitPlayer>();
            npcAnimationStateController = FindAnyObjectByType<NpcAnimationStateController>();
        }

        private void ChangeMicrophone(int index)
        {
            PlayerPrefs.SetInt("user-mic-device-index", index);
        }

        public async void TranscribeRecordedAudio()
        {

            if (apiStatus.isTranscribing == false && apiStatus.isTalking == false)
            {
                apiStatus.isTranscribing = true;
                // Laver dropdown UI så vi kan vælge mic selv.
                //var index = PlayerPrefs.GetInt("user-mic-device-index");
                //clip = Microphone.Start(dropdown.options[index].text, false, duration, 44100);
                //contextIsPerformed = true;
            }

            if (apiStatus.isTranscribing == true)
            {
                Debug.Log("Starting tranccription...");

                byte[] data = SaveWav.Save(fileName, MicInputDetectionScript.userSpeechClip);
                //SaveWav.TrimSilence(new List<float>())

                var request = new CreateAudioTranscriptionsRequest
                {
                    FileData = new FileData() { Data = data, Name = "audio.wav" },
                    Model = "whisper-1",
                    Language = "da"
                };

                Stopwatch stopwatch = Stopwatch.StartNew();
                apiStatus.isTranscribing = true;

                var result = await openai.CreateAudioTranscription(request);

                stopwatch.Stop(); // Stop measuring time
                apiStatus.isTranscribing = false;

                apiCallTimeManager.AddCallDuration_SpeechToText(stopwatch.Elapsed.TotalSeconds);

                //! tjek Mathias om vi kan slette det her
                /*if ((res.Text.Contains("Hello") && res.Text.Length < 11) || (res.Text.Contains("Hi") && res.Text.Length < 11))      //If the user's input to the NPC is Hello or Hi, and what they say is less than 11 characters long, including spaces, 
                {                                                                                       //then the NPC only says "Hmm" and not "Let me think", which is AN audio clip contained within the sound clip array with
                    StartCoroutine(PlayHmmThinkingSound(textToSpeechScript.audioSource));         //thinking sounds. Let me think would be an unusual response to someone saying hello to you.
                }*/

                if (result.Text.Contains("Hello") || result.Text.Contains("Hi"))
                {

                    StartCoroutine(nPCSoundBitPlayer.PlayHmmThinkingSound(0.2f));
                }
                else if ((result.Text.Length >= 12 && !result.Text.Contains("Hello")) || (result.Text.Length >= 12 && !result.Text.Contains("Hi")))          //If what the user says is longer than 12 characters (including spaces), then the current NPC will say a thinking sound like "Hmm" or "Hmm, let me think" or "Hmm let me think for a second".
                {
                    StartCoroutine(nPCSoundBitPlayer.SayThinkingSoundAfterPlayerTalked());
                }
                else if (result.Text.Contains(deleteThisObligatedStringFromWhisperCreditString))
                {
                    result.Text = result.Text.Replace(deleteThisObligatedStringFromWhisperCreditString, "");
                }
                else if (result.Text == deleteThisObligatedStringFromWhisperCreditString)
                {
                    result.Text = "";
                }

                userRecordingString = result.Text;

                MicInputDetectionScript.loudness = 0;
             


                if (string.IsNullOrEmpty(result.Text) == false || string.IsNullOrWhiteSpace(result.Text) == false)
                {
                    Debug.Log("Recorded message: " + userRecordingString + gestureManagerNew.PullLatestGestureCombination());

                    newDataLogManager.SendStringToDataLogger("User: " + userRecordingString);
                    newDataLogManager.TotalUserPrompts += 1;
                    chatTest.AddPlayerInputToChatLog(userRecordingString + gestureManagerNew.PullLatestGestureCombination());
                    choosePromptGestureScript.ClearDictionaryOfPointedItems();
                    pointingManagerScript.rightHandLastSelected = "";
                    pointingManagerScript.leftHandLastSelected = "";

                   

                    string chatGptResponse = await chatTest.SendRequestToChatGpt(chatTest.messages);

                    npcResponse = chatGptResponse;

                    Debug.Log("NPC Response: " + npcResponse);

                    uneditedNPCResponse = npcResponse;
                    // newDataLogManager.SendStringToDataLogger("Erik: " + npcResponse);
                    newDataLogManager.TotalErikResponses += 1;
                    chatTest.AddNpcResponseToChatLog(npcResponse);

                    //Check for current NPC emotion in order to play animation
                    foreach (string primaryEmotion in npcInteractorScript.npcPrimaryEmotions)
                    {
                        npcInteractorScript.CheckErikPrimaryEmotion(primaryEmotion);
                    }


                    foreach (string action in npcInteractorScript.npcActionStrings)
                    {

                        if (npcResponse.Contains(action))
                        {
                            string responseTillActionString = AnimationDelayCalculator.CreateStringUntilKeyword(inputString: npcResponse, actionToCheck: action);

                            int punctuationsCount = AnimationDelayCalculator.CountCharsUsingLinqCount(responseTillActionString, '.'); //Counts amount of punctuations in responseTillActionString

                            int wordInStringCount = AnimationDelayCalculator.CountWordsInString(responseTillActionString);    //Counts the amount of words in responseTillActionString

                            float estimatedTimeTillAction = AnimationDelayCalculator.EstimatedTimeTillAction(wordCount: wordInStringCount,
                                                        wordWeight: 0.15f, punctuationCount: punctuationsCount, punctuationWeight: 1f);

                            Debug.Log("ActionString: " + responseTillActionString + " --" +
                              "Punctuations: " + punctuationsCount + " --" +
                              "Word count: " + wordInStringCount + " --" +
                         "ETA of action: " + estimatedTimeTillAction);

                            int startIndexAction = npcResponse.IndexOf(action);     //Finds the starting index of the action keyword in the ChatGPT response

                            responseTillActionString = "";

                            //Man kan godt løbe ind i problemer med Remove her, og ved ikke helt hvorfor.
                            //Tror det har noget at gøre med at Length starter med at tælle til 1, men indeces starter fra 0.
                            npcResponse = npcResponse.Remove(startIndexAction, action.Length);      //Removes the action keyword from ChatGPT's response plus the following white space

                            Debug.Log("New response after removing action:   " + npcResponse);
                            npcAnimationStateController.AnimateErik(action, estimatedTimeTillAction);
                        }
                    }

                    npcInteractorScript.AnimateFacialExpressionResponse_Erik(npcInteractorScript.npcEmotion, 0.5f);


                    Debug.Log("Edited response: " + npcResponse);

                    result.Text = result.Text.Replace(result.Text, "");
                    userRecordingString = result.Text;

                    //OpenAI TTS (Danish):
                    ttsManagerScript.SynthesizeAndPlay(npcResponse); //https://github.com/mapluisch/OpenAI-Text-To-Speech-for-Unity?tab=readme-ov-file

                    // newDataLogManager.SendStringToDataLogger("Erik: " + "[API: " + apiCallTimeManager.GetLastCombinedCallTime() + "] " + uneditedNPCResponse);

                }

                //contextIsPerformed = false;
            }
        }

    }
}