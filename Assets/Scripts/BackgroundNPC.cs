using GLTFast.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BackgroundNPC : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    public GameObject[] destinations;
    private int destinationID;
    Vector3 target;

    bool isInteracting = false;
    
    [SerializeField] float interactionLength = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        UpdateDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPCinteract") && !isInteracting)
        {
            animator.SetTrigger("InteractionTrigger1");
            isInteracting = true;
            // Debug.Log($"{gameObject.name} interacting: {isInteracting}");
            agent.Stop();

            StartCoroutine(StandStill(interactionLength));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, target) < 1 && isInteracting == false)
        {
            NextDestination();
            UpdateDestination();
        }
    }

    void UpdateDestination()
    {
        target = destinations[destinationID].transform.position;
        agent.SetDestination(target);
    }

    void NextDestination()
    {
        destinationID++;

        if (destinationID >= destinations.Length)
        {
            destinationID = 0;
        }
    }

    IEnumerator StandStill(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        agent.Resume();
        yield return new WaitForSeconds(3);
        isInteracting = false;
        // Debug.Log($"{gameObject.name} interacting: {isInteracting}");
    }
}
