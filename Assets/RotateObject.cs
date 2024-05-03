using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RotateObject : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, -2, 0);
    }
}
