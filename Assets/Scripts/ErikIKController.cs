using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErikIKController : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private BoatRouteNavMesh boatRouteScript;

    private Transform rightHandIKTarget;
    private Transform leftHandIKTarget;
    
    private Transform lookIKTarget;
    public Transform defaultLookTarget;

    [Range(0, 1f)]
    public float HandIKAmount = 1f;

    private bool isLeft = false;
    private bool isRight = false;

    public bool isLookingAtPOI = false;
    public bool isPointing = false;

    private void Start()
    {
        lookIKTarget = defaultLookTarget;
    }

    private void Update()
    {
        // Checks Direction of POI
        Vector3 directionToPoi = (transform.position - boatRouteScript.currentPOI.position).normalized;
        //Debug.Log(directionToPoi);

        if (directionToPoi.z < 0f && !isLeft)
        {
            isLeft = true; isRight = false;
            Debug.Log("left");
        }
        
        if (directionToPoi.z > 0f && !isRight)
        {
            isLeft = false; isRight = true;
            Debug.Log("right");
        }

        // Sets Look target
        if (isLookingAtPOI && lookIKTarget != boatRouteScript.currentPOI)
        {
            lookIKTarget = boatRouteScript.currentPOI;
        }
        else if (!isLookingAtPOI)
        {
            lookIKTarget = defaultLookTarget;
        }

        // Sets right hand IK target
        if (isPointing && isRight && rightHandIKTarget != boatRouteScript.currentPOI)
        {
            rightHandIKTarget = boatRouteScript.currentPOI;
        }
        else if (!isPointing || leftHandIKTarget != null)
        {
            rightHandIKTarget = null;
        }

        // Sets left hand IK target
        if (isPointing && isLeft && leftHandIKTarget != boatRouteScript.currentPOI)
        {
            leftHandIKTarget = boatRouteScript.currentPOI;
        }
        else if (!isPointing || rightHandIKTarget != null)
        {
            leftHandIKTarget = null;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (lookIKTarget != null)
        {
            if (isLookingAtPOI)
            {
                npcAnimator.SetLookAtWeight(1, 0.75f, 1f, 0.75f, 0.7f);
            }
            else
            {
                npcAnimator.SetLookAtWeight(1, 0, 0.7f, 0.75f, 0.7f);
            }
            
            npcAnimator.SetLookAtPosition(lookIKTarget.position);
        }
        
        if (rightHandIKTarget != null)
        {
            npcAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKAmount);
            npcAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKAmount);
            npcAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
            npcAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIKTarget.rotation);
        }

        if (leftHandIKTarget != null)
        {
            npcAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            npcAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            npcAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            npcAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIKTarget.rotation);
        }
    }
}
