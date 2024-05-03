using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] BoatRouteNavMesh boatRouteNavMesh;
    [SerializeField] BoatTilt tilter;
    [SerializeField] FadeController fadeController;
    [SerializeField] MicrophoneCalibration microphoneCalibration;
    private float fadeSpeed_Image = 0.05f, fadeStepStrengh = 0.01f;
    private bool instructionInProgress = false;
    private Instruction currentInstruction;
    //private int currentInstructionIndex = 0;
    private float instructionTimer = 0f;
    private const float countdownDuration = 3f;
    private bool userConfirmed = false;
    private bool userGaveThumbsUp, userPointedAtTheGreenCube, userCanGiveThumbsUp, userCanPointAtCube;
    //private IEnumerator countdownCoroutine;

    [Header("Tick to start with the tutorial enabled!")]
    public bool startExperienceWithTheTutorial;
    public CanvasGroup blackImageCG;
    public TextMeshProUGUI instructionText;

    //public List<Instruction> instructions = new List<Instruction>();

    void Start()
    {
        boatRouteNavMesh = FindObjectOfType<BoatRouteNavMesh>();
        tilter = FindAnyObjectByType<BoatTilt>();

        if (!startExperienceWithTheTutorial) return;

        microphoneCalibration = FindObjectOfType<MicrophoneCalibration>();

        boatRouteNavMesh.StopTheBoat();
        tilter.enabled = false;

        SetInstructionsText("Velkommen! Oplevelsen starter om lidt. Men først skal mikrofonen kalibreres. Lav en Thumbs up for at starte.");
        userCanGiveThumbsUp = true;
        //Check for pointing on a object here

        //If the user passes the check, then proceed and run the DoSection function

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && userCanGiveThumbsUp)
        {
            SetMadeThumbsUpGestureTrue();
        }
        if (Input.GetKeyDown(KeyCode.O) && userCanPointAtCube)
        {
            SetPointedAtGreenCubeTrue();
        }

        if (userPointedAtTheGreenCube)
        {
            HideTutorial();
            boatRouteNavMesh.StartTheBoat();
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

    private void EndInstruction()
    {
        Debug.Log("Ended instruction!");
        instructionInProgress = false;
        userConfirmed = false;
        instructionText.text = "";
    }

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
        blackImageCG.alpha = 1;
        instructionText.enabled = true;
    }

    private void HideTutorial()
    {
        blackImageCG.alpha = 0;
        instructionText.enabled = false;
       if(tilter != null) tilter.enabled = true;
    }

    public void SetPointedAtGreenCubeTrue()
    {
        userPointedAtTheGreenCube = true;
    }

    public void SetMadeThumbsUpGestureTrue()
    {
        userGaveThumbsUp = true;

        StartTutorial();
    }

    [System.Serializable]
    public class Instruction
    {
        public string instruction;

        public Instruction(string instruction)
        {
            this.instruction = instruction;
        }
    }
}