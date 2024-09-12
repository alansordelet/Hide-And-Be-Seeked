using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PriestMovement : MonoBehaviour
{
    [SerializeField] private Transform playerPos;
    private NavMeshAgent agent;
    public float coneAngle = 10f;
    public float coneLength = 30f;
    public Animator priestAnimator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       // if (priestAnimator.GetBool("isSprinting") == true)
        //agent.SetDestination(new Vector3(playerPos.position.x-1, playerPos.position.y, playerPos.position.z -1));
    }
}
