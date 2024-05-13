using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicInputUI : MonoBehaviour // Responsible for displaying and updating the microphone UI.
{
    [SerializeField] private APIStatus apiStatus;
    [SerializeField] private MicInputDetection inputDetection;
    private float lineHeight = 0.015f;
    public Image fillImage, thresholdImage, thresholdMaskImage, iconImage;
    public TextMeshProUGUI textMeshPro;
    public float decaySpeed = 0.8f, smoothLoudnessMax = 0;
  
    //TODO:I think we need some form of calibration at the start of the experience - possibly in the tutorial level? the particpant is prompted to say some words and then we save the max amplitude then..?
    void Start()
    {
        apiStatus = FindObjectOfType<APIStatus>();
        try
        {
            inputDetection = FindObjectOfType<MicInputDetection>();
        }
        catch
        {
            Debug.LogError("No microphone input script found!");
        }
    }

    void Update()
    {
        //TODO:functionality to pause update of the values.
        SmoothFadeOfLoudness();
        UpdateFill();
        UpdateThreshold();
    }

    private void SmoothFadeOfLoudness()
    {
        smoothLoudnessMax -= Time.deltaTime * decaySpeed * smoothLoudnessMax / 3f;
    }
    public void UpdateFill()
    {
        if (smoothLoudnessMax < inputDetection.loudness)
        {
            smoothLoudnessMax = inputDetection.loudness;
        }
        fillImage.fillAmount = smoothLoudnessMax / inputDetection.maxAmplitudeWithCurrentMicrophone;


        if (inputDetection.isListening)
        {
            iconImage.color = Color.white;
        }
        else if (apiStatus.isGeneratingAudio || apiStatus.isGeneratingText || apiStatus.isTranscribing)
            iconImage.color = Color.red;
        else
        {
            iconImage.color = Color.black;
        }
    }

    public void SetText(string message)
    {
        textMeshPro.text = message;
    }

    public void SetTextAndMakeItEmptyAfterAWhile(string message)
    {
        textMeshPro.text = message;
        Invoke(nameof(setTextEmpty), 2.5f);
    }

    private void setTextEmpty()
    {
        textMeshPro.text = "";
    }
    public void UpdateThreshold()
    {
        thresholdImage.fillAmount = 1 - inputDetection.threshold/ inputDetection.maxAmplitudeWithCurrentMicrophone;
        thresholdMaskImage.fillAmount = inputDetection.threshold/ inputDetection.maxAmplitudeWithCurrentMicrophone + lineHeight;
    }
}
