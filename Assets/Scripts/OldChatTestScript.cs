using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenAI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OldChatTestScript : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ScrollRect scroll;
        
    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;
        
    public string nameOfCurrentNPC;
    
    [SerializeField] private TextToSpeech textToSpeech;
        
    public UnityEvent OnReplyReceived;

    private string text;
    private string response;        //This string is what ChatGPT answers after a request
    public bool isDone = true;
    private RectTransform messageRect;

    private float height;
    private OpenAIApi openai = new OpenAIApi();

    public List<ChatMessage> messages = new List<ChatMessage>();
    // Start is called before the first frame update
    private RectTransform AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            
            if (message.Role != "user")
            {
                messageRect = item;
            }
            
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);

            if (message.Role == "user")
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(item);
                height += item.sizeDelta.y;
                scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                scroll.verticalNormalizedPosition = 0;
            }

            return item;
        }

        private void SendReply()
        {
            SendReply(inputField.text);
        }

        public void SendReply(string input)
        {
            var message = new ChatMessage()
            {
                Role = "user",
                Content = input
            };

            openai.CreateChatCompletionAsync(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-16k-0613",
                Messages = messages
            }, OnResponse, OnComplete, new CancellationTokenSource());

            AppendMessage(message);

            inputField.text = "";
        }

        private void OnResponse(List<CreateChatCompletionResponse> responses)
        {
            text = string.Join("", responses.Select(r => r.Choices[0].Delta.Content));

            if (text == "") return;

            if (text.Contains("END_CONVO"))           //An example of how you could make sure the prompting (chatlog)
            {                                           //with the OpenAI API gets wiped if the ChatGPT feels like you wanna
                text = text.Replace("END_CONVO", "");   //end the conversation
                
                Invoke(nameof(EndConvo), 5);
            }
            
            
            var message = new ChatMessage()
            {
                Role = "assistant",
                Content = text
            };

            if (isDone)
            {
                OnReplyReceived.Invoke();
                //messageRect = AppendMessage(message);
                isDone = false;
                //Debug.Log(text);
            }
            
            messageRect.GetChild(0).GetChild(0).GetComponent<Text>().text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            
            response = text;
        }
        
        private void OnComplete()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(messageRect);
            height += messageRect.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;

            textToSpeech.MakeAudioRequest(response);        //The audio file is created within the MakeAudioRequest method and is stored in the clip variable within this method

            if (response.Contains("FINISH_SEN"))
            {
                Debug.Log("Audio REQUEST");
                textToSpeech.MakeAudioRequest(response);        //The audio file is created within the MakeAudioRequest method and is stored in the clip variable within this method
                response = response.Replace("FINISH_SEN", "");
            }
            
            textToSpeech.MakeAudioRequest(response);
            
            isDone = true;
            
            Debug.Log(response);
            
            response = "";
        }

        private void EndConvo()
        {
            //npcDialog.Recover();
            messages.Clear();
        }
}
