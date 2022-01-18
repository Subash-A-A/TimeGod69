using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wall Detection")]
    [SerializeField] Transform orientation;
    [SerializeField] float wallDistance = 0.6f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity;
    [SerializeField] float wallJumpForce;

    [Header("Camera Effects")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;
    [SerializeField] private float cameraTilt;
    [SerializeField] private float cameraTiltTime;

    public float tilt { get; private set; }
    Rigidbody rb;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    Vector3 wallRunJumpDirection;

    bool wallLeft = false;
    bool wallRight = false;
    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        CheckWall();
        if (CanWallRun())
        {
            if (wallLeft)
            {
                StartWallRun();
            }
            else if (wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    void StartWallRun()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                wallRunJumpDirection = transform.up + leftWallHit.normal;
            }
            if (wallRight)
            {
                wallRunJumpDirection = transform.up + rightWallHit.normal;
            }
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * wallJumpForce * 100, ForceMode.Force);
        }
        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -cameraTilt, cameraTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, cameraTilt, cameraTiltTime * Time.deltaTime);
        }
    }

    public void StopWallRun()
    {
        rb.useGravity = true;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, cameraTiltTime * Time.deltaTime);
    }
}
