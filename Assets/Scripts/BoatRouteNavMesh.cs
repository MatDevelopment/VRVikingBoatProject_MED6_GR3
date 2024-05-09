using OpenAI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class BoatRouteNavMesh : MonoBehaviour
{
    private float originalSpeed;
    // Script inspired by Code Monkey -> https://www.youtube.com/watch?v=atCOd4o7tG4
    [SerializeField] private GestureVersionManager gestureVersionManager;
    [SerializeField] private GameObject Boat;
    [SerializeField] private List<Transform> Targets = new List<Transform>();
    private NavMeshAgent BoatNavMeshAgent;

    [Header ("Debug Values")]
    [SerializeField] private int TargetIndex = 0;
    [SerializeField] private float BoatToTargetDistance;

    [Header("Points of Interests")]
    public List<GameObject> PointsOfInterest = new List<GameObject>();
    public Transform currentPOI;

    [Header("Fishing Hut")]
    [SerializeField] private Animator fishingAnimator;
    [SerializeField] private AudioSource fishingAudio;

    [Header("Rune Stone")]
    [SerializeField] private AudioSource runeAudio;

    [Header("Farmstead")]
    [SerializeField] private ParticleSystem sandstormParticleSystem;
    [SerializeField] private AudioSource sandstormAudio;

    [Header("Lindholm Hoeje")]
    [SerializeField] private GameObject burialFire;
    [SerializeField] private AudioSource burialAudio;
    [SerializeField] private ParticleSystem burialParticleSystem;

    [Header("Smith")]
    [SerializeField] private AudioSource smithAudio;
    [SerializeField] private Animator smithAnimator;

    [Header("Boat Builder")]
    [SerializeField] private AudioSource boatBuilderAudio;

    [Header("Fade")]
    public FadeController fadeController;
    [SerializeField] ChatTest chatTest;

    private string extraSystemInfo = "The NPC does not need to talk about the location they are passing, only talk when it fits into the current conversation topic";

    private void Awake()
    {
        BoatNavMeshAgent = GetComponent<NavMeshAgent>();
        originalSpeed = BoatNavMeshAgent.speed;
    }

    public void StopTheBoat()
    {
        BoatNavMeshAgent.speed = 0;
    }
    public IEnumerator StartTheBoat(float delay)
    {
        yield return new WaitForSeconds(delay);
        BoatNavMeshAgent.speed = originalSpeed;
    }

    private void Update()
    {
        if (TargetIndex != Targets.Count)
        {
            BoatNavMeshAgent.destination = Targets[TargetIndex].position; 
            BoatToTargetDistance = Vector3.Distance(Boat.transform.position, Targets[TargetIndex].position);

            if (BoatToTargetDistance < 6 && TargetIndex < Targets.Count)
            {
                // Generic Targets
                
                // End:
                if (Targets[TargetIndex].CompareTag("Goal"))
                {
                    fadeController.FadeOut();
                    fadeController.ShowEndText();
                    Debug.Log("Goal has been reached!");
                }
                // Speed Increase
                else if (Targets[TargetIndex].CompareTag("Boost"))
                {
                    BoatNavMeshAgent.speed += 1;
                    Debug.Log("Boat Speed has been boosted!");
                    TargetIndex += 1;
                }
                // Speed Decrease
                else if (Targets[TargetIndex].CompareTag("Slow"))
                {
                    BoatNavMeshAgent.speed -= 1;
                    Debug.Log("Boat Speed has been slowed!");
                    TargetIndex += 1;
                }
                
                // Points of Interest

                // Fishing Hut
                else if (Targets[TargetIndex].CompareTag("FishingHut"))
                {
                    Debug.Log("Boat at Fishing Hut");
                    currentPOI = PointsOfInterest[0].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string fishingDescription = "You are now passing a fishing hut on the right. ";
                        // "Beside the fishing hut there is a small pier where Erik's good friend Ole is currently fishing.";
                    InterestPointDescription interestPointDescription = PointsOfInterest[0].GetComponent<InterestPointDescription>();
                    fishingDescription = fishingDescription + interestPointDescription.description + " " + extraSystemInfo;
                    Debug.Log("Fishing Description: " + fishingDescription);
                    chatTest.AddSystemInstructionToChatLog(fishingDescription);

                    // Fisherman Casting Line
                    fishingAnimator.SetTrigger("Fish");
                    fishingAudio.Play();
                }
                // Rune Stone
                else if (Targets[TargetIndex].CompareTag("RuneStone"))
                {
                    Debug.Log("Boat at Rune Stone");
                    currentPOI = PointsOfInterest[1].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string runeDescription = "You are now passing a runestone on the left. ";
                    InterestPointDescription interestPointDescription = PointsOfInterest[1].GetComponent<InterestPointDescription>();
                    runeDescription = runeDescription + interestPointDescription.description + " " + extraSystemInfo;
                    
                    if (gestureVersionManager.GestureVersion)
                    {
                        runeDescription = runeDescription + " The NPC now has permission to be POINTING at RUNESTONE";
                    }

                    Debug.Log("Rune Description: " + runeDescription);
                    chatTest.AddSystemInstructionToChatLog(runeDescription);

                    // Crows
                    runeAudio.Play();
                }
                // Farmstead
                else if (Targets[TargetIndex].CompareTag("Farmstead"))
                {
                    Debug.Log("Boat at Farmstead");
                    currentPOI = PointsOfInterest[2].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string farmDescription = "You are now passing a farmstead on the left. ";
                    // "The farm's crops are being covered in sand by a sandstorm which could kill some of the yield.";
                    InterestPointDescription interestPointDescription = PointsOfInterest[2].GetComponent<InterestPointDescription>();
                    farmDescription = farmDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        farmDescription = farmDescription + " The NPC now has permission to be POINTING at FARMSTEAD";
                    }

                    Debug.Log("Farm Description: " + farmDescription);
                    chatTest.AddSystemInstructionToChatLog(farmDescription);

                    // Sandstorm Effect
                    sandstormParticleSystem.Play();
                    sandstormAudio.Play();
                }
                // Lindholm Village
                else if (Targets[TargetIndex].CompareTag("LindholmVillage"))
                {
                    Debug.Log("Boat at Lindholm Village");
                    currentPOI = PointsOfInterest[3].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string villageDescription = "You are now passing Lindholm village on the right. ";
                        // "Lindholm is a small village consisting of a few farms just beside a sacred burial mound";
                    InterestPointDescription interestPointDescription = PointsOfInterest[3].GetComponent<InterestPointDescription>();
                    villageDescription = villageDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        villageDescription = villageDescription + " The NPC now has permission to be POINTING at VILLAGE";
                    }

                    Debug.Log("Village Description: " + villageDescription);
                    chatTest.AddSystemInstructionToChatLog(villageDescription);
                }
                // Lindholm Hoeje
                else if (Targets[TargetIndex].CompareTag("LindholmHoeje"))
                {
                    Debug.Log("Boat at Lindholm Hoeje");
                    currentPOI = PointsOfInterest[4].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string burialDescription = "You are now passing Lindholm Hoeje on the right. ";
                        // "Lindholm Hoeje is a burial mound where the dead are burned in stoneformations immitating a ship's hull symbolising their journey to the afterlife." +
                        // "A burial has just started as you pass";
                    InterestPointDescription interestPointDescription = PointsOfInterest[4].GetComponent<InterestPointDescription>();
                    burialDescription = burialDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        burialDescription = burialDescription + " The NPC now has permission to be POINTING at BURIALMOUND";
                    }

                    Debug.Log("Burial Description: " + burialDescription);
                    chatTest.AddSystemInstructionToChatLog(burialDescription);

                    // Thorsten Burial
                    burialFire.SetActive(true);
                    burialAudio.Play();
                    burialParticleSystem.Play();
                }
                // Market Entrance
                else if (Targets[TargetIndex].CompareTag("MarketEntrance"))
                {
                    Debug.Log("Boat at Market Entrance");
                    currentPOI = PointsOfInterest[5].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string marketDescription = "You are now approaching the entrance to the market. ";
                        // "The market is a large hub for commerce filled with people especially sailors who pass by northern Jutland";
                    InterestPointDescription interestPointDescription = PointsOfInterest[5].GetComponent<InterestPointDescription>();
                    marketDescription = marketDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        marketDescription = marketDescription + " The NPC now has permission to be POINTING at MARKETENTRANCE";
                    }

                    Debug.Log("Market Description: " + marketDescription);
                    chatTest.AddSystemInstructionToChatLog(marketDescription);
                }
                // Blacksmith
                else if (Targets[TargetIndex].CompareTag("Smith"))
                {
                    Debug.Log("Boat at Smith");
                    currentPOI = PointsOfInterest[6].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string smithDescription = "You are now passing a blacksmith on the left. ";
                        // "He is hammering away on the anvil creating armour for people who will be going on a viking journey soon.";
                    InterestPointDescription interestPointDescription = PointsOfInterest[6].GetComponent<InterestPointDescription>();
                    smithDescription = smithDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        smithDescription = smithDescription + " The NPC now has permission to be POINTING at BLACKSMITH";
                    }

                    Debug.Log("Blacksmith Description: " + smithDescription);
                    chatTest.AddSystemInstructionToChatLog(smithDescription);

                    // Hammer
                    smithAudio.Play();
                    smithAnimator.SetTrigger("Hammering");
                }
                // Boatbuilder
                else if (Targets[TargetIndex].CompareTag("BoatBuilder"))
                {
                    Debug.Log("Boat at Boat Builder");
                    currentPOI = PointsOfInterest[7].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string boatDescription = "You are now passing a shipyard on the right. ";
                        // "Half finished ships are being worked on.";
                    InterestPointDescription interestPointDescription = PointsOfInterest[7].GetComponent<InterestPointDescription>();
                    boatDescription = boatDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        boatDescription = boatDescription + " The NPC now has permission to be POINTING at BOATBUILDER";
                    }

                    Debug.Log("Boat Description: " + boatDescription);
                    chatTest.AddSystemInstructionToChatLog(boatDescription);

                    // Woodworking
                    boatBuilderAudio.Play();
                }
                // Traders
                else if (Targets[TargetIndex].CompareTag("FabricTrader"))
                {
                    Debug.Log("Boat at Traders");
                    currentPOI = PointsOfInterest[8].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string traderDescription = "You are now passing a series of traders on the right. ";
                        // "The first trader sells fresh fish, the second sells crops and the last sells woven fabrics like clothing and sails.";
                    InterestPointDescription interestPointDescription = PointsOfInterest[8].GetComponent<InterestPointDescription>();
                    traderDescription = traderDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        traderDescription = traderDescription + " The NPC now has permission to be POINTING at TRADERS";
                    }

                    Debug.Log("Trader Description: " + traderDescription);
                    chatTest.AddSystemInstructionToChatLog(traderDescription);
                }
                // Eriks Place
                else if (Targets[TargetIndex].CompareTag("EriksPlace"))
                {
                    Debug.Log("Boat at Eriks");
                    currentPOI = PointsOfInterest[9].transform;
                    TargetIndex += 1;

                    // System Instruction
                    string eriksDescription = "You are now approaching Eriks own home on the left. ";
                        // "The home is a small fishing hut with a pier and is the place the trip will end.";
                    InterestPointDescription interestPointDescription = PointsOfInterest[9].GetComponent<InterestPointDescription>();
                    eriksDescription = eriksDescription + interestPointDescription.description + " " + extraSystemInfo;

                    if (gestureVersionManager.GestureVersion)
                    {
                        eriksDescription = eriksDescription + " The NPC now has permission to be POINTING at ERIKSHUT";
                    }

                    Debug.Log("Eriks House Description: " + eriksDescription);
                    chatTest.AddSystemInstructionToChatLog(eriksDescription);
                }

                // Default
                else
                {
                    TargetIndex += 1;
                }  
            }

            //if (currentPOI != PointsOfInterest[TargetIndex])
            //{
            //    currentPOI = PointsOfInterest[TargetIndex];
            //}
        }
    }
}
