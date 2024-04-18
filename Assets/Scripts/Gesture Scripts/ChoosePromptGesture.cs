using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoosePromptGesture : MonoBehaviour
{
    [SerializeField] private GestureManager m_Manager;
    
    public Dictionary<string, float> pointingTimes =
        new Dictionary<string, float>();

    //private float counter;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*counter += Time.deltaTime;

        if (counter >= 10)
        {
            pointingTimes.Clear();      //Clears the dictionary of all keys and values.
            counter = 0;
        }*/
    }

    public void AddPointingItemToDic(string pointedItem)
    {
        if (!pointingTimes.ContainsKey(pointedItem))
        {
            pointingTimes.Add(pointedItem, 0);
        }
        /*else
        {
            pointingTimes[pointedItem] += counter;
        }*/
        
        /*// The Add method throws an exception if the new key is
        // already in the dictionary.
        try
        {
            openWith.Add("txt", "winword.exe");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("An element with Key = \"txt\" already exists.");
        }*/

    }

    public void AddPointingTime(string pointedItem, float counter)
    {
        pointingTimes[pointedItem] += counter;
    }

    public string FindMostPointingTimeInDictionary()
    {
        if (pointingTimes.Count > 0)
        {
            Dictionary<string, float>.ValueCollection valueColl =
                pointingTimes.Values;                       //We grab all the key VALUES and save them in the valueColl variable.
            
            float highestValue = valueColl.Max();       //We find the highest key value in the valueColl collection
                    
            string itemKey = pointingTimes.FirstOrDefault(x => x.Value == highestValue).Key;         //We then find the corresponding key for the key value...
                    
            return itemKey;     //and return it.
        }

        return null;
    }

    public void ClearDictionaryOfPointedItems()
    {
        pointingTimes.Clear();
    }

    public string CreatePointingPromptToChatGPT()
    {
        string itemMostlyPointedAt = FindMostPointingTimeInDictionary();

        string chatGPTPointingPrompt = "The Traveller has pointed at the following object/point of interest: " +
                                       itemMostlyPointedAt; 
        //A potential better way of doing this is using the AddSystemInstructionToChatLog method in ChatTest
        //since this simply adds a system prompt to the chatlog on its own, instead of incorporating what the
        //user is pointing at in the same sentence with what the user is saying, which can potentially confuse
        //ChatGPT.
        

        return chatGPTPointingPrompt;
    }
}
