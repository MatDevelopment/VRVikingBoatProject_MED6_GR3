using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public GameObject tutorialRoom, greenCube;
    public Light tutorialLight;

    [Header("Tick to start with the tutorial enabled!")]
    public bool startExperienceWithTheTutorial;
    //public CanvasGroup blackImageCG;
    public TextMeshProUGUI instructionText;


    void Start()
    {
        fadeController = FindObjectOfType<FadeController>();
        boatRouteNavMesh = FindObjectOfType<BoatRouteNavMesh>();
        tilter = FindAnyObjectByType<BoatTilt>();

        if (!startExperienceWithTheTutorial) return;

        microphoneCalibration = FindObjectOfType<MicrophoneCalibration>();

        boatRouteNavMesh.StopTheBoat();
        tilter.enabled = false;

        SetInstructionsText("Velkommen! Oplevelsen starter om lidt. Men først skal mikrofonen kalibreres. Lav en Thumbs up for at starte.");
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
            userCanGiveThumbsUp = true;
            Step_CheckForPointing();
            SetMadeThumbsUpGestureTrue();
        }
        if (Input.GetKeyDown(KeyCode.O) /*&& userCanPointAtCube*/)
        {
            userCanPointAtCube = true;
            SetPointedAtGreenCubeTrue();
        }

        if (userPointedAtTheGreenCube)
        {
            FadeOutTutorial();
            boatRouteNavMesh.StartTheBoat(1);
        }
    }

    private IEnumerator DoSection( float delay )
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

        StartCoroutine(microphoneCalibration.ListenForLoudnessForDuration(3));
        SetInstructionsText("Jeg kan godt lide vikinger. De har nogle sjove hatte på");
        yield return new WaitForSeconds(3);

        if (userConfirmed)
        {
            SetInstructionsText("Kalibrering færdig!");
            yield return new WaitForSeconds(2);

            Step_CheckForPointing();
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
        SetInstructionsText("Om 5 sekunder skal du sige en saetning. Er du klar?");

        StartCoroutine(DoSection(3));
    }

    private void Step_CheckForPointing()
    {
        userCanPointAtCube = true;
        ShowGreenCube();
        SetInstructionsText("Nu er du klar - peg på den grønne terning for at starte");
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
            SetInstructionsText ("Om " + i + " sekunder skal du sige en saetning. Er du klar?");
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
    }

    private void ShowTutorial()
    {
        //blackImageCG.alpha = 1;
        instructionText.enabled = true;
    }

    private void HideTutorial()
    {
        //blackImageCG.alpha = 0;
        instructionText.enabled = false;
        if (tilter != null) tilter.enabled = true;

    }

    private void FadeOutTutorial()
    {
        //blackImageCG.alpha = 0;
        instructionText.enabled = false;
        if (tilter != null) tilter.enabled = true;
        StartCoroutine(FadeRoomMaterials());
        StartCoroutine(boatRouteNavMesh.StartTheBoat(1));
        StartCoroutine(FadeRoomMaterials());
        
        //fadeController.FadeOutAfterTime(1);
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
        foreach(Transform gm in tutorialRoom.GetComponentsInChildren<Transform>())
        {
            gm.gameObject.SetActive(false);
        }
        dataLogManager.timeThatTutorialEnded = Time.time;

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

    public void SetLookedAtObjectTrue()
    {
        if (userCanGiveThumbsUp && !userGaveThumbsUp)
        {
            userGaveThumbsUp = true;

            StartTutorial();
        }
    }

    private void ShowGreenCube()
    {
        greenCube.SetActive(true);
    }
    private void HideGreenCube()
    {
        greenCube.SetActive(false);
    }
}