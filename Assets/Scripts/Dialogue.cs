using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)]
    public string[] questions;

    public Sound[] answerAudio;
}
