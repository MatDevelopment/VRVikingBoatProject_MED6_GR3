using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    //Script Inspiration -> https://www.youtube.com/watch?v=Oadq-IrOazg

    //Get animator with fade animations and insert in inspector.
    public Animator fadeAnimator;

    //These are related to testing the methods of the script through number input.
    [Tooltip("While active: Fades can be tested. Press 1 to run FadeOut. Press 2 to run FadeIn. Press 3 to run ShowEndText.")]
    [SerializeField] private bool testFades = true;

    // Update is called once per frame
    void Update()
    {
        //Only allow to activate different functions with keyboard input if the testFades bool is true
        if (testFades == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                FadeOut();
                Debug.Log("Fading Out");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                FadeIn();
                Debug.Log("Fading In");
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                ShowEndText();
                Debug.Log("Showing End Text");
            }
        }
    }

    //Three different methods that can be called from anywhere (if needed) to start their respective animation.
    public void FadeOut()
    {
        fadeAnimator.SetTrigger("FadeOut");
    }

    public void FadeIn()
    {
        fadeAnimator.SetTrigger("FadeIn");
    }

    public void ShowEndText()
    {
        fadeAnimator.SetTrigger("ShowEndText");
    }

    public void FadeInAfterTime(float delay)
    {
        Invoke(nameof(FadeIn), delay);
    }
    public void FadeOutAfterTime(float delay)
    {
        Invoke(nameof(FadeOut), delay);
    }
}
