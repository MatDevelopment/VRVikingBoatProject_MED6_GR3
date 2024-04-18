using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReadyPlayerMe.AvatarCreator;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace OpenAI
{
    public class ChatTest : MonoBehaviour
    {
        public string nameOfCurrentNPC;
        [SerializeField] private NpcInfo npcInfo;
        [SerializeField] private WorldInfo worldInfo;

        [SerializeField] private NPCInteractorScript erikInteractorScript;
        [SerializeField] private NPCInteractorScript arneInteractorScript;
        [SerializeField] private NPCInteractorScript fridaInteractorScript;
        [SerializeField] private NPCInteractorScript ingridInteractorScript;
        [SerializeField] private APICallTimeManager apiCallTimeManager;
        [SerializeField] private TextToSpeech textToSpeech;

        //! tjek Mathias om vi kan slette det her
        //[SerializeField] private LLMversionPlaying LLMversionPlayingScript;


        public UnityEvent OnReplyReceived;
        
        //public bool isDone = true;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        public List<ChatMessage> messages = new List<ChatMessage>();

        public AudioClip[] currentNpcThinkingSoundsArray;

        private void Awake()
        {
            //! tjek Mathias om vi kan slette det her
            //LLMversionPlayingScript = GameObject.FindWithTag("LLMversionGameObject").GetComponent<LLMversionPlaying>();
        }

        private void Start()
        {
            apiCallTimeManager = FindObjectOfType<APICallTimeManager>();

            currentNpcThinkingSoundsArray = erikInteractorScript.arrayThinkingNPCsounds;
            textToSpeech.audioSource = erikInteractorScript.NPCaudioSource;
            textToSpeech.voiceID_name = erikInteractorScript.voiceIDNameThisNpc;
            messages = erikInteractorScript.ChatLogWithNPC;
        }
        
        public async Task<string> SendRequestToChatGpt(List<ChatMessage> combinedMessages)
        {
            CreateChatCompletionRequest request = new CreateChatCompletionRequest();
            request.Messages = combinedMessages;
            //request.Model = "gpt-3.5-turbo-16k-0613";
            request.Model = "gpt-4-1106-preview";
            request.Temperature = 0.5f;
            request.MaxTokens = 256;

            Stopwatch stopwatch = Stopwatch.StartNew();

            var response = await openai.CreateChatCompletion(request);

            stopwatch.Stop();

            apiCallTimeManager.AddCallDuration_ChatGPT(stopwatch.Elapsed.TotalSeconds);


            if (response.Choices != null && response.Choices.Count > 0)
            {
                var chatResponse = response.Choices[0].Message;

                return chatResponse.Content;
                
            }

            return null;
        }

        public void AddPlayerInputToChatLog(string playerInput)
        {
            var userMessage = new ChatMessage()
            {
                Role = "user",
                Content = playerInput
            };
            messages.Add(userMessage);

            //! tjek Mathias om vi kan slette det her
            switch (nameOfCurrentNPC)
            {
                case "Erik":
                    erikInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                case "Arne":
                    arneInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                case "Frida":
                    fridaInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                case "Ingrid":
                    ingridInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                default:
                    erikInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
            }
            
        }

        public void AddNpcResponseToChatLog(string npcResponse)
        {
            var assistantMessage = new ChatMessage()
            {
                Role = "assistant",
                Content = npcResponse
            };
            
            messages.Add(assistantMessage);

            //! tjek Mathias om vi kan slette det her
            switch (nameOfCurrentNPC)
            {
                case "Erik":
                    erikInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                /*case "Arne":
                    arneInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                case "Frida":
                    fridaInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                case "Ingrid":
                    ingridInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;*/
                default:
                    erikInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
            }

            //messages.Add(assistantMessage);
        }

        public void AddSystemInstructionToChatLog(string instruction)
        {
            
            var message = new ChatMessage()
            {
                Role = "system",
                Content = instruction
            };
            messages.Add(message);

            //! tjek Mathias om vi kan slette det her
            switch (nameOfCurrentNPC)
            {
                case "Erik":
                    erikInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                case "Arne":
                    arneInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                case "Frida":
                    fridaInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                case "Ingrid":
                    ingridInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                default:
                    erikInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                
            }
        }
        
    }
}
