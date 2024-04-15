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

            if (BoatToTargetDistance < 4 && TargetIndex < Targets.Count)
            {
                TargetIndex += 1;
            }
        }
    }
}
