using System;
using System.Collections;
using System.Collections.Generic;
using EnablingXR;
using UnityEngine;

public class DialogueUILookAt : MonoBehaviour
{
    public Camera cameraToFollow;
    public float smoothTime = 0.3F;

    private void Start()
    {
        XRController.EnableXRCoroutine();
    }

    // Update is called once per frame
    void Update()
    {
        var lookAtPos = new Vector3(cameraToFollow.transform.position.x, transform.position.y, cameraToFollow.transform.position.z);
        transform.LookAt(lookAtPos);
        transform.forward *= -1;
        transform.position = new Vector3(transform.position.x, cameraToFollow.transform.position.y, transform.position.z);
    }
}
