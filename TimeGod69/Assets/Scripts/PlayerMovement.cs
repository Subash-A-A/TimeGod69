
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform orientation;
    [SerializeField] private float playerHeight = 2f;
    public float movementSpeed = 7f;
    public float movementMultiplier = 10f;
    float horizontalMovement;
    float verticalMovement;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    [Header("Walking")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 7f;
    [SerializeField] float acceleration = 10f; // acceleration is used to lerp b/w walk and sprint speed.

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
    public KeyCode walkKey = KeyCode.LeftShift;
    RaycastHit slopeHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        movementSpeed = sprintSpeed;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, whatIsGround);

        MyInput();
        ControlDrag();
        ControlSpeed();

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

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
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
        if (isGrounded)
        {
            // We are resetting y velocity to zero when grounded.
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void ControlDrag()
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

    void ControlSpeed()
    {
        if (Input.GetKey(walkKey) && isGrounded)
        {
            movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            movementSpeed = Mathf.Lerp(movementSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
    }

}
