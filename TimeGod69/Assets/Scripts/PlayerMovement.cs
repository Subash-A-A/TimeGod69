
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] Transform orientation;
    [SerializeField] private float playerHeight = 2f;
    public float movementSpeed = 10f;
    public float movementMultiplier = 10f;
    float horizontalMovement;
    float verticalMovement;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;


    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 1f;


    [Header("Ground Detection")]
    public bool isGrounded;
    [SerializeField] Transform groundCheck;
    private float checkRadius = 0.4f;
    [SerializeField] LayerMask whatIsGround;

    [Header("Jumping")]
    public float jumpForce = 15f;
    [SerializeField] private float airMultiplier = 0.2f;

    Rigidbody rb;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    RaycastHit slopeHit;
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                // This means we are on slope
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);

        MyInput();
        HandleDrag();

        if (isGrounded && Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * movementSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
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
