using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenAI;
using UnityEngine;



public class UserGazeErik : MonoBehaviour
{
    private bool isErikVisible;
    [SerializeField] private ChatTest chatTestScript;
    [SerializeField] private NPCInteractorScript npcInteractorScript;
    
    private void OnBecameInvisible()
    {
        isErikVisible = false;
        StartCoroutine(IsErikVisibleAfterDuration(10)); //Prompt designed to slightly annoy Erik for not looking in his vicinity for 10 seconds.
        StartCoroutine(IsErikVisibleAfterDuration(23)); //Prompt designed to make Erik perceive the user as RUDE for not looking in his vicinity for more than 20 seconds.
    }

    private void OnBecameVisible()
    {
        isErikVisible = true;
        StopAllCoroutines();
    }

    private IEnumerator IsErikVisibleAfterDuration(float gazeTimeDuration)
    {
        yield return new WaitForSeconds(gazeTimeDuration);
        if (isErikVisible == false)
        {   
            //Maybe look for a keyword in the chatTestScript.combinedMessages string list variable,
            //which can be used for counting the amount of times that the user has NOT been looking
            //at Erik for more than X amount of time (maybe within a recent timeframe).
            //This could make Erik seem conscious of the fact that user does not "respect" them if they have been avoiding looking at Erik.
            //We can make Erik have either positive or negative reactions to this, depending on the context of the conversation e.g.
            //...maybe by searching for other keywords in the conversation.
            
            npcInteractorScript.InformAndInitiateNpcTalk("The Traveller has NOT looked at Erik for" + gazeTimeDuration + ". Erik considers this to be rude if the Traveller has NOT looked at him for more than 20 seconds and will be verbal about it to the Traveller." +
                                                         "Otherwise Erik will simply remember this information and keep it in mind.");
        }
    }
    
}
