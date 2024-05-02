using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using OpenAI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class WaveTrigger : MonoBehaviour
{
    [SerializeField] private ChatTest _chatTest;
    [FormerlySerializedAs("_dynamicGestureDetector")] [SerializeField] private DynamicGestureManager _dynamicGestureManager;
        
    [FormerlySerializedAs("rightWaveTriggerPrefab")] [SerializeField] private GameObject waveTriggerPrefab;

    private GameObject rightWaveTriggerInstance;
    private GameObject leftWaveTriggerInstance;
    
    [FormerlySerializedAs("waveTriggerParent")] [SerializeField] private Transform boatParent;

    private int waveGestureStepCount;

    private void Awake()
    {
        _dynamicGestureManager = GameObject.FindWithTag("DynamicGestureManager").GetComponent<DynamicGestureManager>();
    }

    void OnTriggerExit (Collider collider)
    {
        if (collider.gameObject.CompareTag("waveTriggerCounterTag"))
        {
            //Debug.Log("Wave INCREMENT");
            waveGestureStepCount++;
        }

        if (collider.gameObject.CompareTag("waveTriggerCounterTag") && waveGestureStepCount == 2);  //Here the wave is detected
        {
            StartCoroutine(_dynamicGestureManager.justWavedDuration_Active(3f));
            _dynamicGestureManager.wavePerformed?.Invoke();
            Debug.Log("Wave Detected");
            DestroyTriggerInstances();
            waveGestureStepCount = 0;
        }
    }
     
    public void SpawnWaveTriggers()        //Method to be called in event in inspector. Hierarchy: User Gesture Manager > Gesture Detection > Right Hand Gesture Detection > Canvas_rightImages > Custom Gesture - Wave Pose > Gesture Performed (event)
    {
        //Debug.Log("SpawnedWaveTrigger!:DDD");
        //Vector3 thisGameObjectPosition = transform.localPosition; //WRONG. Use TransformPoint to go from local space to world point.
        Vector3 rightWaveTriggerPosition = transform.TransformPoint(Vector3.right * 1.8f);
        Vector3 leftWaveTriggerPosition = transform.TransformPoint(Vector3.left * 1.8f);
        
        rightWaveTriggerInstance = Instantiate(waveTriggerPrefab, rightWaveTriggerPosition, transform.rotation, boatParent);
        leftWaveTriggerInstance = Instantiate(waveTriggerPrefab, leftWaveTriggerPosition, transform.rotation, boatParent);
        
        Destroy(rightWaveTriggerInstance, 2f);
        Destroy(leftWaveTriggerInstance, 2f);
    }

    public void DestroyTriggerInstances()
    {
        Destroy(rightWaveTriggerInstance);
        Destroy(leftWaveTriggerInstance);
    }

    public void StartCoroutine_DestroyTriggerInstancesAfterDelay()
    {
        StopAllCoroutines();
        StartCoroutine(DestroyTriggerInstancesAfterDelay());
    }
    
    private IEnumerator DestroyTriggerInstancesAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(rightWaveTriggerInstance);
        Destroy(leftWaveTriggerInstance);
    }
}
