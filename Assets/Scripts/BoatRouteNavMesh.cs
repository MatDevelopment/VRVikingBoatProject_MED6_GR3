using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BoatRouteNavMesh : MonoBehaviour
{
    private float originalSpeed;
    // Script inspired by Code Monkey -> https://www.youtube.com/watch?v=atCOd4o7tG4
    [SerializeField] private GameObject Boat;
    [SerializeField] private List<Transform> Targets = new List<Transform>();
    private NavMeshAgent BoatNavMeshAgent;

    [Header ("Debug Values")]
    [SerializeField] private int TargetIndex = 0;
    [SerializeField] private float BoatToTargetDistance;

    [Header("Points of Interests")]
    [SerializeField] private List<Transform> PointsOfInterest = new List<Transform>();
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

    [Header("Smith")]
    [SerializeField] private AudioSource smithAudio;

    [Header("Boat Builder")]
    [SerializeField] private AudioSource boatBuilderAudio;

    [Header("Fade")]
    public FadeController fadeController;
    
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
                    currentPOI = PointsOfInterest[0];
                    TargetIndex += 1;

                    // Fisherman Casting Line
                    fishingAnimator.SetTrigger("Fish");
                    fishingAudio.Play();
                }
                // Rune Stone
                else if (Targets[TargetIndex].CompareTag("RuneStone"))
                {
                    Debug.Log("Boat at Rune Stone");
                    currentPOI = PointsOfInterest[1];
                    TargetIndex += 1;

                    // Crows
                    runeAudio.Play();
                }
                // Farmstead
                else if (Targets[TargetIndex].CompareTag("Farmstead"))
                {
                    Debug.Log("Boat at Farmstead");
                    currentPOI = PointsOfInterest[2];
                    TargetIndex += 1;

                    // Sandstorm Effect
                    sandstormParticleSystem.Play();
                    sandstormAudio.Play();
                }
                // Lindholm Village
                else if (Targets[TargetIndex].CompareTag("LindholmVillage"))
                {
                    Debug.Log("Boat at Lindholm Village");
                    currentPOI = PointsOfInterest[3];
                    TargetIndex += 1;
                }
                // Lindholm Hoeje
                else if (Targets[TargetIndex].CompareTag("LindholmHoeje"))
                {
                    Debug.Log("Boat at Lindholm Hoeje");
                    currentPOI = PointsOfInterest[4];
                    TargetIndex += 1;

                    // Thorsten Burial
                    burialFire.SetActive(true);
                    burialAudio.Play();
                }
                // Market Entrance
                else if (Targets[TargetIndex].CompareTag("MarketEntrance"))
                {
                    Debug.Log("Boat at Market Entrance");
                    currentPOI = PointsOfInterest[5];
                    TargetIndex += 1;
                }
                // Blacksmith
                else if (Targets[TargetIndex].CompareTag("Smith"))
                {
                    Debug.Log("Boat at Smith");
                    currentPOI = PointsOfInterest[6];
                    TargetIndex += 1;

                    // Hammer
                    smithAudio.Play();
                }
                // Boatbuilder
                else if (Targets[TargetIndex].CompareTag("BoatBuilder"))
                {
                    Debug.Log("Boat at Boat Builder");
                    currentPOI = PointsOfInterest[7];
                    TargetIndex += 1;

                    // Woodworking
                    boatBuilderAudio.Play();
                }
                // Traders
                else if (Targets[TargetIndex].CompareTag("FabricTrader"))
                {
                    Debug.Log("Boat at Traders");
                    currentPOI = PointsOfInterest[8];
                    TargetIndex += 1;
                }
                // Eriks Place
                else if (Targets[TargetIndex].CompareTag("EriksPlace"))
                {
                    Debug.Log("Boat at Eriks");
                    currentPOI = PointsOfInterest[9];
                    TargetIndex += 1;
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
