using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    float playerHeight = 2f;
    public float movementSpeed = 10f;
    public float movementMultiplier = 10f;
    float horizontalMovement;
    float verticalMovement;

   [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 1f;

    Vector3 moveDirection;


    [Header("Jumping")]
    public bool isGrounded;
    public float jumpForce = 15f;
    [SerializeField] private float airMultiplier = 0.2f;

    Rigidbody rb;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f);

        MyInput();
        HandleDrag();

        if(isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement;
    }

    void MovePlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration); 
        }
        else
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void HandleDrag() 
    {
        if (!isGrounded)
        {
            rb.drag = airDrag;
        }
        else
        {
            rb.drag = groundDrag;
        }
    }
}
