using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCgazemanager : MonoBehaviour
{
    [SerializeField] NewDataLogManager newDataLogManager;
    
    private float gazeTimeToActivate = 0.5f;

    private float startGazeTime = 0f;
    private float stopGazeTime = 0f;


    public void StartCountGaze()        //Called on action event of Hover Enter on NPC gaze collider
    {
        startGazeTime = Time.fixedTime;         //The time in seconds since the start of the game saved in startGazeTime float variable
    }

    public void StopGazeCount()
    {
        stopGazeTime = Time.fixedTime;          //The time in seconds since the start of the game stored in stopGazeTime, when the user stops looking at an NPC


        newDataLogManager.ErikGazeTime += (stopGazeTime - startGazeTime);
    }
}