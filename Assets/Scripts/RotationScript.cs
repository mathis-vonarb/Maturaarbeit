using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    Animator anim;
    Vector3 velocityVector;
    Rigidbody rb;
    
    public StringVariable moveState;
    
    float currentSpeed;
    float maxSpeed;
    float percentageOfMaxSpeed;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        velocityVector = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        currentSpeed = velocityVector.magnitude;        
        if (moveState.value == "Walking"){maxSpeed = 10f;}
        if (moveState.value == "Sprinting"){maxSpeed = 16f;}
        if (moveState.value == "Crouching"){maxSpeed = 8f;}
        percentageOfMaxSpeed = currentSpeed/maxSpeed;

        anim.speed = percentageOfMaxSpeed;
    }
}
