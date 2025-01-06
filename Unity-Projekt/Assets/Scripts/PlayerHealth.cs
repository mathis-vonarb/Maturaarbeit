using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public FloatVariable health;
    public FloatVariable maxHealth;
    public Slider healthBar;

    public FloatVariable stamina;
    public FloatVariable maxStamina;
    public Slider staminaBar;

    public FloatVariable hunger;
    public FloatVariable maxHunger;
    public Slider hungerBar;

    public StringVariable moveState;
    public Transform orientation;
    public GameManagerScript gameManager;
    public BoolVariable isDead;

    RaycastHit hitInfo;

    public float healthIncrement;
    public float staminaIncrement;
    public float hungerDecrement;
    public float StarveDamage;
    public float sprintCost;
    public float playerHeight;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ResetAttributes();
    }

    void Update()
    {
        if (isDead.value != true)
        {
            HealthControl();
            StaminaControl();
            HungerControl();
        }
    }

    public void ChangeHealth(float amount)
    {
        health.value += amount;
        healthBar.value = health.value;
    }

    public void ChangeStamina(float amount)
    {
        stamina.value += amount;
        staminaBar.value = stamina.value;
    }

    public void ChangeHunger(float amount)
    {
        hunger.value += amount;
        hungerBar.value = hunger.value;
    }

    void StaminaControl()
    {
        if (stamina.value > maxStamina.value)
        {
            stamina.value = maxStamina.value;
            staminaBar.value = stamina.value;
        }
        if (moveState.value == "Sprinting" && rb.velocity != new Vector3(0, rb.velocity.y, 0))
        {
            ChangeStamina(-sprintCost * Time.deltaTime);
        }
        else
        {
            ChangeStamina(staminaIncrement * Time.deltaTime);
        }
    }

    void HealthControl()
    {
        if (health.value > maxHealth.value)
        {
            health.value = maxHealth.value;
            healthBar.value = health.value;
        }

        else if (hunger.value == 0f)
        {
            ChangeHealth(-StarveDamage * Time.deltaTime);
        }
        
        else if (rb.velocity == Vector3.zero && hunger.value > 5)
        {
            ChangeHealth(healthIncrement * Time.deltaTime);
        }
        if(health.value <= 1f && (isDead.value)!=true)
        {
            isDead.value = true;
        }
    }

    void HungerControl()
    {
        if (hunger.value <= 0f)
        {
            hunger.value = 0f;
            hungerBar.value = hunger.value;
        }
        if (hunger.value > maxHunger.value)
        {
            hunger.value = maxHunger.value;
            hungerBar.value = hunger.value;
        }
        if (health.value < maxHealth.value)
        {
            ChangeHunger(-hungerDecrement * Time.deltaTime);
        }
    }

    void ResetAttributes()
    {
        health.value = maxHealth.value;
        healthBar.maxValue = maxHealth.value;
        healthBar.value = health.value;

        stamina.value = maxStamina.value;
        staminaBar.maxValue = maxStamina.value;
        staminaBar.value = stamina.value;

        hunger.value = maxHunger.value;
        hungerBar.maxValue = maxHunger.value;
        hungerBar.value = hunger.value;

        isDead.value = false;
    }
}


