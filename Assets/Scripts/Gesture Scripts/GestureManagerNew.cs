using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;

public class GestureManagerNew : MonoBehaviour
{
    public GestureManagerDebuggingMenu gestureManagerDebuggingMenu;
    public List<XRHandPose> GestureList_Right, GestureList_Left;

    public bool pointingWithLeftHand, pointingWithRightHand;
    public XRHandPose pointingPose;

    public XRHandPose pose1, pose2;

    public string PointingActionLeft, PointingActionRight, combinedGesturesString;

    public List<string> combinedGestureHistory;

    public bool startInDebugMode = true;

    private void Awake()
    {
        gestureManagerDebuggingMenu = FindAnyObjectByType<GestureManagerDebuggingMenu>();
    }

    void Start()
    {
        AddPointingAtTargetDescription_LeftHand("giant wolf"); // Mock description for debugging;
        //AddPointingAtTargetDescription_RightHand("Tree (47)"); // Mock description for debugging;'
        DEBUG_AddGestureToLists();

        Debug.Log(PullCombinedGesturesString());

        gestureManagerDebuggingMenu = GetComponent<GestureManagerDebuggingMenu>();
        if (!startInDebugMode) return;
        gestureManagerDebuggingMenu.ShowMenu();
    }

    private void DEBUG_AddGestureToLists()
    {
        AddGestureToList_Right(pose2);
        AddGestureToList_Left(pose1);
    }
    private void Update()
    {
     if(Input.GetKeyDown(KeyCode.J)) {
            AddPointingAtTargetDescription_RightHand("A nice fish that jumped in the river water"); // Mock description for debugging;
            RemoveGestureFromList_Left(pose1);
        }   
    }
    private void checkForPointingWithHands() // Checks if the current gesture is pointing for both the left and right hands;
    {
        if (GestureList_Left.Count > 0 && GestureList_Left[0] == pointingPose)
        {
            pointingWithLeftHand = true;
        }
        else
        {
            pointingWithLeftHand = false;
                    }

        if (GestureList_Right.Count > 0 && GestureList_Right[0] == pointingPose)
        {
            pointingWithRightHand = true;
        }
        else
        {
            pointingWithRightHand = false;
        }
    }
    public void AddPointingAtTargetDescription_LeftHand(string interestPointDescription) // sets the current pointing string for left hand. 
    {
        PointingActionLeft = "pointing at " + interestPointDescription + " with their left hand";
    }
    public void AddPointingAtTargetDescription_RightHand(string interestPointDescription) // Duplicated for ease of readability.
    {
        PointingActionRight = "pointing at " + interestPointDescription + " with their right hand";
    }

    public string PullCombinedGesturesString() // The most important method in this class! return a string that describes what the user is currently doing with their hands.
    {
        string combinedString = "[User is ";

        //Left hand check
        if (PointingActionLeft != "") // check if string is empty
            combinedString += PointingActionLeft; // eg. "pointing at XX with their left hand"
        else if (GestureList_Left.Count>0)
            combinedString += "doing a " + GestureList_Left[0].name + " with their left hand";

        //Check if right hand is doing something and add an "and" between the two gesture strings.
        if (PointingActionRight != null || GestureList_Right[0] != null)
            combinedString += " and ";

        //Right hand check
        if (PointingActionRight != "")
            combinedString += PointingActionRight;
        else if (GestureList_Right[0] != null)
            combinedString += "doing a " + GestureList_Right[0].name + " with their right hand";

        combinedString += "]";

        //Check if any gestures are being made at all, if not, send a clean string
        if (PointingActionLeft == null && GestureList_Left[0] == null && PointingActionRight == null && GestureList_Right[0] == null)
            combinedString = "";
        else
            combinedGestureHistory.Add(combinedString);

        combinedGesturesString = combinedString;

        return combinedString;
    }

    public void AddGestureToList_Right(XRHandPose handPose)
    {
        GestureList_Right.Add(handPose);

        checkForPointingWithHands();

        if (startInDebugMode)
            gestureManagerDebuggingMenu.UpdateDebugTexts(GestureList_Right, GestureList_Left);
    }
    public void AddGestureToList_Left(XRHandPose handPose)
    {
        GestureList_Left.Add(handPose);

        checkForPointingWithHands();

        if (startInDebugMode)
            gestureManagerDebuggingMenu.UpdateDebugTexts(GestureList_Right, GestureList_Left);
    }
    public void RemoveGestureFromList_Left(XRHandPose handPose)
    {
        GestureList_Left.Remove(handPose);

        checkForPointingWithHands();

        if (startInDebugMode)
            gestureManagerDebuggingMenu.UpdateDebugTexts(GestureList_Right, GestureList_Left);
    }
    public void RemoveGestureFromList_Right(XRHandPose handPose)
    {
        GestureList_Right.Remove(handPose);

        checkForPointingWithHands();

        if (startInDebugMode)
            gestureManagerDebuggingMenu.UpdateDebugTexts(GestureList_Right, GestureList_Left);
    }

    public void OnValidate() // This is called everytime an update in the inspector UI happens. for example ticking a bool check box!
    {
        if (startInDebugMode && gestureManagerDebuggingMenu != null) gestureManagerDebuggingMenu.ShowMenu();
        else if (!startInDebugMode && gestureManagerDebuggingMenu != null) gestureManagerDebuggingMenu.HideMenu();
    }
}
