using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinkingBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private int animationDuration = 5;

    [SerializeField]
    private float blendDuration = 1f;

    [SerializeField]
    private int numberOfAnimations;

    private int TalkingId;
    private float timeSinceAnimationStart;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TalkingId = Random.Range(0, numberOfAnimations);

        // Debug.Log($"Start Animation ID: {TalkingId}");

        animator.SetFloat("ThinkingBlend", TalkingId);

        timeSinceAnimationStart = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceAnimationStart += Time.deltaTime;

        if (timeSinceAnimationStart >= animationDuration)
        {
            timeSinceAnimationStart = 0;
            
            int newTalkingId = Random.Range(0, numberOfAnimations);

            if (newTalkingId == TalkingId)
            {
                newTalkingId++;

                if (newTalkingId >= numberOfAnimations)
                {
                    newTalkingId -= 2;
                }
            }

            // Debug.Log($"animation change {newTalkingId}");

            TalkingId = newTalkingId;
        }

        animator.SetFloat("TalkingBlend", TalkingId, blendDuration, Time.deltaTime);
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
