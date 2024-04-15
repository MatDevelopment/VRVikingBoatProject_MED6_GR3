using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTilt : MonoBehaviour
{
    //Inspiration https://discussions.unity.com/t/how-to-make-a-sine-wave-with-a-transform/68170/2 
    [SerializeField] private GameObject Boat;
    [SerializeField] private float amplitudeXRotation = 1.0f;
    [SerializeField] private float amplitudeZRotation = 1.0f;
    [SerializeField] private float amplitudeYPosition = 0.3f;
    private float omegaXR = 1.0f;
    private float omegaZR = 1.0f;
    private float omegaYP = 1.0f;
    private float index;

    // Update is called once per frame
    void Update()
    {
        index += Time.deltaTime;
        float x = amplitudeXRotation*Mathf.Cos(omegaXR*index);
        float z = amplitudeZRotation*Mathf.Cos(omegaZR*index);
        float y = amplitudeYPosition*Mathf.Sin(omegaYP*index);

        Boat.transform.Rotate(x,0,z, Space.Self);

        Boat.transform.position = new Vector3(Boat.transform.position.x, y + 1, Boat.transform.position.z);

    }
}
