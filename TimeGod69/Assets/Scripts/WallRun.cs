using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;
    [Header("Wall Running")]
    [SerializeField] float wallDistance = 0.6f;
    [SerializeField] float minimumJumpHeight = 1.5f;

    bool wallLeft = false;
    bool wallRight = false;

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, wallDistance);
    }

    void Update()
    {
        CheckWall();
        if (CanWallRun())
        {
            if (wallLeft)
            {
                Debug.Log("Wall is on left");
            }
            else if (wallRight)
            {
                Debug.Log("Wall is on Right");
            }
        }
    }
}
