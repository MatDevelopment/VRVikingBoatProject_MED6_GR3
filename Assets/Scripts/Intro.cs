using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Intro : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TextMeshProUGUI FirstLine;
    public TextMeshProUGUI SecondLine;
    public TextMeshProUGUI ThirdLine;

    public AudioSource[] EnvironmentSounds;
    public AudioSource Blacksmith;
    public AudioSource Wind;

    public CanvasGroup introUI;
    bool IntroIsOver = false;

    private float secondsBetweenFades = 5;

    float EnvironmentVolume = 0;
    float MusicVolume = 0.5f;
    float fadeDelay = 0.2f;

    private void Start()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        FadeInText(FirstLine);
        yield return new WaitForSeconds(secondsBetweenFades);

        FadeOutText(FirstLine);
        FadeInText(SecondLine);

        yield return new WaitForSeconds(secondsBetweenFades);

        FadeOutText(SecondLine);
        FadeInText(ThirdLine);

        yield return new WaitForSeconds(secondsBetweenFades);

        FadeOutText(ThirdLine);
        EnvironmentVolume = 1;
        IntroIsOver = false;
    }

    private void Update()
    {
        foreach (AudioSource Audio in EnvironmentSounds)
        {
            if (Audio.volume != EnvironmentVolume)
            {
                Audio.volume = Mathf.MoveTowards(Audio.volume, EnvironmentVolume, fadeDelay * Time.deltaTime);
            }
        }

        if (IntroIsOver)
        {
            introUI.alpha = Mathf.MoveTowards(introUI.alpha, 0, fadeDelay * Time.deltaTime);
            
            if (introUI.alpha == 0)
            {
                IntroIsOver = false;
            }
        }
    }

    void FadeInText(TextMeshProUGUI Text)
    {
        Text.alpha = Mathf.Lerp(Text.alpha, 1, fadeDelay * Time.deltaTime);
    }


    void FadeOutText(TextMeshProUGUI Text)
    {
        Text.alpha = Mathf.Lerp(Text.alpha, 0, fadeDelay * Time.deltaTime);
    }
}
