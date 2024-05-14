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
        [SerializeField] private APIStatus apiStatus;
        [SerializeField] private NpcInfo npcInfo;
        [SerializeField] private WorldInfo worldInfo;

        [SerializeField] private NPCInteractorScript nPCInteractorScript;

        [SerializeField] private APICallTimeManager apiCallTimeManager;
        //[SerializeField] private TextToSpeech textToSpeech;
   
        public UnityEvent OnReplyReceived;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        public List<ChatMessage> messages = new List<ChatMessage>();

   
        private void Start()
        {
            apiCallTimeManager = FindObjectOfType<APICallTimeManager>();
            apiStatus = FindObjectOfType<APIStatus>();
            //currentNpcThinkingSoundsArray = nPCInteractorScript.arrayThinkingNPCsounds;
            //textToSpeech.audioSource = nPCInteractorScript.NPCaudioSource;
            //textToSpeech.voiceID_name = nPCInteractorScript.voiceIDNameThisNpc;
            messages = nPCInteractorScript.ChatLogWithNPC;
        }
        
        public async Task<string> SendRequestToChatGpt(List<ChatMessage> combinedMessages)
        {
            CreateChatCompletionRequest request = new CreateChatCompletionRequest();
            request.Messages = combinedMessages;
            //request.Model = "gpt-3.5-turbo-16k-0613";
            request.Model = "gpt-4-1106-preview";
            request.Temperature = 0.9f;
            request.MaxTokens = 256;

            Stopwatch stopwatch = Stopwatch.StartNew();
            apiStatus.isGeneratingText = true;

            var response = await openai.CreateChatCompletion(request);

            stopwatch.Stop();
            apiStatus.isGeneratingText = false;

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
                    nPCInteractorScript.ChatLogWithNPC.Add(userMessage);
                    break;
                default:
                    nPCInteractorScript.ChatLogWithNPC.Add(userMessage);
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
                    nPCInteractorScript.ChatLogWithNPC.Add(assistantMessage);
                    break;
                default:
                    nPCInteractorScript.ChatLogWithNPC.Add(assistantMessage);
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
                    nPCInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                default:
                    nPCInteractorScript.ChatLogWithNPC.Add(message);
                    break;
                
            }
        }
        
    }
}
