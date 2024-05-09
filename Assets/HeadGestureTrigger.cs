using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;

public class HeadGestureTrigger : MonoBehaviour
{
    [SerializeField] private ChatTest _chatTest;
    [SerializeField] private DynamicGestureManager _dynamicGestureManager;
    
    //Prefabs used for placing at the relevant positions for HEADSHAKE and HEADNOD individually. Only relevant for gameobject tag reading.
    [SerializeField] private GameObject topBottomTriggerPrefab;
    [SerializeField] private GameObject leftRightTriggerPrefab;

    //Top/bottom colliders used for detecting HEADNOD
    private GameObject topHeadTriggerInstance;
    private GameObject bottomHeadTriggerInstance;
    
    //Left/right colliders used for detecting HEADSHAKE
    private GameObject rightHeadTriggerInstance;
    private GameObject leftHeadTriggerInstance;

    private int topBottomHead_Counter;
    private int leftRightHead_Counter;
    
    [SerializeField] private Transform boatParent;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _dynamicGestureManager = GameObject.FindWithTag("DynamicGestureManager").GetComponent<DynamicGestureManager>();
    }

    void OnTriggerExit (Collider collider)
    {
        //HEADNOD DETECTION
        if (collider.gameObject.CompareTag("topBottomHeadTriggerTag"))
        {
            topBottomHead_Counter++;
            
            if (topBottomHead_Counter == 2)
            {
                //StartCoroutine(_dynamicGestureManager.justWavedDuration_Active(3f));
                //_dynamicGestureManager.wavePerformed?.Invoke();
                Debug.Log("HEADNOD Detected");
                _chatTest.AddSystemInstructionToChatLog("The Traveller just did a headnod in acknowledgement of what Erik has just said. Erik will take this into consideration.");
                DestroyTopBottomTriggersInstances();
                topBottomHead_Counter = 0;
            }
        }
        
        
        //HEADSHAKE DETECTION
        if (collider.gameObject.CompareTag("leftRightHeadTriggerTag"))
        {
            topBottomHead_Counter++;

            if (leftRightHead_Counter == 2)
            {
                //StartCoroutine(_dynamicGestureManager.justWavedDuration_Active(3f));
                //_dynamicGestureManager.wavePerformed?.Invoke();
                Debug.Log("HEADSHAKE Detected");
                _chatTest.AddSystemInstructionToChatLog("The Traveller just did a headshake in disagreement of what Erik has just said. Erik will take this into consideration.");
                DestroyLeftRightTriggersInstances();
                leftRightHead_Counter = 0;
            }
        }
    }
    
    public void SpawnHeadGestureTriggers(float timeUntilTriggerDestroy)        //Method to be called in event in inspector. Hierarchy: User Gesture Manager > Gesture Detection > Right Hand Gesture Detection > Canvas_rightImages > Custom Gesture - Wave Pose > Gesture Performed (event)
    {
        //Debug.Log("SpawnedWaveTrigger!:DDD");
        //Vector3 thisGameObjectPosition = transform.localPosition; //WRONG. Use TransformPoint to go from local space to world point.
        
        //Right/left head trigger positions
        Vector3 rightHeadTriggerPosition = transform.TransformPoint(Vector3.right * 1.5f);
        Vector3 leftHeadTriggerPosition = transform.TransformPoint(Vector3.left * 1.5f);
        
        //Top/bottom head trigger positions
        Vector3 topHeadTriggerPosition = transform.TransformPoint(Vector3.up * 1.5f);
        Vector3 bottomHeadTriggerPosition = transform.TransformPoint(Vector3.down * 1.5f);
        
        /*Vector3 topHeadTriggerPosition = transform.TransformPoint(transform.position.x, transform.position.y, transform.position.z);
        Vector3 bottomHeadTriggerPosition = transform.TransformPoint(transform.position.x, transform.position.y + 0.2f, transform.position.z);*/
        
        rightHeadTriggerInstance = Instantiate(leftRightTriggerPrefab, rightHeadTriggerPosition, transform.rotation, boatParent);
        leftHeadTriggerInstance = Instantiate(leftRightTriggerPrefab, leftHeadTriggerPosition, transform.rotation, boatParent);
        
        topHeadTriggerInstance = Instantiate(topBottomTriggerPrefab, topHeadTriggerPosition, transform.rotation, boatParent);
        bottomHeadTriggerInstance = Instantiate(topBottomTriggerPrefab, bottomHeadTriggerPosition, transform.rotation, boatParent);
        
        Destroy(rightHeadTriggerInstance, timeUntilTriggerDestroy);
        Destroy(leftHeadTriggerInstance, timeUntilTriggerDestroy);
        Destroy(topHeadTriggerInstance, timeUntilTriggerDestroy);
        Destroy(bottomHeadTriggerInstance, timeUntilTriggerDestroy);
    }
    
    public void DestroyTopBottomTriggersInstances()
    {
        Destroy(topHeadTriggerInstance);
        Destroy(bottomHeadTriggerInstance);
    }
    public void DestroyLeftRightTriggersInstances()
    {
        Destroy(rightHeadTriggerInstance);
        Destroy(leftHeadTriggerInstance);
    }
    
    
    private IEnumerator DestroyTriggerInstancesAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(rightHeadTriggerInstance);
        Destroy(leftHeadTriggerInstance);
        Destroy(topHeadTriggerInstance);
        Destroy(bottomHeadTriggerInstance);
    }
}
