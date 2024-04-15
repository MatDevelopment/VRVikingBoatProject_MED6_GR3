using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicInputUI : MonoBehaviour
{
    private MicInputDetection inputDetection;
    private float lineHeight = 0.015f;
    public Image fillImage, thresholdImage, thresholdMaskImage, iconImage;
    public float decaySpeed = 0.8f, smoothLoudnessMax = 0;
    [Range(1f, 10f)]
    public float maxAmplitudeWithCurrentMicrophone = 5f;

    //TODO:I think we need some form of calibration at the start of the experience - possibly in the tutorial level? the particpant is prompted to say some words and then we save the max amplitude then..?
    void Start()
    {
        try {
            inputDetection = FindObjectOfType<MicInputDetection>();
        } catch {
            Debug.LogError("No microphone input script found!");
        }
    }

    void Update()
    {
        //TODO:functionality to pause update of the values.
        SmoothFadeOfLoudness();
        UpdateFill();
       // UpdateThreshold();
    }

    private void SmoothFadeOfLoudness()
    {

        smoothLoudnessMax -= Time.deltaTime * decaySpeed*smoothLoudnessMax/3f;

    }
    public void UpdateFill()
    {
        if(smoothLoudnessMax< inputDetection.loudness)
        {
            smoothLoudnessMax = inputDetection.loudness;
        }
        fillImage.fillAmount = smoothLoudnessMax/maxAmplitudeWithCurrentMicrophone; 


        if (inputDetection.isListening)
        {
            iconImage.color = Color.white;
        }else
            iconImage.color = Color.black;
    }
    //public void UpdateThreshold() //TODO: threshold is not correct..!
    //{
    //    thresholdImage.fillAmount = 1 - inputDetection.threshold;
    //    thresholdMaskImage.fillAmount =  inputDetection.threshold + lineHeight ;
    //}
}