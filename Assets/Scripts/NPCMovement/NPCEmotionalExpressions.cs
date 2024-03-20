using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class NPCEmotionalExpressions : MonoBehaviour
{
    int blendShapeCount;
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    public bool DebugEnabled = true;
    
    // Intensity of Emotional States
    public float blendValue = 0.5f;
    float expressionSpeed = 0.01f;

    // Based on Ekman's 7 Emotional States
    public string currentMood = "isStatic";

    public bool isHappy = false;
    public bool isSad = false;
    public bool isAngry = false;
    public bool isSurprised = false;
    public bool isScared = false;
    public bool isDisgusted = false;
    public bool isContempted = false;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    // Start is called before the first frame update
    void Start()
    {
        blendShapeCount = skinnedMesh.blendShapeCount;
    }

    // Update is called once per frame
    void Update()
    {
        // Debugging Keys
        if (DebugEnabled) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentMood = "isDefault";
            }

            // Happy = H
            if (Input.GetKeyDown(KeyCode.H))
            {
                isHappy = true;
            }

            // Sad = Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isSad = true;
            }

            // Angry = A
            if (Input.GetKeyDown(KeyCode.A))
            {
                isAngry = true;
            }

            // Surprised = S
            if (Input.GetKeyDown(KeyCode.S))
            {
                isSurprised = true;
            }

            // Scared = F
            if (Input.GetKeyDown(KeyCode.F))
            {
                isScared = true;
            }

            // Disgust = D
            if (Input.GetKeyDown(KeyCode.D))
            {
                isDisgusted = true;
            }

            // Contempt = C
            if (Input.GetKeyDown(KeyCode.C))
            {
                isContempted = true;
            }
        }

        // Emotional State Logic
        if (isHappy == true)
        {
            currentMood = "isHappy";
            isHappy = false;
            // Happy(blendValue);
        }

        if (isSad == true)
        {
            currentMood = "isSad";
            isSad = false;
            // Sad(blendValue);
        }

        if (isAngry == true)
        {
            currentMood = "isAngry";
            isAngry = false;
            // Angry(blendValue);
        }

        if (isSurprised == true)
        {
            currentMood = "isSurprised";
            isSurprised = false;
            // Surprised(blendValue);
        }

        if (isScared == true)
        {
            currentMood = "isScared";
            isScared = false;
            // Scared(blendValue);
        }

        if (isDisgusted == true)
        {
            currentMood = "isDisgusted";
            isDisgusted = false;
            // Disgust(blendValue);
        }

        if (isContempted == true)
        {
            currentMood = "isContempted";
            isContempted = false;
            // Contempt(blendValue);
        }

        // Switch case which reads the current mood and runs the appropriate function
        switch (currentMood)
        {
            case "isStatic":
                break;
            case "isDefault":
                DefaultExpression();
                break;
            case "isHappy":
                Happy(blendValue);
                break;
            case "isSad":
                Sad(blendValue);
                break;
            case "isAngry":
                Angry(blendValue);
                break;
            case "isSurprised":
                Surprised(blendValue);
                break;
            case "isScared":
                Scared(blendValue);
                break;
            case "isDisgusted":
                Disgust(blendValue);
                break;
            case "isContempted":
                Contempt(blendValue);
                break;
            default:
                break;
        }
    }

    // Function for resetting all emotion blendshapes to 0 except the current emotion
    void ResetEmotionalState(int[] notEffectedShapes)
    {
        bool isEqual = false;

        for (int i = 15; i < blendShapeCount; i++)
        {
            foreach (int j in notEffectedShapes)
            {
                if (j == i)
                {
                    isEqual = true;
                }
            }
            if (!isEqual)
            {
                float currentBlendValue = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(i), 0, expressionSpeed);
                skinnedMeshRenderer.SetBlendShapeWeight(i, currentBlendValue);
            }

            isEqual = false;
        }
    }

    void DefaultExpression()
    {
        float CombinedValue = 0;

        for (int i = 15; i < blendShapeCount; i++)
        {
            float currentBlendValue = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(i), 0, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(i, currentBlendValue);
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(i);
        }

        float AverageValue = CombinedValue / blendShapeCount - 15;

        if (AverageValue <= 0)
        {
            currentMood = "isStatic";
            Debug.Log("Default");
        }
    }

    void Happy(float blendValue)
    {
        // FACS: 6 + 12
        int LeftLipRaiser = 63;
        int RightLipRaiser = 64;
        int LeftCheekRaiser = 48;
        int RightCheekRaiser = 49;

        // Resetting previous emotion
        int[] shapes = { LeftLipRaiser, RightLipRaiser, LeftCheekRaiser, RightCheekRaiser };
        ResetEmotionalState(shapes);

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]);
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Happy");
        }
    }
    void Sad(float blendValue)
    {
        // FACS: 1 + 4 + 15
        int innerBrowRaiser = 18;
        int LeftBrowLowerer = 16;
        int RightBrowLowerer = 17;
        int LeftLipLowerer = 28;
        int RightLipLowerer = 29;

        int[] shapes = { innerBrowRaiser, LeftBrowLowerer, RightBrowLowerer, LeftLipLowerer, RightLipLowerer };
        ResetEmotionalState(shapes);

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]); ;
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Sad");
        }
    }

    void Angry(float blendValue)
    {
        // FACS: 4 + 5 + 7 + 23
        int LeftBrowLowerer = 16;
        int RightBrowLowerer = 17;
        int LeftLidRaiser = 23;
        int RightLidRaiser = 24;
        int LeftLidTightener = 21;
        int RightLidTightener = 22;
        int LeftMouthStretch = 55;
        int RightMouthStretch = 56;

        int[] shapes = { LeftBrowLowerer, RightBrowLowerer, LeftLidRaiser, RightLidRaiser, LeftLidTightener, RightLidTightener, LeftMouthStretch, RightMouthStretch };

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]); ;
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Angry");
        }
    }

    void Surprised(float blendValue)
    {
        // FACS: 1 + 2 + 5B + 26
        int InnerBrowRaiser = 18;
        int LeftOuterBrowRaiser = 19;
        int RightOuterBrowRaiser = 20;
        int LeftUpperLidRaiser = 23;
        int RightUpperLidRaiser = 24;
        int JawDrop = 50;

        int[] shapes = { InnerBrowRaiser, LeftOuterBrowRaiser, RightOuterBrowRaiser, LeftUpperLidRaiser, RightUpperLidRaiser };
        ResetEmotionalState(shapes);

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]); ;
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Surprised");
        }
    }

    void Scared(float blendValue)
    {
        // FACS: 1 + 2 + 4 + 5 + 7 + 20 + 26
        int InnerBrowRaiser = 18;
        int LeftOuterBrowRaiser = 19;
        int RightOuterBrowRaiser = 20;
        int LeftBrowLowerer = 16;
        int RightBrowLowerer = 17;
        int LeftUpperLidRaiser = 23;
        int RightUpperLidRaiser = 24;
        int LeftLidTightener = 21;
        int RightLidTightener = 22;
        int LeftMouthStretch = 55;
        int RightMouthStretch = 56;
        int JawDrop = 50;

        int[] shapes = { InnerBrowRaiser, LeftOuterBrowRaiser, RightOuterBrowRaiser, LeftBrowLowerer, RightBrowLowerer, LeftUpperLidRaiser, RightUpperLidRaiser, 
                        LeftLidTightener, RightLidTightener, LeftMouthStretch, RightMouthStretch };
        
        ResetEmotionalState(shapes);

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]); ;
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Scared");
        }
    }

    void Disgust(float blendValue)
    {
        // FACS: 9 + 15 + 17
        int LeftNoseWrinkler = 33;
        int RightNoseWrinkler = 34;
        int LeftLipLowerer = 28;
        int RightLipLowerer = 29;
        int MouthShrugLower = 31;
        int MouthShrugUpper = 32;

        int[] shapes = { LeftNoseWrinkler, RightNoseWrinkler, LeftLipLowerer, RightLipLowerer, MouthShrugLower, MouthShrugUpper };
        ResetEmotionalState(shapes);

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]); ;
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Disgusted");
        }
    }

    void Contempt(float blendValue)
    {
        // FACS: R12A + R14A
        int RightLipCornerPuller = 38;
        int RightDimpler = 54;

        int[] shapes = { RightLipCornerPuller, RightDimpler };
        ResetEmotionalState(shapes);

        // Smoothing and changing each blendshape
        foreach (int shape in shapes)
        {
            float currentBlend = Mathf.Lerp(skinnedMeshRenderer.GetBlendShapeWeight(shape), blendValue, expressionSpeed);
            skinnedMeshRenderer.SetBlendShapeWeight(shape, currentBlend);
        }

        float CombinedValue = 0;

        // Calculating a average value for the blendshapes
        for (int i = 0; i < shapes.Length; i++)
        {
            CombinedValue += skinnedMeshRenderer.GetBlendShapeWeight(shapes[i]); ;
        }

        float AverageValue = CombinedValue / shapes.Length;

        // Stopping the transition if close to chosen value
        if (AverageValue >= 0.95f * blendValue)
        {
            currentMood = "isStatic";
            Debug.Log("Contempted");
        }
    }
}
