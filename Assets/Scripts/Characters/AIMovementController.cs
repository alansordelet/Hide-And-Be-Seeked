using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovementController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get the normalized desired velocity
        Vector3 normalizedMovement = navMeshAgent.desiredVelocity.normalized;

        // Project the movement direction onto the AI's right vector
        Vector3 rightVector = Vector3.Project(normalizedMovement, transform.right);

        // Calculate right velocity
        float rightVelocity = rightVector.magnitude * Vector3.Dot(rightVector, transform.right);

        // Convert velocity to [0, 1] range and assign to animator parameter
        animator.SetFloat("Turn", Mathf.InverseLerp(-1f, 1f, rightVelocity));

       // Debug.Log(animator.GetFloat("Turn"));
    }
}
