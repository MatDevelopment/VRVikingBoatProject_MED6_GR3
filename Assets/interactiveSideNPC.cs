using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using WaterSystem.Data;

public class interactiveSideNPC : MonoBehaviour
{
    [SerializeField] private DynamicGestureManager _dynamicGestureManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _lookTarget;

    private float lookWeight = 0f;
    private float handWeight = 0f;
    public Transform pointRotation;

    public bool waveDebug = false;
    private bool currentlyLookingAtNpc = false;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _dynamicGestureManager = GameObject.FindWithTag("DynamicGestureManager").GetComponent<DynamicGestureManager>();
    }

    public void StartCoroutine_currentlyLookingAtNpcDuration_Active()
    {
        StopAllCoroutines();
        StartCoroutine(currentlyLookingAtNpcDuration_Active(3f));
    }

    public void doWaveAnimation()
    {
        if (_dynamicGestureManager.justWaved && currentlyLookingAtNpc)      //USE SOMETHING ELSE THAN A STATIC. UNITY IS CRASHING:( Or maybe it is the TestLogs being produced? (Memory leak)
        {
            StartCoroutine(lookAtPlayer());
            Debug.Log("*NPC waves back*");      //This is where to play a wave animation on the NPC
        }
        
    }

    /*public void SetFalse_currentlyLookingAtNpc()
    {
        currentlyLookingAtNpc = false;
    }*/
    
    public IEnumerator currentlyLookingAtNpcDuration_Active(float duration)
    {
        currentlyLookingAtNpc = true;
        yield return new WaitForSeconds(duration);
        currentlyLookingAtNpc = false;
    }

    private void Update()
    {
        // Check rotation to look target
        var lookPos = _lookTarget.position - pointRotation.transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        rotation *= Quaternion.Euler(-90f, 0f, 0f);
        pointRotation.rotation = rotation;

        // Trigger in the inspector to test look mechanic
        if (waveDebug)
        {
            StartCoroutine(lookAtPlayer());
            waveDebug = false;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // Setting Looking IK
        _animator.SetLookAtWeight(lookWeight, 0.25f, 0.75f, 0.75f, 0.7f);
        _animator.SetLookAtPosition(_lookTarget.position);

        // Setting Hand IK
        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handWeight);
        _animator.SetIKPosition(AvatarIKGoal.RightHand, _lookTarget.position);
        _animator.SetIKRotation(AvatarIKGoal.RightHand, pointRotation.rotation);
    }

    private IEnumerator lookAtPlayer()
    {
        float time = 0;
        float duration = 1;

        float startValue = 0;
        float endValue = 1;
        float handEndValue = 0.25f;

        while (time < duration)
        {
            lookWeight = Mathf.Lerp(startValue, endValue, time / duration);
            handWeight = Mathf.Lerp(startValue, handEndValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        //handWeight = handEndValue;
        _animator.SetTrigger("Wave");
        yield return new WaitForSeconds(2);

        time = 0;
        while (time < duration)
        {
            lookWeight = Mathf.Lerp(endValue, startValue, time / duration);
            handWeight = Mathf.Lerp(handEndValue, startValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        lookWeight = startValue;
        handWeight = startValue;
    }
}
