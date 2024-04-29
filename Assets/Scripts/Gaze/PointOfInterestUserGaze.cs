using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestUserGaze : MonoBehaviour
{

    [SerializeField] private NPCInteractorScript _npcInteractorScript;
    
    private float gazeTimeToActivate;
    
    private string pointOfInterestDescription;

    private void Start()
    {
        _npcInteractorScript = GameObject.FindWithTag("erikNpcInteractorScript").GetComponent<NPCInteractorScript>();
        pointOfInterestDescription = GetComponentInParent<InterestPointDescription>().description;
        gazeTimeToActivate = GetComponentInParent<InterestPointDescription>().gazeTimeToActivate;
    }

    public void StartCoroutine_RegisterUserGazeAtPointOfInterest()
    {
        StartCoroutine(RegisterUserGazeAtPointOfInterest(gazeTimeToActivate, pointOfInterestDescription));
    }
    
    private IEnumerator RegisterUserGazeAtPointOfInterest(float gazeTimeToActivate, string userGazeDescription)
    {
        yield return new WaitForSeconds(gazeTimeToActivate);
        
        _npcInteractorScript.InformAndInitiateNpcTalk(userGazeDescription);
    }
    
}
