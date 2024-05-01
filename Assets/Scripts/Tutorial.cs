using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] MicrophoneCalibration microphoneCalibration;
    private float fadeSpeed_Image = 0.05f, fadeStepStrengh = 0.01f;
    private bool instructionInProgress = false;
    private Instruction currentInstruction;
    //private int currentInstructionIndex = 0;
    private float instructionTimer = 0f;
    private const float countdownDuration = 3f;
    private bool userConfirmed = false;
    private bool userGaveThumbsUp, userPointedAtTheGreenCube;
    //private IEnumerator countdownCoroutine;

    [Header("Tick to start with the tutorial enabled!")]
    public bool startExperienceWithTheTutorial;
    public CanvasGroup blackImageCG;
    public TextMeshProUGUI instructionText;

    //public List<Instruction> instructions = new List<Instruction>();

    void Start()
    {
        if (!startExperienceWithTheTutorial) return;

        microphoneCalibration = FindObjectOfType<MicrophoneCalibration>();

        SetInstructionsText("Velkommen! Oplevelsen starter om lidt. Men først skal mikrofonen kalibreres. Lav en Thumbs up for at starte.");

        //Check for pointing on a object here

        //If the user passes the check, then proceed and run the DoSection function

        //
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartTutorial();
        }
    }
    private IEnumerator DoSection( float delay )
    {
        yield return new WaitForSeconds(delay);

        Debug.Log("Started instruction!");
        instructionInProgress = true;
        instructionTimer = 0f;
        userConfirmed = false;

        StartCoroutine(Countdown(3));

        yield return new WaitForSeconds(3);

        StartCoroutine(microphoneCalibration.ListenForLoudnessForDuration(3));
        SetInstructionsText("Jeg kan godt lide vikinger. De har nogle sjove hatte på");
        yield return new WaitForSeconds(3);

        if (userConfirmed)
        {

        }
    }

    public void StartTutorial()
    {
        userGaveThumbsUp = true;

        SetInstructionsText("Om 5 sekunder, sig sætningen: Jeg kan godt lide vikinger. De har nogle sjove hatte på");

        StartCoroutine(DoSection(1));
    }
    //private IEnumerator DoSection()
    //{
    //    StartInstruction(instructions[currentInstructionIndex]);

    //    yield return new WaitForSeconds(3);

    //    StartCoroutine(microphoneCalibration.ListenForLoudnessForDuration(3));

    //    yield return new WaitForSeconds(3);

    //    if (userConfirmed)
    //    {
    //        EndInstruction();
    //        if (currentInstructionIndex < instructions.Count)
    //        {
    //            currentInstructionIndex++;
    //            StartCoroutine(DoSection());
    //            Debug.Log("Do next section!");

    //        }
    //        else
    //        {
    //            HideTutorial();
    //        }
    //    }
    //    else
    //    {
    //        StartCoroutine(DoSection());
    //    }
    //}

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
        Debug.Log("Started Coundown!");
        for (int i = secondsToCountDown; i > 0; i--)
        {
            SetInstructionsText(currentInstruction.instruction + "\n in " + i + " seconds");
            yield return new WaitForSeconds(1f);
        }

        SetInstructionsText(currentInstruction.instruction);
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