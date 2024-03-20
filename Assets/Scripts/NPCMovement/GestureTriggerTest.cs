using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureTriggerTest : MonoBehaviour
{
    private Animator npcAnimator;

    // Start is called before the first frame update
    void Start()
    {
        npcAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Send the message to the Animator to activate the trigger parameter named "Jump"
            npcAnimator.SetTrigger("Wave");
        }
    }
}
