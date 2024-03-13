using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehavior : StateMachineBehaviour
{

    [SerializeField]
    private int numberOfBoredAnimations;

    [SerializeField]
    private int timeUntilBored = 10;

    [SerializeField]
    private float blendDuration = 1f;

    private int boredID;
    private int previousBoredID;

    private bool isBored = false;

    private float timeSinceAnimationStart;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBored == false)
        {
            timeSinceAnimationStart += Time.deltaTime;

            if (timeSinceAnimationStart > timeUntilBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isBored = true;
                boredID = Random.Range(1, numberOfBoredAnimations + 1);
                boredID = boredID * 2 - 1;

                if (boredID == previousBoredID)
                {
                    boredID += 2;

                    if (boredID > numberOfBoredAnimations * 2 + 1)
                    {
                        boredID -= 4;

                        if (boredID < 0)
                        {
                            boredID += 2;
                        }
                    }
                }

                animator.SetFloat("IdleBlend", boredID - 1);
                previousBoredID = boredID;
                // Debug.Log($"Bored: {isBored}, Starting Animation: {boredID}");
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("IdleBlend", boredID, blendDuration, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (isBored)
        {
            boredID--;
        }

        isBored = false;
        timeSinceAnimationStart = 0;
        // Debug.Log($"Bored: {isBored}, Starting Animation: {boredID}");
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
