using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingManager : MonoBehaviour
{
    private GestureManagerNew m_Manager;

    [SerializeField] private ChoosePromptGesture choosePromptGestureScript;
    public Transform rightHand_indexStart, rightHand_indexEnd, leftHand_indexStart, leftHand_indexEnd;
    public LineRenderer leftLineRenderer, rightLinerenderer;
    public int layermask;
    private int invertedLayermask;
    public string leftHandLastSelected, rightHandLastSelected;

    public bool showLines;

    private float counter;
    private float pointingResetCounter;


    void Start()
    {
        choosePromptGestureScript = FindObjectOfType<ChoosePromptGesture>();
        m_Manager = FindAnyObjectByType<GestureManagerNew>();

        // Bit shift the index of the layer (8) to get a bit mask // From the unity docs..
        invertedLayermask = 1 << layermask;
    }

    void Update()
    {
        pointingResetCounter += Time.deltaTime;     //Counter used for clearing the dictionary filled with the objects the user pointed at.
        counter += Time.deltaTime;             //Counter used for counting the time the user is pointing at an object.

        if (pointingResetCounter >= 10)     //If pointingResetCounter reaches 10 seconds, aka the user hasn't pointed their finger ...
        {
            if (choosePromptGestureScript.pointingTimes.Count > 0)
            {
                choosePromptGestureScript.ClearDictionaryOfPointedItems();  //the dictionary of pointed objects is cleared
                pointingResetCounter = 0;   //and the pointingResetCounter is reset
                rightHandLastSelected = "";
                leftHandLastSelected = "";
            }
            
        }   
        
        if (m_Manager.pointingWithLeftHand)
        {
            RaycastHit hit;
            Vector3 leftPointingDirection = (leftHand_indexEnd.position - leftHand_indexStart.position).normalized; //Calculating the direction to shoot the ray.
            //Left hand pointing shoot ray!
            if (Physics.Raycast(leftHand_indexEnd.position, leftPointingDirection, out hit, Mathf.Infinity, invertedLayermask)) {
                Debug.DrawRay(leftHand_indexEnd.position, leftPointingDirection * hit.distance, Color.yellow); Debug.Log(hit.transform.gameObject);
                if (hit.transform.gameObject.name != leftHandLastSelected)      //Instead of looking at the pointed at gameobject's name, maybe first look for ray collision with a certain tag
                                                                                //like "PointOfInterest" FIRST. This can potentially be better for performance.
                {
                    pointingResetCounter = 0;
                    counter = 0;
                    choosePromptGestureScript.AddPointingItemToDic(hit.transform.GetComponent<InterestPointDescription>().description);
                    
                    //Add to Array/List here
                    m_Manager.AddPointingAtTargetDescription_LeftHand(choosePromptGestureScript.FindMostPointingTimeInDictionary());
                    leftHandLastSelected = hit.transform.gameObject.name;
                }
                else if (hit.transform.gameObject.name == leftHandLastSelected)
                {
                    pointingResetCounter = 0;
                    choosePromptGestureScript.AddPointingTime(hit.transform.GetComponent<InterestPointDescription>().description, counter);      //If the object currently being pointed at has not changed from the last frame
                                                                                                            //then we add the time from the last frame to count how long the user is pointing at the object
                }
            }
            else
                Debug.DrawRay(leftHand_indexEnd.position, leftPointingDirection * 1000, Color.white);

            if (showLines)
            {
                leftLineRenderer.SetPosition(0, leftHand_indexEnd.position);
                leftLineRenderer.SetPosition(1, leftHand_indexEnd.position + leftPointingDirection * 300);
            }
            
        }
        if (m_Manager.pointingWithRightHand)
        {
            RaycastHit hit;
            Vector3 rightPointingDirection = (rightHand_indexEnd.position - rightHand_indexStart.position).normalized;
            //Right hand pointing shoot ray! pew pew
            if (Physics.Raycast(rightHand_indexEnd.position, rightPointingDirection, out hit, Mathf.Infinity, invertedLayermask))
            {
                Debug.DrawRay(rightHand_indexEnd.position, rightPointingDirection * hit.distance, Color.yellow); Debug.Log(hit.transform.gameObject);
                if (hit.transform.gameObject.name != rightHandLastSelected)
                {
                    pointingResetCounter = 0;
                    counter = 0;
                    choosePromptGestureScript.AddPointingItemToDic(hit.transform.GetComponent<InterestPointDescription>().description);
                    
                    //m_Manager.AddPointingAtTargetDescription_RightHand(hit.transform.GetComponent<InterestPointDescription>().description);
                    m_Manager.AddPointingAtTargetDescription_RightHand(choosePromptGestureScript.FindMostPointingTimeInDictionary());       //DO NULL CHECK maybe to see if the pointing description dictionary is empty.
                    rightHandLastSelected = hit.transform.gameObject.name;
                }
                else if (hit.transform.gameObject.name == rightHandLastSelected)
                {
                    pointingResetCounter = 0;
                    choosePromptGestureScript.AddPointingTime(hit.transform.GetComponent<InterestPointDescription>().description, counter);      //If the object currently being pointed at has not changed from the last frame
                                                                                                            //then we add the time from the last frame to count how long the user is pointing at the object
                }
            }
            else
                Debug.DrawRay(rightHand_indexEnd.position, rightPointingDirection * 1000, Color.white);

            if (showLines)
            {
                rightLinerenderer.SetPosition(0, rightHand_indexEnd.position);
                rightLinerenderer.SetPosition(1, rightHand_indexEnd.position + rightPointingDirection * 300);
            }
        }
    }
    public void ShowLines()
    {
        showLines = true;
        leftLineRenderer.enabled = true;
        rightLinerenderer.enabled = true;
    }

    public void HideLines()
    {
        showLines = false;
        leftLineRenderer.enabled = false;
        rightLinerenderer.enabled = false;
    }
}