using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScript : MonoBehaviour
{
    public FloatVariable playerAttackRange;
    public BoolVariable playerCanAttack;
    public FloatVariable mobCount;
    public FloatVariable score;
    
    public float maxHealth;
    public float health;
    public bool isDead;
    public float spawnDistance;

    Transform player;
    Rigidbody rb;
    Vector3 distanceVector;
    PlayerHealth playerHealthScript;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        health = maxHealth;
        isDead = false;
        playerHealthScript = GameObject.Find("PlayerParent").GetComponent<PlayerHealth>();
    }

    void Update()
    {
        distanceVector = transform.position-player.position;
        PlayerAttack();
        VoidCheck();
    }


    private void PlayerAttack()
    {
        if((Input.GetKey(KeyCode.Mouse0)) && playerCanAttack.value==true && distanceVector.magnitude <= playerAttackRange.value)
        {
            playerCanAttack.value = false;
            TakeDamage(1f);
            if (isDead)
                TakeKnockback(2f);
            else
                TakeKnockback(7f);
        }
        if((Input.GetKey(KeyCode.Mouse1)) && playerCanAttack.value==true && distanceVector.magnitude <= playerAttackRange.value && isDead)
        {
            playerHealthScript.ChangeHunger(15);
            DestroyNPC();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f && !isDead)
        {
            KillNPC();
        }
    }

    public void TakeKnockback(float amount)
    {
        rb.AddForce(distanceVector*amount, ForceMode.Impulse);
    }

    private void KillNPC()
    {
        score.value += 1;
        isDead = true;
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        Invoke("DestroyNPC", 30);
    }

    private void DestroyNPC()
    {
        mobCount.value -= 1;
        Destroy(gameObject);
    }

    private void VoidCheck()
    {
        if (transform.position.y <= 0)
        {
            DestroyNPC();
        }
        if (distanceVector.magnitude > spawnDistance)
        {
            DestroyNPC();
        }
    }
}
