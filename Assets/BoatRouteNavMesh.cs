using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BoatRouteNavMesh : MonoBehaviour
{
    // Script inspired by Code Monkey -> https://www.youtube.com/watch?v=atCOd4o7tG4

    [SerializeField] private Transform movePositionTransform;
    private NavMeshAgent BoatNavMeshAgent;

    private void Awake()
    {
        BoatNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        BoatNavMeshAgent.destination = movePositionTransform.position;
    }
}
