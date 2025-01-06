using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveAI : MonoBehaviour
{
    
    public StringVariable moveState;
    public FloatVariable mobCount;
    public NavMeshAgent agent;
    public Transform transform;
    public Rigidbody rb;
    public Vector3 walkPoint;
    
    public LayerMask whatIsGround; 
    public LayerMask whatIsPlayer;

    public float sightRangeNormal;
    public float sightRangeWhenCrouching;
    public float walkPointDistance;
    
    Transform player;
    Vector3 distanceVector;
    float onMeshThreshold = 3f;
    float actualSightRange;
    float distanceToPlayer;

    string state;
    bool resting;
    bool hasWalkPoint;

    PlayerHealth playerHealth;
    BattleScript battleScript;

    public bool IsAgentOnNavMesh(GameObject agentObject)
    {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                return agentPosition.y >= hit.position.y;
            }
        }

    return false;
    }
    
    public bool isPanicking()
    {
        if (battleScript.health < battleScript.maxHealth)
        {
            return true;
        }
    return false;
    }

    private void Awake()
    {
        player = GameObject.Find("PlayerParent").transform;
        agent = GetComponent<NavMeshAgent>();
        playerHealth = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
        battleScript = GetComponent<BattleScript>();
        state = "Patrol";
        resting = false;
    }

    void Update()
    {
        if (!IsAgentOnNavMesh(agent.gameObject) && !battleScript.isDead)
        {
            DestroyNPC();
        }
        
        if (!battleScript.isDead)
        {
            if (moveState.value == "Crouching")
            {
                actualSightRange = sightRangeWhenCrouching;
            }
            else
            {    
                actualSightRange = sightRangeNormal;
            }

            distanceVector = transform.position-player.position;
            distanceToPlayer = distanceVector.magnitude;
            if (isPanicking())
            {
                if (distanceToPlayer>actualSightRange)
                {
                    if (state == "Patrol")
                    {
                        Patrol();
                    }
                    if (state == "Rest")
                    {
                        Rest();
                    }
                }
                if (distanceToPlayer<=actualSightRange)
                {
                    Flee();
                }
            }
            else
            {
                if (state == "Patrol")
                {
                    Patrol();
                }
                if (state == "Rest")
                {
                    Rest();
                }
            }
        }
    }


    private void Patrol()
    {
        if (!hasWalkPoint || (walkPoint == player.position && distanceToPlayer > sightRangeNormal)) 
        {
            SearchWalkPoint();
        }

        if (IsAgentOnNavMesh(agent.gameObject))
        {    
            agent.SetDestination(walkPoint);
        }
        Vector3 WalkPointDistance = transform.position - walkPoint;

        if (WalkPointDistance.magnitude < 1f)
        {
            hasWalkPoint = false;
            state = "Rest";
        }

    }

    void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointDistance, walkPointDistance);
        float randomZ = Random.Range(-walkPointDistance, walkPointDistance);

        walkPoint = new Vector3(transform.position.x+randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            {
            hasWalkPoint = true;
            }
    }

    private void Flee()
    {
        agent.SetDestination(player.position+(distanceVector.normalized*actualSightRange));
    }

    private void Rest()
    {
        agent.SetDestination(transform.position);
        if (!resting)
            Invoke("StatePatrol", Random.Range(3, 5));
            resting = true;
    }

    private void StatePatrol()
    {
        state = "Patrol";
        resting = false;
    }

    private void KillNPC()
    {
        battleScript.isDead = true;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        Invoke("DestroyNPC", 30);
    }

    private void DestroyNPC()
    {
        mobCount.value -= 1;
        Destroy(gameObject);
    }
}