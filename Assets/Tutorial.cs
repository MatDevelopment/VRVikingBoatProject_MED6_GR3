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
    private int currentInstructionIndex = 0;
    private float instructionTimer = 0f;
    private const float countdownDuration = 3f;
    private bool userConfirmed = false;
    private bool userGaveThumbsUp;
    //private IEnumerator countdownCoroutine;

    [Header("Tick to start with the tutorial enabled!")]
    public bool startExperienceWithTheTutorial;
    public CanvasGroup blackImageCG;
    public TextMeshProUGUI instructionText;

    public List<Instruction> instructions = new List<Instruction>();

    void Start()
    {

        if (!startExperienceWithTheTutorial) return;

        microphoneCalibration = FindObjectOfType<MicrophoneCalibration>();

        SetInstructionsText("Velkommen! Oplevelsen starter om lidt. Men først skal mikrofonen kalibreres. Peg på den grønne terning for at starte.");

        //Check for pointing on a object here

        //If the user passes the check, then proceed and run the DoSection function

        //


        StartCoroutine(DoSection());
    }

    private IEnumerator DoSection()
    {
        StartInstruction(instructions[currentInstructionIndex]);

        yield return new WaitForSeconds(3);

        StartCoroutine(microphoneCalibration.ListenForLoudnessForDuration(3));

        yield return new WaitForSeconds(3);

        if (userConfirmed)
        {
            EndInstruction();
            if (currentInstructionIndex < instructions.Count)
            {
                currentInstructionIndex++;
                StartCoroutine(DoSection());
                Debug.Log("Do next section!");

            }
            else
            {
                HideTutorial();
            }
        }
        else
        {
            StartCoroutine(DoSection());
        }
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

    private void StartInstruction(Instruction instruction)
    {
        Debug.Log("Started instruction!");
        currentInstruction = instruction;
        instructionInProgress = true;
        instructionTimer = 0f;
        userConfirmed = false;

        //if (countdownCoroutine != null)
        //{
        //    StopCoroutine(countdownCoroutine);
        //}

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        Debug.Log("Started Coundown!");
        for (int i = 3; i > 0; i--)
        {
            SetInstructionsText(currentInstruction.instruction + "\n in " + i + " seconds");
            yield return new WaitForSeconds(1f);
        }

        SetInstructionsText(currentInstruction.instruction);
    }

    //private void DisplayInstruction()
    //{
    //    // Display instruction UI, handle user confirmation, and failure conditions
    //    // For example, check microphone input and set isUserConfirmed accordingly
    //    // Here's a simplified example:
    //    if (microphoneCalibration.currentLoudness> microphoneThreshold)
    //    {
    //        userConfirmed = true; // User confirmed
    //    }
    //}

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

    private IEnumerator FadeFromBlack()
    {
        while (blackImageCG.alpha > 0)
        {
            yield return new WaitForSeconds(fadeSpeed_Image);
            blackImageCG.alpha -= fadeStepStrengh;
        }
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