using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeutralAI : MonoBehaviour
{

    public StringVariable moveState;
    public FloatVariable mobCount;

    public NavMeshAgent agent;
    public Transform player;
    public Transform transform;
    public Rigidbody rb;
    public Vector3 walkPoint;
    Vector3 distanceVector;
    
    public LayerMask whatIsGround; 
    public LayerMask whatIsPlayer;

    public float attackCooldown;
    public float attackRange;
    public float sightRangeNormal;
    public float sightRangeWhenCrouching;
    public float attackDamage;
    public float walkPointDistance;

    float actualSightRange;
    float distanceToPlayer;
    float onMeshThreshold = 3f;
    bool didAttack;
    bool hasWalkPoint;
    bool resting;
    string state;
    
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
    
    public bool isAgressive()
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
            if (isAgressive())
            {
                if (distanceToPlayer>actualSightRange && distanceToPlayer>=attackRange)
                {
                    Patrol();
                }
                if (distanceToPlayer<=actualSightRange && distanceToPlayer>=attackRange)
                {
                    Chase();
                }
                if (distanceToPlayer<=actualSightRange && distanceToPlayer<=attackRange)
                {
                    Attack();
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

    private void Chase()
    {
        agent.SetDestination(player.position+(distanceVector.normalized*(attackRange-1f)));
    }

    private void Attack()
    {
        agent.SetDestination(player.position+(distanceVector.normalized*(attackRange-1f)));

        if (!didAttack)
        {
            didAttack = true;
            playerHealth.ChangeHealth(-attackDamage);
            Invoke("ResetAttack", attackCooldown);
        }
    }

    private void Rest()
    {
        agent.SetDestination(transform.position);
        if (!resting)
            Invoke("StatePatrol", Random.Range(3, 5));
            resting = true;
    }

    private void ResetAttack()
    {
        didAttack = false;
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
        Invoke("DestroyNPC", 60);
    }

    private void DestroyNPC()
    {
        mobCount.value -= 1;
        Destroy(gameObject);
    }
}