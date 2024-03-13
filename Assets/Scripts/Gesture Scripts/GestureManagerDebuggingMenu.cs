using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands.Gestures;

public class GestureManagerDebuggingMenu : MonoBehaviour
{
    [SerializeField]
    public PointingManager pointingManager;
    public TextMeshProUGUI gestureListText_right, gestureListText_left, gestureHistoryText;
    public CanvasGroup[] canvasGroups;

    private void Awake()
    {
        //pointingManager = GetComponent<PointingManager>();

    }
    void Start()
    {
        gestureListText_right.text = "Hello \n hello \n hello n\"";
        gestureListText_left.text = "Hello \n hello \n hello n\"";
    }

    public void UpdateDebugTexts(List<XRHandPose> GestureList_Right, List<XRHandPose> GestureList_Left) //For debugging
    {
        string result_right = "";
        string result_left = "";
        foreach (var listMember in GestureList_Right)
            result_right += listMember.ToString() + "";
        foreach (var listMember in GestureList_Left)
            result_left += listMember.ToString() + "";

        gestureListText_right.text = result_right;
        gestureListText_left.text = result_left;

        gestureHistoryText.text += result_right;
        gestureHistoryText.text += result_left;
    }

    public void ShowMenu()
    {
        foreach (CanvasGroup cg in canvasGroups)
        {
            cg.alpha = 1.0f;
        }
        pointingManager.ShowLines();
    }

    public void HideMenu()
    {
        foreach (CanvasGroup cg in canvasGroups)
        {
            cg.alpha = 0.0f;
        }
            pointingManager.HideLines();
    }
}
