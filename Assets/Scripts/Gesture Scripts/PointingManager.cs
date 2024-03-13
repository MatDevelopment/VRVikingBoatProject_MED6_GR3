using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingManager : MonoBehaviour
{
    private GestureManagerNew m_Manager;
    public Transform rightHand_indexStart, rightHand_indexEnd, leftHand_indexStart, leftHand_indexEnd;
    public LineRenderer leftLine, rightLine;
    public int layermask;
    private int invertedLayermask;
    public GameObject leftHandLastSelected, rightHandLastSelected;

    public bool showLines;


    void Start()
    {
        m_Manager = GameObject.FindAnyObjectByType<GestureManagerNew>();

        // Bit shift the index of the layer (8) to get a bit mask // From the unity docs..
        invertedLayermask = 1 << layermask;
    }

    void Update()
    {
        if (m_Manager.pointingWithLeftHand)
        {
            RaycastHit hit;
            Vector3 leftPointingDirection = (leftHand_indexEnd.position - leftHand_indexStart.position).normalized;
            //Left hand pointing shoot ray!
            if (Physics.Raycast(leftHand_indexEnd.position, leftPointingDirection, out hit, Mathf.Infinity, invertedLayermask)) {
                Debug.DrawRay(leftHand_indexEnd.position, leftPointingDirection * hit.distance, Color.yellow); Debug.Log(hit.transform.gameObject);
                if (hit.transform.gameObject != leftHandLastSelected)
                {
                    m_Manager.AddPointingAtTargetDescription_LeftHand(hit.transform.GetComponent<InterestPointDescription>().description);
                    leftHandLastSelected = hit.transform.gameObject;
                }   
            }
            else
                Debug.DrawRay(leftHand_indexEnd.position, leftPointingDirection * 1000, Color.white);

            if (showLines)
            {
                leftLine.SetPosition(0, leftHand_indexEnd.position);
                leftLine.SetPosition(1, leftHand_indexEnd.position + leftPointingDirection * 300);
            }
        }
        if (m_Manager.pointingWithRightHand)
        {
            RaycastHit hit;
            Vector3 rightPointingDirection = (rightHand_indexEnd.position - rightHand_indexStart.position).normalized;
            //Right hand pointing shoot ray!
            if (Physics.Raycast(rightHand_indexEnd.position, rightPointingDirection, out hit, Mathf.Infinity, invertedLayermask))
            {
                Debug.DrawRay(rightHand_indexEnd.position, rightPointingDirection * hit.distance, Color.yellow); Debug.Log(hit.transform.gameObject);
                if (hit.transform.gameObject != rightHandLastSelected)
                {
                    m_Manager.AddPointingAtTargetDescription_RightHand(hit.transform.GetComponent<InterestPointDescription>().description);
                    rightHandLastSelected = hit.transform.gameObject;
                }
            }
            else
                Debug.DrawRay(rightHand_indexEnd.position, rightPointingDirection * 1000, Color.white);

            if (showLines)
            {
                rightLine.SetPosition(0, rightHand_indexEnd.position);
                rightLine.SetPosition(1, rightHand_indexEnd.position + rightPointingDirection * 300);
            }
        }
    }
    public void ShowLines()
    {
        showLines = true;
        leftLine.enabled = true;
        rightLine.enabled = true;
    }

    public void HideLines()
    {
        showLines = false;
        leftLine.enabled = false;
        rightLine.enabled = false;
    }
}