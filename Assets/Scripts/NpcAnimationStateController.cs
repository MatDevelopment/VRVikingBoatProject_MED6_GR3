using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NpcAnimationStateController : MonoBehaviour //TODO: needs to add illustrator gestures
{
    [SerializeField] APIStatus apiStatus;
    [SerializeField] Animator animator;

    private AudioPlayer audioPlayer;

    void Start()
    {
        apiStatus = FindObjectOfType<APIStatus>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        animator = GetComponent<Animator>();
    }

    // Checks if the audio source is playing to call the animator to transition between talking and idle
    private void Update()
    { //TODO:This probably needs to be changed

        if (apiStatus.isTalking == true)
        {
            animator.SetBool("isThinking", false);
        }
        else
        {
            animator.SetBool("isThinking", true);
        }

        if (audioPlayer.AudioSourcePlaying() == true)
        {
            animator.SetBool("isTalking", true);
        }
        else
        {
            animator.SetBool("isTalking", false);
        }
    }


    public void AnimateErik(string gestureName, float delay)
    {
        StartCoroutine(AnimateBodyResponse_Erik(gestureName, delay));
    }
  
    public IEnumerator AnimateBodyResponse_Erik(string triggerString, float delay)

    {
        yield return new WaitForSeconds(delay);

        switch (triggerString)      //Seek for trigger strings and run this method in a foreach loop containing all possible actions.
                                    //Then run a coroutine under each switch case including the time calculated until the animation is played, which will allow things to run asynchrously
        {
            case "APPROVE":
                animator.SetTrigger("gest.Approval"); // Sets the trigger which is the same for all the layers.
                //the list of ints are the different values for the blend tree   
                //the list of doubles are the Proablities for each value to be chosen
                animator.SetFloat("ApprovalType", RandomWeightedPicker.Pick(new List<int> { 0, 1 }, new List<double> { 0.2, 0.8 }));//Pick between the values and set the float in the animator. This determines the gesture variation to be played.
                break;

            case "DISAPPROVE":
                animator.SetTrigger("gest.Disapproval");
                animator.SetFloat("DisapprovalType", RandomWeightedPicker.Pick(new List<int> { 0, 1 }, new List<double> { 0.2, 0.8 }));
                break;

            case "GREETING":
                animator.SetTrigger("gest.Greeting");
                //Only one variation implemented!
                break;
            case "POINTING":
                animator.SetTrigger("gest.Pointing");
                //Only one variation implemented!
                break;
            case "UNSURE":
                animator.SetTrigger("gest.Unsure");
                animator.SetFloat("DisapprovalType", RandomWeightedPicker.Pick(new List<int> { 0, 1 }, new List<double> { 0.7, 0.3 }));
                break;
            case "GRATITUDE":
                animator.SetTrigger("gest.Gratitude");
                //Only one variation implemented!
                break;
            case "CONDOLENCE":
                animator.SetTrigger("gest.Condolence");
                //Only one variation implemented!
                break;
            case "INSULT":
                animator.SetTrigger("gest.Insult");
                //Only one variation implemented!
                break;
            case "STOP":
                animator.SetTrigger("gest.Stop");
                //Only one variation implemented!
                break;
            //case Gesture.Continue:
            //    animator.SetTrigger("gest.Continue");
            //    break;
            default:
                Debug.Log("No gesture used!");
                break;
        }
    }
    }
