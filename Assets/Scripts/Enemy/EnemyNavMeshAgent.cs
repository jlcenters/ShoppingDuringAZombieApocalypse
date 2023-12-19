using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
 * 
 * 
    ENEMY NAVMESH AGENT FUNCTIONS:
        patrols set waypoints
        if player is in view, agent will chase
        if in attacking distance, agent will be able to attack
        if player leaves attacking distance, agent will chase player
        if player escapes chase sequence, agent will return to its patrol
 *
 *
 */
public class EnemyNavMeshAgent : MonoBehaviour
{
    [Header("Agent Info")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float waitTime = 4f;
    [SerializeField] private float rotateTime = 2f;
    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float runningSpeed = 9f;

    [Header("Targeting Info")]
    [SerializeField] private float viewRadius = 15f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private Transform[] waypoints;
    private int currentWaypoint = 0;

    private Vector3 lastPlayerPosition = Vector3.zero;
    private Vector3 playerCurrentPosition = Vector3.zero;

    //manipulatable stats
    private float timeToWait;
    private float timeToRotate;
    private bool playerInRange = false;
    private bool playerNear = false;
    private bool isPatroling = true;
    private bool caughtPlayer = false;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        agent.isStopped = false;
        agent.speed = walkingSpeed;
        SetDestinationFromList();

        timeToWait = waitTime;
        timeToRotate = rotateTime;
    }
    private void Update()
    {
        Searching();

        if(!isPatroling)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }



    private void Patrol()
    {
        //if target near, walk towards it; else, go on to next waypoint
        if (playerNear)
        {
            Debug.Log("player near");
            //if rotation completed, walk towards target; else, continue rotation
            if (timeToRotate <= 0)
            {
                ToggleMovement(true, walkingSpeed);
                LookingForPlayer(lastPlayerPosition);
            }
            else
            {
                ToggleMovement(false);
                timeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            Debug.Log("player not near");

            lastPlayerPosition = Vector3.zero;
            playerNear = false;
            SetDestinationFromList();

            //if stopping distance met, check wait time
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                //if wait time reached, begin walking to next patrol point; else, continue waiting
                if (timeToWait <= 0)
                {
                    NextWaypoint();
                    ToggleMovement(true, walkingSpeed);
                    timeToWait = waitTime;
                }
                else
                {
                    ToggleMovement(false);
                    timeToWait -= Time.deltaTime;
                }
            }
        }
    }
    private void Chase()
    {
        //reset
        playerNear = false;
        lastPlayerPosition = Vector3.zero;

        //if player was not caught, run and set destination to where the player is
        if (!caughtPlayer)
        {
            ToggleMovement(true, runningSpeed);
            agent.SetDestination(playerCurrentPosition);
        }

        //if agent got to stopping distance, check if player will stop chasing
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //if player is out of view and the wait time has been met, return to patroling;
            //else if player is 2.5 units from the enemy, stop chasing and begin wait counter
            if (timeToWait <= 0 && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag(targetTag).transform.position) >= 6f && !caughtPlayer)
            {
                isPatroling = true;
                playerNear = false;
                ToggleMovement(true, walkingSpeed);
                timeToRotate = rotateTime;
                timeToWait = waitTime;
                SetDestinationFromList();
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag(targetTag).transform.position) >= 2.5f)
                {
                    ToggleMovement(false);
                    timeToWait -= Time.deltaTime;
                }
            }
        }
    }
    //is isMoving is true, agent is not stopped
    //default moveSpeed is 0f, but can specify if agent will be moving
    private void ToggleMovement(bool isMoving, float moveSpeed=0f)
    {
        agent.isStopped = !isMoving;
        agent.speed = moveSpeed;
    }
    public void NextWaypoint()
    {
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        SetDestinationFromList();
    }
    private void CaughtPlayer(bool isCaught)
    {
        caughtPlayer = isCaught;
    }
    private void LookingForPlayer(Vector3 lastPlayerPosition)
    {
        agent.SetDestination(lastPlayerPosition);

        //when destination is reached, begin timer
        if(Vector3.Distance(transform.position, lastPlayerPosition) <= 0.3f)
        {
            //if timer runs out, return to patrol; else, wait longer
            if(timeToWait <= 0)
            {
                playerNear = false;
                ToggleMovement(true, walkingSpeed);
                SetDestinationFromList();
                timeToWait = waitTime;
                timeToRotate = rotateTime;
            }
            else
            {
                ToggleMovement(false);
                timeToWait -= Time.deltaTime;
            }
        }
    }
    private void Searching()
    {
        Collider[] playerCollider = Physics.OverlapSphere(transform.position, viewRadius, targetLayer);

        for (int i = 0; i < playerCollider.Length; i++)
        {
            Debug.Log("looping colliders");

            //set transform, direction, and distance from to current
            Transform playerTransform = playerCollider[i].transform;
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            float playerDistance = Vector3.Distance(transform.position, playerTransform.position);

            //if the player is within viewpoint, check distance
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                //if there is an obstacle in the way, agent cannot see target but knows target is in range; else, target not in range
                if (!Physics.Raycast(transform.position, directionToPlayer, playerDistance, obstacleLayer))
                {
                    Debug.Log("player in range");

                    playerInRange = true;
                    isPatroling = false;
                }
                else
                {
                    Debug.Log("player in range");

                    playerInRange = false;
                }
            }
            Debug.Log("was not in view of player");

            //if target is farther than view radius, out of range
            if (playerDistance > viewRadius)
            {
                playerInRange = false;
            }
            //if target in range, set position variable to current
            if (playerInRange)
            {
                Debug.Log("player in range");
                playerCurrentPosition = playerTransform.position;
            }
        }
    }
    private bool SetDestinationFromList()
    {
        return agent.SetDestination(waypoints[currentWaypoint].position);
    }
}
