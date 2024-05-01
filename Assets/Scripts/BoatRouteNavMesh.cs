using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BoatRouteNavMesh : MonoBehaviour
{
    // Script inspired by Code Monkey -> https://www.youtube.com/watch?v=atCOd4o7tG4
    [SerializeField] private GameObject Boat;
    [SerializeField] private List<Transform> Targets = new List<Transform>();
    private NavMeshAgent BoatNavMeshAgent;

    [Header ("Debug Values")]
    [SerializeField] private int TargetIndex = 0;
    [SerializeField] private float BoatToTargetDistance;

    [SerializeField] private List<Transform> PointsOfInterest = new List<Transform>();
    public Transform currentPOI;

    public FadeController fadeController;
    
    private void Awake()
    {
        BoatNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (TargetIndex != Targets.Count)
        {
            BoatNavMeshAgent.destination = Targets[TargetIndex].position; 
            BoatToTargetDistance = Vector3.Distance(Boat.transform.position, Targets[TargetIndex].position);

            if (BoatToTargetDistance < 6 && TargetIndex < Targets.Count)
            {
                if (Targets[TargetIndex].CompareTag("Goal"))
                {
                    fadeController.FadeOut();
                    fadeController.ShowEndText();
                    Debug.Log("Goal has been reached!");
                }
                else if (Targets[TargetIndex].CompareTag("Boost"))
                {
                    BoatNavMeshAgent.speed += 1;
                    Debug.Log("Boat Speed has been boosted!");
                    TargetIndex += 1;
                }
                else if (Targets[TargetIndex].CompareTag("Slow"))
                {
                    BoatNavMeshAgent.speed -= 1;
                    Debug.Log("Boat Speed has been slowed!");
                    TargetIndex += 1;
                }
                else
                {
                    TargetIndex += 1;
                }  
            }

            if (currentPOI != PointsOfInterest[TargetIndex])
            {
                currentPOI = PointsOfInterest[TargetIndex];
            }
        }
    }
}
