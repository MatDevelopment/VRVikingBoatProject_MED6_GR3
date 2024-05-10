using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] NewDataLogManager dataLogManager;
    [SerializeField] BoatRouteNavMesh boatRouteNavMesh;
    [SerializeField] BoatTilt tilter;
    [SerializeField] FadeController fadeController;
    [SerializeField] MicrophoneCalibration microphoneCalibration;
    private float fadeSpeed_Image = 0.05f, fadeStepStrengh = 0.01f;
    private bool instructionInProgress = false;

    //private int currentInstructionIndex = 0;
    private float instructionTimer = 0f;
    private const float countdownDuration = 3f;
    private bool userConfirmed = false;
    private bool userGaveThumbsUp, userPointedAtTheGreenCube, userCanGiveThumbsUp, userCanPointAtCube;
    //private IEnumerator countdownCoroutine;
    public GameObject tutorialRoom, greenCube, gazeCube1, gazeCube2;
    public Light tutorialLight;

    [Header("Tick to start with the tutorial enabled!")]
    public bool startExperienceWithTheTutorial, useGesturesForConfirmation;
    //public CanvasGroup blackImageCG;
    public TextMeshProUGUI instructionText;

    public bool lookingAtGazeCube1, lookingAtGazeCube2;
    public Slider gaze1Slider, gaze2Slider;

    private bool CanLookAtCube2;
  
    private bool userStartedCalibrationStep;
    [SerializeField] private GestureVersionManager gestureVersionManager;

    void Start()
    {

        fadeController = FindObjectOfType<FadeController>();
        boatRouteNavMesh = FindObjectOfType<BoatRouteNavMesh>();
        tilter = FindAnyObjectByType<BoatTilt>();

        if (!startExperienceWithTheTutorial) return;

        microphoneCalibration = FindObjectOfType<MicrophoneCalibration>();

        boatRouteNavMesh.StopTheBoat();
        tilter.enabled = false;

        if (useGesturesForConfirmation)
        {

            SetInstructionsText("Velkommen! Oplevelsen starter om lidt. Men først skal mikrofonen kalibreres. Lav en Thumbs up for at starte.");
        }
        else if (!useGesturesForConfirmation)
        {
            SetInstructionsText("Velkommen! Oplevelsen starter om lidt. Men først skal mikrofonen kalibreres. Kig på den orange terning for at starte.");
            gazeCube2.SetActive(false);
            gaze2Slider.transform.parent.gameObject.SetActive(false);
        }

        if (useGesturesForConfirmation)
            userCanGiveThumbsUp = true;
        HideGreenCube();
        //Check for pointing on a object here

        //If the user passes the check, then proceed and run the DoSection function

        //FadeOutTutorial();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) /*&& userCanGiveThumbsUp*/)
        {
            if (useGesturesForConfirmation)
            {
                userCanGiveThumbsUp = true;
                Step_CheckForPointing();
                SetMadeThumbsUpGestureTrue();
            }
            else
            {
                SetLookedAtGazecube1True();
            }
        }
        if (Input.GetKeyDown(KeyCode.O) /*&& userCanPointAtCube*/)
        {
            if (useGesturesForConfirmation)
            {
                userCanPointAtCube = true;
                SetPointedAtGreenCubeTrue();
            }
            else if (!useGesturesForConfirmation)
            {
                Step_CheckForLookAtGaze2();
            }
        }
        if (userPointedAtTheGreenCube)
        {
            FadeOutTutorial();
            boatRouteNavMesh.StartTheBoat(1);
        }

        if (lookingAtGazeCube1)
            gaze1Slider.value += Time.deltaTime;
        else if (!lookingAtGazeCube1)
            gaze1Slider.value = 0;

        if (gaze1Slider.value >= 1 && !userStartedCalibrationStep)
            SetLookedAtGazecube1True();


        if (lookingAtGazeCube2)
            gaze2Slider.value += Time.deltaTime;
        else if (!lookingAtGazeCube2)
            gaze2Slider.value = 0;

        if (gaze2Slider.value >= 1 && !gestureVersionManager.tutorialDone)
            FadeOutTutorial();

    }

    private IEnumerator DoSection(float delay)
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Started instruction!");
        instructionInProgress = true;
        instructionTimer = 0f;
        userConfirmed = false;

        StartCoroutine(TryCalibrationStep());
    }

    private IEnumerator TryCalibrationStep()
    {
        StartCoroutine(Countdown(5));

        yield return new WaitForSeconds(5);

        StartCoroutine(microphoneCalibration.ListenForLoudnessForDuration(8));
        SetInstructionsText("Jeg kan godt lide vikinger. De har nogle sjove hatte på");
        yield return new WaitForSeconds(8);

        if (userConfirmed)
        {
            SetInstructionsText("Kalibrering færdig!");
            yield return new WaitForSeconds(2);

            if (useGesturesForConfirmation)
                Step_CheckForPointing();
            else if (!useGesturesForConfirmation)
            {
                Step_CheckForLookAtGaze2();

            }
        }
        else
        {
            SetInstructionsText("Kalibrering mislykkedes - volumen for lav. Prøv igen.");
            yield return new WaitForSeconds(2);
            StartCoroutine(TryCalibrationStep());
        }
    }

    public void StartTutorial()
    {
        SetInstructionsText("Om 5 sekunder skal du sige en sætning. Er du klar?");

        StartCoroutine(DoSection(3));
    }

    private void Step_CheckForPointing()
    {
        userCanPointAtCube = true;
        ShowGreenCube();
        SetInstructionsText("Nu er du klar - peg på den grønne terning for at starte");
    }

    private void Step_CheckForLookAtGaze2()
    {
        //userCanPointAtCube = true;
        gazeCube2.SetActive(true);
        gaze2Slider.transform.parent.gameObject.SetActive(true);
        CanLookAtCube2 = true;
        SetInstructionsText("Nu er du klar - kig på den lilla terning for at starte");
    }


    public void UserTalkedLoudEnough()
    {
        userConfirmed = true;
        instructionInProgress = false;
    }

    public void UserDidNotTalkLoudEnough()
    {
        userConfirmed = false;
        instructionInProgress = true;
    }

    private IEnumerator Countdown(int secondsToCountDown)
    {
        Debug.Log("Started A Countdown!");
        for (int i = secondsToCountDown; i > 0; i--)
        {
            SetInstructionsText("Om " + i + " sekunder skal du sige en sætning. Er du klar?");
            yield return new WaitForSeconds(1f);
        }
    }

    //private void EndInstruction()
    //{
    //    Debug.Log("Ended instruction!");
    //    instructionInProgress = false;
    //    userConfirmed = false;
    //    instructionText.text = "";
    //}

    private void SetInstructionsText(string input)
    {
        instructionText.text = input;
    }

    public void OnValidate() // Called everytime an update in the inspector UI happens. for example ticking a bool check box!
    {
        if (startExperienceWithTheTutorial) ShowTutorial();
        else if (!startExperienceWithTheTutorial) HideTutorial();

        if (startExperienceWithTheTutorial && useGesturesForConfirmation)
            ShowGestureObjects();
        else if (startExperienceWithTheTutorial && !useGesturesForConfirmation)
            ShowGazeObjects();
    }

    private void ShowTutorial()
    {

        tutorialRoom.SetActive(true);
        //blackImageCG.alpha = 1;
        instructionText.enabled = true;
    }

    private void HideTutorial()
    {
        tutorialRoom.SetActive(false);
        //blackImageCG.alpha = 0;
        instructionText.enabled = false;
        if (tilter != null) tilter.enabled = true;

    }

    private void ShowGestureObjects()
    {
        greenCube.SetActive(true);
        gazeCube1.SetActive(false);
        gazeCube2.SetActive(false);
        gaze1Slider.transform.parent.gameObject.SetActive(false);
        gaze2Slider.transform.parent.gameObject.SetActive(false);
    }

    private void ShowGazeObjects()
    {
        greenCube.SetActive(false);
        gazeCube1.SetActive(true);
        gazeCube2.SetActive(false);
        gaze1Slider.transform.parent.gameObject.SetActive(true);
        if (CanLookAtCube2 == true)
            gaze2Slider.transform.parent.gameObject.SetActive(true);
        else
            gaze2Slider.transform.parent.gameObject.SetActive(false);
    }

    private void FadeOutTutorial()
    {
        //blackImageCG.alpha = 0;
        instructionText.enabled = false;
        if (tilter != null) tilter.enabled = true;
        StartCoroutine(FadeRoomMaterials());
        StartCoroutine(boatRouteNavMesh.StartTheBoat(1));
        //StartCoroutine(FadeRoomMaterials());

        //fadeController.FadeOutAfterTime(1);

    }

    public void SetLookingAtGazeCube1True()
    {
        lookingAtGazeCube1 = true;
    }

    public void SetLookingAtGazeCube2True()
    {
        lookingAtGazeCube2 = true;
    }

    public void SetLookingAtGazeCube1False()
    {
        lookingAtGazeCube1 = false;
    }
    public void SetLookingAtGazeCube2False()
    {
        lookingAtGazeCube2 = false;
    }


    IEnumerator FadeRoomMaterials()
    {
        float fadeDuration = 0.7f;


        Material[] mats = tutorialRoom.GetComponent<MeshRenderer>().materials;

        Color originalColor = mats[0].color; // Assuming the object has at least one material
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            Color currentColor = Color.Lerp(originalColor, transparentColor, t);

            foreach (Material mat in mats)
            {
                mat.color = currentColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 100;
        // Ensure the final color is fully transparent
        foreach (Material mat in mats)
        {
            mat.color = transparentColor;
        }

        DestroyTutorialRoom();
        //Invoke(nameof(DestroyTutorialRoom), 1);
        //Destroy(tutorialRoom); // Destroy the GameObject when the fading is complete
    }

    private void DestroyTutorialRoom()
    {
        if (gestureVersionManager.tutorialDone) return;
        StopCoroutine(nameof(FadeRoomMaterials));
        tutorialRoom.SetActive(false);
        dataLogManager.timeThatTutorialEnded = Time.time;

        gestureVersionManager.tutorialDone = true;

        //Destroy(tutorialRoom);
    }

    public void SetPointedAtGreenCubeTrue()
    {
        if (userCanPointAtCube && !userPointedAtTheGreenCube)
        {
            Invoke(nameof(InvokedSetTrue), 1.5f);

            Destroy(greenCube);
            //fadeController.FadeIn();

            GameObject greenParticles = Instantiate(Resources.Load("GreenParticles"), greenCube.transform.position, Quaternion.identity) as GameObject;
            Destroy(greenParticles, 3);
        }
    }

    private void InvokedSetTrue()
    {
        userPointedAtTheGreenCube = true;
    }



    public void SetMadeThumbsUpGestureTrue()
    {
        if (userCanGiveThumbsUp && !userGaveThumbsUp)
        {
            userGaveThumbsUp = true;

            StartTutorial();
        }
    }

    public void SetLookedAtGazecube1True()
    {
        gazeCube1.SetActive(false);
        gaze1Slider.transform.parent.gameObject.SetActive(false);
        StartTutorial();
        userStartedCalibrationStep = true;
    }

    public void SetLookedAtGazecube2True()
    {
        gazeCube2.SetActive(false);
        gaze2Slider.transform.parent.gameObject.SetActive(false);
        StartTutorial();
    }

    private void ShowGreenCube()
    {
        if (greenCube != null)
            greenCube.SetActive(true);
    }
    private void HideGreenCube()
    {
        if (greenCube != null)
            greenCube.SetActive(false);
    }
}