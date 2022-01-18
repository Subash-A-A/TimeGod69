using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    [Header("Grapple Settings")]
    [SerializeField] LayerMask grappleLayer;
    [SerializeField] KeyCode grappleKey = KeyCode.Q;
    [SerializeField] Transform gunTip, cam, player;
    [SerializeField] float maxDistance = 70f;
    [SerializeField] float spring = 4.5f;
    [SerializeField] float damper = 7f;
    [SerializeField] float massScale = 4.5f;
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    private SpringJoint joint;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
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
        if (Physics.Raycast(cam.position, cam.forward, out hitInfo, maxDistance))
        {
            grapplePoint = hitInfo.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distFromPoint * 0.8f;
            joint.minDistance = distFromPoint * 0.25f;

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
}
