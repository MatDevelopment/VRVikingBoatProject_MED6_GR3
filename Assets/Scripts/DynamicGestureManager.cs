using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicGestureManager : MonoBehaviour
{
    public bool justWaved;
    [SerializeField] public UnityEvent wavePerformed;
    
    public IEnumerator justWavedDuration_Active(float duration)
    {
        justWaved = true;
        yield return new WaitForSeconds(duration);
        justWaved = false;
    }
    
    
    
    
    
}
