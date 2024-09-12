using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAndFollow : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Transform playerPos;
    [SerializeField] NavMeshAgent agent;
    int Destination = 0;
    private float timeToResetState = 10f;
    private float timeOutOfVision = 0f;
    private float maxDistance = 10f;
    private AIstates aiType;
    [SerializeField] PriestVisibilityCheck priestVision;
    [SerializeField] PlayerBehaviour player;
    [SerializeField] AudioSource jumpscareViolin;
    [SerializeField] AudioSource chaseViolin;
    [SerializeField] AudioSource run;
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource deathHit;
    [SerializeField] Transform grabPos;

    public Animator priestAnimator;

    private bool playerGrabbed;
    // Start is called before the first frame update

    private enum AIstates
    {
        PATROL,
        FOLLOW
    }
    protected void Start()
    {
        //sqrMaxDistance = maxDistance * maxDistance;
        aiType = AIstates.PATROL;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private const float PatrolThreshold = 2.5f;
    private const float RotationSmoothFactor = 0.5f;

    protected void Update()
    {
        if (GameManager.instance.isTeleporting)
        {
            chaseViolin.Stop();
            chaseViolin.volume = 0;
            Debug.Log("Going Back To Patrol");
            agent.speed = 3.5f;
            priestAnimator.SetBool("isSprinting", false);
            walk.gameObject.SetActive(true);
            run.gameObject.SetActive(false);
            SetBehaviour(AIstates.PATROL);
        }

        if (!agent.isActiveAndEnabled) return;

        switch (aiType)
        {
            case AIstates.PATROL:
                HandlePatrolState();
                break;
            case AIstates.FOLLOW:
                HandleFollowState();
                break;
        }

    }

    private void HandlePatrolState()
    {
        if (agent.remainingDistance <= PatrolThreshold)
        {
            Destination = ++Destination % patrolPoints.Length;
            agent.SetDestination(patrolPoints[Destination].transform.position);
        }

        if (priestVision.playerInView && GetBehaviour() == AIstates.PATROL)
        {
            if (!jumpscareViolin.isPlaying)
                jumpscareViolin.Play();

            chaseViolin.volume = 1.0f;

            if (!chaseViolin.isPlaying)
                chaseViolin.Play();

            agent.speed = 4.5f;

            priestAnimator.SetBool("isSprinting", true);
            walk.gameObject.SetActive(false);
            run.gameObject.SetActive(true);
            SetBehaviour(AIstates.FOLLOW);
            timeOutOfVision = 0f;
        }
    }

    private void HandleFollowState()
    {
        if (!priestVision.playerInView)
        {
            chaseViolin.volume = Mathf.Max(chaseViolin.volume - 0.25f * Time.deltaTime, 0.3f);
            timeOutOfVision += Time.deltaTime;
            if (timeOutOfVision >= timeToResetState && GetBehaviour() == AIstates.FOLLOW)
            {
                chaseViolin.Stop();
                chaseViolin.volume = 0;
                Debug.Log("Going Back To Patrol");
                agent.speed = 3.5f;
                priestAnimator.SetBool("isSprinting", false);
                walk.gameObject.SetActive(true);
                run.gameObject.SetActive(false);
                SetBehaviour(AIstates.PATROL);
            }
        }
        else
        {
            chaseViolin.volume = Mathf.Max(chaseViolin.volume + 0.75f * Time.deltaTime, 1);
        }

        HandleAgentMovementAndRotation();
    }

    private void HandleAgentMovementAndRotation()
    {
        Vector3 AgentToPlayer = playerPos.transform.position - agent.transform.position;
        float sqrDistance = AgentToPlayer.sqrMagnitude;

        if (sqrDistance >= maxDistance)
        {
            priestAnimator.SetBool("isHitting", false);
            agent.SetDestination(playerPos.transform.position);
        }
        else
        {
            priestAnimator.SetBool("isHitting", true);
            agent.ResetPath();
            agent.speed = 0;
            playerPos.GetComponent<PlayerBehaviour>().enabled = false;
            playerPos.GetComponent<Collider>().enabled = false;
            playerPos.position = grabPos.position;
            playerPos.rotation = grabPos.rotation;

            foreach (Transform child in playerPos)
            {
                if (child.name == "PlayerCam A")
                {
                    child.rotation = grabPos.rotation;
                    break;
                }
            }
            StartCoroutine(WaitForAnimationEnd());
        }
    }
    private bool soundPlayed = false;
    IEnumerator WaitForAnimationEnd()
    {

        float animationLength = priestAnimator.GetCurrentAnimatorStateInfo(0).length;
        chaseViolin.Stop();
        walk.Stop();
        run.Stop();
        yield return new WaitForSeconds(animationLength);
       
        if (!soundPlayed)
        {
            deathHit.Play();
            soundPlayed = true;
        }

        yield return new WaitForSeconds(0.5f);

        GameManager.instance.Death = true;
        Debug.Log("Animation ended.");

    }
    private void SetBehaviour(AIstates state)
    {
        aiType = state;
    }

    private AIstates GetBehaviour()
    {
        return aiType;
    }
}
