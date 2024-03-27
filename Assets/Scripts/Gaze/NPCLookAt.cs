using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPCLookAt : MonoBehaviour
{
    
    [SerializeField] private Animator npcAnimator;
    [FormerlySerializedAs("lookTarget")] [SerializeField] private GameObject travellerLookTarget;
    //[SerializeField] private GameObject funeralPyreLookTarget;
    //[SerializeField] private LevelChanger levelChangerScript;

    //private bool gazeChangedToFuneralPyre = false;

    private void Awake()
    {
        //levelChangerScript = GameObject.FindWithTag("LevelChanger").GetComponent<LevelChanger>();
    }

    private void OnAnimatorIK()
    {
        if (npcAnimator.enabled)
        {
            npcAnimator.SetLookAtWeight(1, 0, 0.75f, 0.75f, 0.7f);
            npcAnimator.SetLookAtPosition(travellerLookTarget.transform.position);
        }
        else
        {
            npcAnimator.SetLookAtWeight(0);
        }
    }

    private void Update()
    {
        /*if (levelChangerScript.Scene4Active == true && levelChangerScript.FuneralPyreLit == true)
        {
            if (gazeChangedToFuneralPyre == false)
            {
                travellerLookTarget = funeralPyreLookTarget;
                gazeChangedToFuneralPyre = true;
            }
        }*/
    }
}
