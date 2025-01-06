using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform orientation;
    public Transform playerBody;
    public Rigidbody rb;
    public BoolVariable isDead;

    public float rotationSpeed = 100;

    Transform player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.Find("PlayerParent").GetComponent<Transform>();
    }

    void Update()
    {
        if(isDead.value != true)
        {
            Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
            orientation.forward = viewDirection.normalized;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDirection != Vector3.zero)
            {
                playerBody.forward = Vector3.Slerp(playerBody.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        
    }
}
