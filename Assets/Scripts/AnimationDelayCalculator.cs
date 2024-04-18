using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AnimationDelayCalculator 
{
    public static float CalculateTimeToPerformAction()
    {
        return 2f;
    }

    public static float EstimatedTimeTillAction(int punctuationCount, int wordCount, float punctuationWeight, float wordWeight) // Takes in the variables created above as arguments to estimate a time to play the gesture/Action animation.
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

        DebugInfo(punctuationCount, wordCount, wordWeight, punctuationWeight);
        return estimatedTimeTillAction;
    }

    public static string CreateStringUntilKeyword(string inputString, string actionToCheck)
    {
        string responseTillAction = inputString;

        int endCharIndex = inputString.IndexOf(actionToCheck);
        responseTillAction = inputString.Substring(0, endCharIndex);

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

    public static int CountCharsUsingLinqCount(string sourceString, char charToFind)
    {
        return sourceString.Count(t => t == charToFind);
    }

    public static int CountWordsInString(string sourceString)
    {
        int NumberOfWords = sourceString.Split().Length;        //Counts the numbers of words in sourceString... apparently: https://stackoverflow.com/a/26794798

        return NumberOfWords;
    }

    private static void DebugInfo(int punctuationCount, int wordCount, float punctuationWeight, float wordWeight)
    {
      
    }
}
