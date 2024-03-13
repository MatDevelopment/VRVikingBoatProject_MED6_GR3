using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneInfo : MonoBehaviour
{
    [TextArea(3,20)]
    [SerializeField] private string SceneInfo;
    
    public string GetPrompt()
    {
        return $"Current surroundings: {SceneInfo}\n";
    }
}
