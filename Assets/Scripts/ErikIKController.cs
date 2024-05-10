using GLTFast.Schema;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ErikIKController : MonoBehaviour
{
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private BoatRouteNavMesh boatRouteScript;
    [SerializeField] private NpcAnimationStateController npcAnimationStateController;
    [SerializeField] private GameObject xrRigOffset;

    private Transform rightHandIKTarget;
    private Transform leftHandIKTarget;
    
    private Transform lookIKTarget;
    public Transform LookTarget;
    public Transform defaultLookTarget;
    public Transform chosenLookTarget;

    [Range(0, 1f)]
    public float HandIKAmount = 0f;
    [Range(0, 1f)]
    public float BodyIKAmount = 0f;

    public bool isLeft = false;
    public bool isRight = false;

    public bool isLookingAtPOI = false;
    public bool isPointing = false;

    public bool pointDebug = false;

    public Transform pointRotation;

    private void Start()
    {
        defaultLookTarget = xrRigOffset.transform;
        chosenLookTarget = defaultLookTarget;
        lookIKTarget = LookTarget;
    }

    private void Update()
    {
        defaultLookTarget = xrRigOffset.transform;
        
        // Checks Direction of POI
        Vector3 directionToPoi = (transform.position - chosenLookTarget.position).normalized;

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
        if (isLookingAtPOI)
        {
            //lookIKTarget = boatRouteScript.currentPOI;
            //StartCoroutine(ChangeLookTarget(startPosition, boatRouteScript.currentPOI.position, 1));
            lookIKTarget.position = Vector3.Lerp(lookIKTarget.position, chosenLookTarget.position, 0.2f * Time.deltaTime);
        }
        else if (!isLookingAtPOI)
        {
            // lookIKTarget = startTransform;
            lookIKTarget.position = Vector3.Lerp(lookIKTarget.position, defaultLookTarget.position, 0.2f * Time.deltaTime); ;
        }

        // Sets right hand IK target
        if (isPointing && isRight && rightHandIKTarget != boatRouteScript.currentPOI)
        {
            rightHandIKTarget = chosenLookTarget;
        }
        else if (!isPointing || leftHandIKTarget != null)
        {
            rightHandIKTarget = null;
        }

        // Sets left hand IK target
        if (isPointing && isLeft && leftHandIKTarget != boatRouteScript.currentPOI)
        {
            leftHandIKTarget = chosenLookTarget;
        }
        else if (!isPointing || rightHandIKTarget != null)
        {
            leftHandIKTarget = null;
        }

        if (isPointing && isRight)
        {
            npcAnimator.SetBool("PointingRight", true);
            npcAnimator.SetBool("PointingLeft", false);
        }
        else if (isPointing && isLeft)
        {
            npcAnimator.SetBool("PointingLeft", true);
            npcAnimator.SetBool("PointingRight", false);
        }
        else
        {
            npcAnimator.SetBool("PointingRight", false);
            npcAnimator.SetBool("PointingLeft", false);
        }

        // Check rotation to look target
        var lookPos = lookIKTarget.position - pointRotation.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        pointRotation.rotation = rotation;

        // Trigger in the inspector to test look mechanic
        if (pointDebug)
        {
            PointingDebug();
            pointDebug = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (lookIKTarget != null)
        {
            if (isLookingAtPOI)
            {
                npcAnimator.SetLookAtWeight(1, BodyIKAmount, 0.75f, 0.75f, 0.7f);
            }
            else
            {
                npcAnimator.SetLookAtWeight(1, BodyIKAmount, 0.7f, 0.75f, 0.7f);
            }

            npcAnimator.SetLookAtPosition(lookIKTarget.position);
        }
        
        if (rightHandIKTarget != null)
        {
            npcAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKAmount);
            npcAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKAmount);
            npcAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIKTarget.position);
            npcAnimator.SetIKRotation(AvatarIKGoal.RightHand, pointRotation.rotation);
        }

        if (leftHandIKTarget != null)
        {
            npcAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            npcAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            npcAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIKTarget.position);
            npcAnimator.SetIKRotation(AvatarIKGoal.LeftHand, pointRotation.rotation);
        }
    }

    public void PointingDebug()
    {
        StartCoroutine(npcAnimationStateController.AnimateBodyResponse_Erik("POINTING", 0f));
    }

    public IEnumerator ChangeLookTarget(float duration)
    {
        float time = 0;

        while (time < duration)
        {
            lookIKTarget.position = Vector3.Lerp(chosenLookTarget.position, defaultLookTarget.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        lookIKTarget.position = defaultLookTarget.position;
    }

    public void ChooseLookTarget(string triggerString)
    {
        switch (triggerString)
        {
            case "FISHINGHUT":
                chosenLookTarget = boatRouteScript.PointsOfInterest[0].transform;
                break;
            case "RUNESTONE":
                chosenLookTarget = boatRouteScript.PointsOfInterest[1].transform;
                break;
            case "FARMSTEAD":
                chosenLookTarget = boatRouteScript.PointsOfInterest[2].transform;
                break;
            case "VILLAGE":
                chosenLookTarget = boatRouteScript.PointsOfInterest[3].transform;
                break;
            case "BURIALMOUND":
                chosenLookTarget = boatRouteScript.PointsOfInterest[4].transform;
                break;
            case "MARKETENTRANCE":
                chosenLookTarget = boatRouteScript.PointsOfInterest[5].transform;
                break;
            case "BLACKSMITH":
                chosenLookTarget = boatRouteScript.PointsOfInterest[6].transform;
                break;
            case "BOATBUILDER":
                chosenLookTarget = boatRouteScript.PointsOfInterest[7].transform;
                break;
            case "TRADERS":
                chosenLookTarget = boatRouteScript.PointsOfInterest[8].transform;
                break;
            case "ERIKSHUT":
                chosenLookTarget = boatRouteScript.PointsOfInterest[9].transform;
                break;
        }
    }
}
