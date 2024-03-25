using System;
using System.Collections.Generic;
using Amazon;
using System.IO;
using System.Linq;
using UnityEngine;
using Amazon.Polly;
using Amazon.Runtime;
using Amazon.Polly.Model;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using UnityEngine.Events;
using UnityEngine.Networking;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using OpenAI;
using static Amazon.Polly.Model.Internal.MarshallTransformations.DescribeVoicesRequestMarshaller;

public class TextToSpeech : MonoBehaviour
{
    [SerializeField] private Whisper whisperScript;

    public AudioSource audioSource;
    [FormerlySerializedAs("animator")] [SerializeField] public Animator animatorSelectedNpc;
    private string accessKey;
    private string secretKey;

    [FormerlySerializedAs("voiceIDActor")] [SerializeField] private List<TextToSpeech> voiceIDActorsList;
    public string voiceID_name;

    [FormerlySerializedAs("isGenereatingSpeech")] public bool isGeneratingSpeech = false;
    //public List<string> VoiceId { get; }

    public AudioClip[] thinkingClips;

    private void Awake()
    {
        //List<TextToSpeech> voiceIDActorsList1 = new List<TextToSpeech>(VoiceId.FindValue(voiceID_name));
        //voiceIDActorsList = VoiceId;
        //ReadString();
        //Predicate<Person> oscarFinder = (Person p) => { return p.Name == "Oscar"; };
        
    }

    public async void MakeAudioRequest(string message)
    {
        var credentials = new BasicAWSCredentials(accessKey, secretKey);
        var client = new AmazonPollyClient(credentials, RegionEndpoint.EUCentral1); //Originally EUCentral1

        if (voiceID_name.Length > 0)
        {
            isGeneratingSpeech = true; //NEW
            
            var request = new SynthesizeSpeechRequest()
            {
                Text = message,
                Engine = Engine.Neural,
                VoiceId = VoiceId.FindValue(voiceID_name),
                //VoiceId = voiceIDActorsList.Find(voiceIDActorsList.Contains(voiceID_name)),
                OutputFormat = OutputFormat.Mp3
            };
            
            

            var response = await client.SynthesizeSpeechAsync(request);

            WriteIntoFile(response.AudioStream);
        }

        string audioPath;
        
        #if UNITY_ANDROID && !UNITY_EDITOR
            audioPath = $"jar:file://{Application.persistentDataPath}/audio.mp3";
        #elif (UNITY_IOS || UNITY_OSX) && !UNITY_EDITOR
            audioPath = $"file://{Application.persistentDataPath}/audio.mp3";
        #else
            audioPath = $"{Application.persistentDataPath}/audio.mp3";
        #endif
        
        using (var www = UnityWebRequestMultimedia.GetAudioClip(audioPath, AudioType.MPEG))
        {
            var op = www.SendWebRequest();

            while (!op.isDone) await Task.Yield();

            var clip = DownloadHandlerAudioClip.GetContent(www);

            audioSource.clip = clip;
            isGeneratingSpeech = false; //NEW
            audioSource.Play();
        }
    }

    // Checks if the audio source is playing to call the animator to transition between talking and idle
    private void Update() {
        
        if (whisperScript.ECAIsDoneTalking) {
            animatorSelectedNpc.SetBool("isThinking", false);
        }
        else {
            animatorSelectedNpc.SetBool("isThinking", true);
        }
        
        if (audioSource.isPlaying) {
            animatorSelectedNpc.SetBool("isTalking", true);
        }
        else {
            animatorSelectedNpc.SetBool("isTalking", false);
        }
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream($"{Application.persistentDataPath}/audio.mp3", FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
    
    private void ReadString()
    {
        string path = "Assets/authaws.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        accessKey = reader.ReadLine();
        if(accessKey == null) accessKey = "";
        secretKey = reader.ReadLine();
        if(secretKey == null) secretKey = "";
        reader.Close();
    }

    private void OnDestroy()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
