using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimationStateController : MonoBehaviour
{
    //public enum Gesture { approval, disapproval, greeting, pointing, unsure, gratitude, condolence, insult, stop, Continue
    //    } //TODO: needs to add illustrator gestures
    Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AnimateBodyResponse_Erik("INSULT", 1));
    }

    public void AnimateErik(string gestureName, float delay)
    {
        Debug.Log("Animate Erik!!");
        StartCoroutine(AnimateBodyResponse_Erik(gestureName, delay));
    }

    //THIS is the method meant to be used for GESTURES/ACTIONS
    public IEnumerator AnimateBodyResponse_Erik(string triggerString, float delay)      //This could also be repurposed for ChatGPT to choose a body gesture on their own, instead of asking
                                                                                //it to pick a secondary emotion. Could be argued to potentially give ChatGPT more agency.
    {


        yield return new WaitForSeconds(delay);

        switch (triggerString)      //Seek for trigger strings and run this method in a foreach loop containing all possible actions.
                                    //Then run a coroutine under each switch case including the time calculated until the animation is played, which will allow things to run asynchrously
        {
            case "APPROVE":
                animator.SetTrigger("gest.Approval");
                break;
            case "DISAPPROVE":
                animator.SetTrigger("gest.Disapproval");
                break;
            case "GREETING":
                animator.SetTrigger("gest.Greeting");
                break;
            case "POINTING":
                animator.SetTrigger("gest.Pointing");
                break;
            case "UNSURE":
                animator.SetTrigger("gest.Unsure");
                break;
            case "GRATITUDE":
                animator.SetTrigger("gest.Gratitude");
                break;
            case "CONDOLENCE":
                animator.SetTrigger("gest.Condolence");
                break;
            case "INSULT":
                animator.SetTrigger("gest.Insult");
                break;
            case "STOP":
                animator.SetTrigger("gest.Stop");
                break;
            //case Gesture.Continue:
            //    animator.SetTrigger("gest.Continue");
            //    break;
            default:
                Debug.Log("No gesture used!");
                break;

        }

       
    }
 

    //public void PlayGesture(Gesture gesture)
    //{
    //    switch (gesture)
    //    {
    //        case Gesture.approval:
    //            animator.SetTrigger("gest.Approval");
    //            break;
    //        case Gesture.disapproval:
    //            animator.SetTrigger("gest.Disapproval");
    //            break;
    //        case Gesture.greeting:
    //            animator.SetTrigger("gest.Greeting");
    //            break;
    //        case Gesture.pointing:
    //            animator.SetTrigger("gest.Pointing");
    //            break;
    //        case Gesture.unsure:
    //            animator.SetTrigger("gest.Unsure");
    //            break;
    //        case Gesture.gratitude:
    //            animator.SetTrigger("gest.Gratitude");
    //            break;
    //        case Gesture.condolence:
    //            animator.SetTrigger("gest.Condolence");
    //            break;
    //        case Gesture.insult:
    //            animator.SetTrigger("gest.Insult");
    //            break;
    //        case Gesture.stop:
    //            animator.SetTrigger("gest.Stop");
    //            break;
    //        case Gesture.Continue:
    //            animator.SetTrigger("gest.Continue");
    //            break;
    //    }
    //}
}
