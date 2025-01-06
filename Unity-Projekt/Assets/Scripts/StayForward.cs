using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayForward : MonoBehaviour
{
    Transform transform;
    Rigidbody rb;
    Vector3 velocityVector;
    
    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        velocityVector = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (velocityVector.magnitude > 0.05)
        {
            transform.forward = velocityVector.normalized;
        } 
    }
}
