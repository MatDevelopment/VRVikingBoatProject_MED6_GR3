using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIStatus : MonoBehaviour //Responsible for holding and providing the current status for Api calls and derived variables like is the ECA talking.
{
    [SerializeField]
    public bool isTranscribing, isGeneratingText, isGeneratingAudio, isTalking;


    public IEnumerator SetIsTalkingFalseAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        isTalking = false;
    }


}
