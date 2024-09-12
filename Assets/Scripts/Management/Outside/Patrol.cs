using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] NavMeshAgent agent;
    int Destination = 0;
    private const float PatrolThreshold = 2.5f;
    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
 

    protected void Update()
    {
        HandlePatrolState();
    }


    private void HandlePatrolState()
    {
        if (agent.remainingDistance <= PatrolThreshold)
        {
            Destination = ++Destination % patrolPoints.Length;
            agent.SetDestination(patrolPoints[Destination].transform.position);
        }


    }

}
