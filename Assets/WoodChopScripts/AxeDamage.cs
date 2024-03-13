using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    // This code is inspired by 
    // https://www.youtube.com/watch?v=tPWMZ4Ic7PA&list=PLYuJT1wgJunlRwTk-q6EIL5kb3trRnFq0&index=9&t=157s&ab_channel=CodeMonkey

    public Transform rayPoistion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;

        if (Physics.Raycast(rayPoistion.position, rayPoistion.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.distance < 0.05f)
            {
                if (hit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    damageable.Damage(50, hit.point);
                }
            }
            /*else
            {
                Debug.Log($"{hit.distance}");
            }*/

            Debug.DrawRay(rayPoistion.position, rayPoistion.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
        }

         else
        {
            Debug.DrawRay(rayPoistion.position, rayPoistion.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
        
    }
}
