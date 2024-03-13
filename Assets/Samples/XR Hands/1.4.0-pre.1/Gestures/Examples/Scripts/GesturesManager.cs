using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;


public class GesturesManager :MonoBehaviour // Responsible for keeping gesture variables, aswell as making them easy to access from other classes.
{
 
    //public GestureManagerDebuggingMenu gestureManagerDebuggingMenu;
    public List<XRHandPose> GestureList_Right, GestureList_Left;

    public bool pointingWithLeftHand, pointingWithRightHand;
    public XRHandPose pointingPose;

    public string PointingActionLeft, PointingActionRight;

    public bool startInDebugMode = true;

    void Start()
    {
        if (!startInDebugMode) return;
    }

    public void AddGestureToList_Right(XRHandPose handPose)
    {
        //nameOfCurrentRightGesture = handPose.name;
        GestureList_Right.Add(handPose);

        checkForPointingWithHands();

        //if (startInDebugMode)
        //    gestureManagerDebuggingMenu();
    }

    public void RemoveGestureFromList_Right(XRHandPose handPose)
    {
        GestureList_Right.Remove(handPose);

        checkForPointingWithHands();

        //if (startInDebugMode)
        //    SetDebuggingTexts();
    }

    public void AddGestureToList_Left(XRHandPose handPose)
    {
        //nameOfCurrentLeftGesture = handPose.name;
        GestureList_Left.Add(handPose);

        checkForPointingWithHands();

        //if (startInDebugMode)
        //    SetDebuggingTexts();
    }

    public void RemoveGestureFromList_Left(XRHandPose handPose)
    {
        GestureList_Left.Remove(handPose);

        checkForPointingWithHands();

        //if (startInDebugMode)
        //    SetDebuggingTexts();
    }
 
    private void checkForPointingWithHands()
    {
        if (GestureList_Left.Count > 0 && GestureList_Left[0] == pointingPose)
        {
            pointingWithLeftHand = true;
        }
        else
            pointingWithLeftHand = false;

        if (GestureList_Right.Count > 0 && GestureList_Right[0] == pointingPose)
        {
            pointingWithRightHand = true;
        }
        else
            pointingWithRightHand = false;
    }
    public void AddPointingAtTargetDescription_LeftHand(string interestPointDescription)
    {
        PointingActionLeft = "user is pointing at " + interestPointDescription + " with their left hand";
    }
    public void AddPointingAtTargetDescription_RightHand(string interestPointDescription)
    {
        PointingActionRight = "user is pointing at " + interestPointDescription + " with their right hand";
    }
}
