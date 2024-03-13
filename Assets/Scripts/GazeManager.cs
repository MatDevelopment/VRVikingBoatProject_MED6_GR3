using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeManager : MonoBehaviour
{
    public Camera viewCamera;
    public GameObject lastGazedUpon;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        CheckGaze();
    }
    private void CheckGaze()
    {
        if (lastGazedUpon)
        {
            lastGazedUpon.SendMessage("NotGazingUpon", SendMessageOptions.DontRequireReceiver);
        }

        Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity))
        {
            hit.transform.SendMessage("GazingUpon", SendMessageOptions.DontRequireReceiver);
            lastGazedUpon = hit.transform.gameObject;
        }
    }
    
}
