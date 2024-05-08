using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureVersionManager : MonoBehaviour
{
    public bool GestureVersion = true;

    [SerializeField] private GameObject userGestureManagerGameObject;
    // Start is called before the first frame update
    void Start()
    {
        if (GestureVersion)
        {
            userGestureManagerGameObject.SetActive(true);
        }
        else
        {
            userGestureManagerGameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
