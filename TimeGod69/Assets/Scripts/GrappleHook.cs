using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Grapple Settings")]
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] KeyCode grappleKey = KeyCode.Q;
    [SerializeField] Transform gunTip, cam, player;
    [SerializeField] float range = 70f;
    [SerializeField] float spring = 3f;
    [SerializeField] float minDistanceMultiplier = 0.2f;
    [SerializeField] float maxDistanceMultiplier = 0.3f;
    private float sconst;
    [SerializeField] float damper = 7f;
    [SerializeField] float massScale = 4.5f;

    [Header("Aim Assist")]
    [SerializeField] float aimAssistRadius = 1f;
    [SerializeField] GameObject debugAssist;
    [SerializeField] bool drawAssist = true;
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    private SpringJoint joint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        debugAssist.SetActive(false);
    }

    private void Update()
    {
        DrawAssistPoint();

        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }
        else if (Input.GetKeyUp(grappleKey))
        {
            StopGrapple();
        }
    }
    private void LateUpdate()
    {
        DrawGrapple();
    }
    void StartGrapple()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(cam.position, aimAssistRadius, cam.forward, out hitInfo, range, grappleLayer))
        {
            grapplePoint = hitInfo.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.minDistance = distFromPoint * minDistanceMultiplier;
            joint.maxDistance = distFromPoint * maxDistanceMultiplier;

            joint.spring = spring;
            joint.damper = damper;
            joint.massScale = massScale;

            lineRenderer.positionCount = 2;

        }
    }
    void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    void DrawGrapple()
    {
        if (!joint) return;
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    void DrawAssistPoint()
    {
        RaycastHit hit;
        if (Physics.SphereCast(cam.position, aimAssistRadius, cam.forward, out hit, range, grappleLayer) && drawAssist)
        {
            debugAssist.SetActive(true);
            debugAssist.transform.forward = cam.transform.forward;
            debugAssist.transform.position = hit.point;
        }
        else
        {
            debugAssist.SetActive(false);
        }
    }
}
