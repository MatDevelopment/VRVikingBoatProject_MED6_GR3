using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UIElements;

public class NpcAnimationStateController : MonoBehaviour //TODO: needs to add illustrator gestures
{
    [SerializeField] APIStatus apiStatus;
    [SerializeField] Animator animator;
    [SerializeField] ErikIKController ikController;

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
                int chosenGesture = RandomWeightedPicker.Pick(new List<int> { 0, 1 }, new List<double> { 0.7, 0.3 });
                animator.SetFloat("ApprovalType", chosenGesture);//Pick between the values and set the float in the animator. This determines the gesture variation to be played.

                if (chosenGesture == 0)
                {
                    float headTime = 0;
                    float headDuration = 0.5f;

                    float headStartValue = 0.75f;
                    float headEndValue = 0;

                    while (headTime < headDuration)
                    {
                        ikController.HeadIKAmount = Mathf.Lerp(headStartValue, headEndValue, headTime / headDuration);
                        headTime += Time.deltaTime;
                        yield return null;
                    }

                    ikController.HeadIKAmount = headEndValue;

                    yield return new WaitForSeconds(1);

                    while (headTime < headDuration)
                    {
                        ikController.HeadIKAmount = Mathf.Lerp(headEndValue, headStartValue, headTime / headDuration);
                        headTime += Time.deltaTime;
                        yield return null;
                    }

                    ikController.HeadIKAmount = headStartValue;
                }

                break;

            case "DISAPPROVE":
                animator.SetTrigger("gest.Disapproval");

                int dissapproveChosenGesture = RandomWeightedPicker.Pick(new List<int> { 0, 1 }, new List<double> { 0.7, 0.3 });
                animator.SetFloat("DisapprovalType", dissapproveChosenGesture);

                if (dissapproveChosenGesture == 0)
                {
                    float headTime = 0;
                    float headDuration = 0.5f;

                    float headStartValue = 0.75f;
                    float headEndValue = 0;

                    while (headTime < headDuration)
                    {
                        ikController.HeadIKAmount = Mathf.Lerp(headStartValue, headEndValue, headTime / headDuration);
                        headTime += Time.deltaTime;
                        yield return null;
                    }

                    ikController.HeadIKAmount = headEndValue;

                    yield return new WaitForSeconds(1);

                    while (headTime < headDuration)
                    {
                        ikController.HeadIKAmount = Mathf.Lerp(headEndValue, headStartValue, headTime / headDuration);
                        headTime += Time.deltaTime;
                        yield return null;
                    }

                    ikController.HeadIKAmount = headStartValue;
                }
                break;

            case "GREETING":
                animator.SetTrigger("gest.Greeting");
                //Only one variation implemented!
                break;
            case "POINTING":
                //animator.SetTrigger("gest.Pointing")
                if (!ikController.angleInvalid)
                {
                    ikController.isPointing = true;
                    ikController.isLookingAtPOI = true;

                    float time = 0;
                    float duration = 3;

                    float startValue = 0;
                    float endValue = 1;
                    float bodyEndValue = 0.25f;

                    while (time < duration)
                    {
                        ikController.HandIKAmount = Mathf.Lerp(startValue, endValue, time / duration);
                        ikController.BodyIKAmount = Mathf.Lerp(startValue, bodyEndValue, time / duration);
                        time += Time.deltaTime;
                        yield return null;
                    }

                    yield return new WaitForSeconds(2);

                    ikController.isLookingAtPOI = false;
                    StartCoroutine(ikController.ChangeLookTarget(3f));

                    time = 0;
                    while (time < duration)
                    {
                        ikController.HandIKAmount = Mathf.Lerp(endValue, startValue, time / duration);
                        ikController.BodyIKAmount = Mathf.Lerp(bodyEndValue, startValue, time / duration);
                        time += Time.deltaTime;
                        yield return null;
                    }

                    animator.SetBool("PointingLeft", false);
                    animator.SetBool("PointingRight", false);
                    ikController.HandIKAmount = startValue;
                    ikController.isPointing = false;
                    ikController.BodyIKAmount = startValue;
                } 
                else
                {
                    Debug.Log("Point angle invalid");
                }
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
