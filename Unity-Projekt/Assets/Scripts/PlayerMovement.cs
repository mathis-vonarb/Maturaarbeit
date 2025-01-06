using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public Transform orientation;
    public Transform playerBody;
    public StringVariable state;
    public FloatVariable stamina;
    public BoolVariable isDead;

    public float walkingSpeed;
    public float sprintingSpeed;
    public float crouchingSpeed;
    public float playerHeight;
    public float playerCrouchHeight;
    public float playerWidth;
    public float dragOnGround;
    public float jumpForce;
    public float airMultiplier;
    public float jumpCost;
    public float jumpReserve;
    
    bool isGrounded;
    float movementSpeed = 7;
    float horizontalInput;
    float verticalInput;

    Vector3 movementDirection;
    RaycastHit hitInfo;
    Rigidbody rb;
    Transform tr;
    SphereCollider col;
    PlayerHealth healthScript;
    Vector3 velocityVector;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        healthScript = GetComponent<PlayerHealth>();
        rb.freezeRotation = true;
        state.value = "Walking";
        movementSpeed = walkingSpeed;
        col.radius = 1f;
        playerBody.localScale = new Vector3(playerBody.localScale.x, 1f, playerBody.localScale.z); 
    }

    void Update()
    {
        velocityVector = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (isDead.value != true)
        {
            CastRay();
            GetInput();
            SpeedControl();
            
            if (isGrounded)
            {
                rb.drag = dragOnGround;
            }
            else if (!isGrounded)
            {
                rb.drag = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead.value != true)
        {
            MovePlayer();
        }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKeyDown(KeyCode.C) && (state.value != "Crouching"))
        {
            state.value = "Crouching";
            movementSpeed = crouchingSpeed;
            col.radius = 0.8f;
            playerBody.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            rb.AddForce(-Vector3.up*10, ForceMode.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.C) && (state.value=="Crouching"))
        {
            state.value = "Walking";
            movementSpeed = walkingSpeed;
            col.radius = 1f;
            playerBody.localScale = new Vector3(1f, 1f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && (state.value != "Sprinting"))
        {
            state.value = "Sprinting";
            movementSpeed = sprintingSpeed;
            col.radius = 1f;
            playerBody.localScale = new Vector3(playerBody.localScale.x, 1f, playerBody.localScale.z);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && (state.value=="Sprinting"))
        {
            state.value = "Walking";
            movementSpeed = walkingSpeed;
            col.radius = 1f;
            playerBody.localScale = new Vector3(playerBody.localScale.x, 1f, playerBody.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !(stamina.value < jumpCost))
        {
            Jump();
            healthScript.ChangeStamina(-jumpCost);
        }

        if (stamina.value < 5)
        {
            state.value = "Walking";
            movementSpeed = walkingSpeed;
            col.radius = 1f;
            playerBody.localScale = new Vector3(playerBody.localScale.x, 1f, playerBody.localScale.z);
        }
    }

    private void MovePlayer()
    {
        movementDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if (isGrounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f, ForceMode.Force);
            playerBody.Rotate(transform.forward, Time.deltaTime * velocityVector.magnitude, Space.World);
        }
        else if (!isGrounded)
        {
            rb.AddForce(movementDirection.normalized * movementSpeed * 10f * airMultiplier, ForceMode.Force);
            playerBody.Rotate(transform.forward, Time.deltaTime * velocityVector.magnitude, Space.World);
        }
    }

    private void SpeedControl()
    {
        if (velocityVector.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = velocityVector.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up*jumpForce, ForceMode.Impulse);
    }


    private void CastRay()
    {
        int layerMask = 1 << 8; 
        layerMask = ~layerMask;
        Physics.Raycast(orientation.position, orientation.TransformDirection(Vector3.down), out hitInfo, playerHeight*0.5f+jumpReserve, layerMask);
        
        if (hitInfo.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
