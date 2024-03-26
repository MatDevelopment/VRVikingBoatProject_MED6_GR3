using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBodyScaler : MonoBehaviour
{
    private IEnumerator scalerCoroutine;
    private float threshold = 0.4f;
    private float ticktime = 0.05f;
    private Vector3 scaleToAdd = new Vector3(0.1f, 0.1f, 0.1f);
    public Transform virtualBodyTransform, trackingPoint, startOfWrist;
    public float distBetweenPoints;
    
    void Start()
    {
        scalerCoroutine = Scaler(); // reference to the coroutine so it can be easily stopped later.
                StartCoroutine(scalerCoroutine);
    }

    private IEnumerator Scaler()
    {
        while (true)
        {
            yield return new WaitForSeconds(ticktime);

            distBetweenPoints = Vector3.Distance(trackingPoint.position, startOfWrist.position);
            if (distBetweenPoints > threshold)
            {
                virtualBodyTransform.localScale += scaleToAdd;
                Vector3 smaller = virtualBodyTransform.localScale -= scaleToAdd;

                Vector3 bigger = virtualBodyTransform.localScale += scaleToAdd;

                //if(Vector3.Distance())
            }
            //else if (distBetweenPoints < threshold)
            //{
            //    virtualBodyTransform.localScale += scaleToAdd;
            //}
        }
    }
}
