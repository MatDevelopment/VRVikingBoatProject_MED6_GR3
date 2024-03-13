using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Occupation
{
    Fisherman,
    Farmer,
    Mom,
    Ninth_Century_Wares_Trader,
    Livestock_Trader,
    Slave,
    Norse_Mythology_Preacher,
    Warrior,
    Weaver
    
}

public enum Talent
{
    Painting,
    Dancing,
    Fishing,
    Hunting,
    Fighting,
    Cooking,
    Weaving
}

public enum Personality
{
    Cynical,
    Social,
    Political,
    Opportunist,
    Artistic,
    Kind,
    Altruistic,
    Naive,
    Smart,
    Funny,
    Confident,
    Boastful
}

public class NpcInfo : MonoBehaviour
{
    [SerializeField] private string npcName = "";
    [SerializeField] private int npcAge;
    [SerializeField] private Occupation npcOccupation;
    [SerializeField] private Talent npcTalents;
    [SerializeField] private Personality npcPersonality;
    
    [TextArea(3,20)]
    [SerializeField] private string npcPersonalityDescription;

    public string GetPrompt()
    {
        return $"NPC Name: {npcName}\n" +
               $"NPC Age: {npcAge}\n" +
               $"NPC Occupation: {npcOccupation.ToString()}\n" +
               $"NPC Talent: {npcTalents.ToString()}\n" +
               $"NPC Personality: {npcPersonality.ToString()}\n" +
               $"NPC Personality Description: {npcPersonalityDescription}\n";

    }
}
