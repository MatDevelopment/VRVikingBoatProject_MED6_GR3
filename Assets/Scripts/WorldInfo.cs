using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    [TextArea(3,20)]
    [SerializeField] private string gameStory;
    [TextArea(3,20)]
    [SerializeField] private string gameWorld;
    
    public string GetPrompt()
    {
        return $"Game Story: {gameStory}\n" +
               $"Game World: {gameWorld}\n";
    }
}
