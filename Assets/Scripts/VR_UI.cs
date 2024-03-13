using System;
using System.Collections;
using System.Collections.Generic;
using EnablingXR;
using UnityEngine;

public class VR_UI : MonoBehaviour
{
    public Camera cameraToFollow;
    public float cameraDistance = 3.0F;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private Transform target;
     
    void Awake()
    {
        target = cameraToFollow.transform;
    }

    private void Start()
    {
        XRController.EnableXRCoroutine();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = cameraToFollow.transform.TransformPoint(new Vector3(0, 0, cameraDistance));
       
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        var lookAtPos = new Vector3(cameraToFollow.transform.position.x, transform.position.y, cameraToFollow.transform.position.z);
        transform.LookAt(lookAtPos);
        transform.forward *= -1;
    }
}
