using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindController : MonoBehaviour
{
    public float grindSpeed = 5f;
    public float grindDistance = 2f;
    public LayerMask grindMask;
    public Transform grindStartPoint;
    public Transform grindEndPoint;

    private bool isGrinding = false;
    private Vector3 grindDirection;

    private void Update()
    {
        if (isGrinding)
        {
            // Move the player along the grind rail
            transform.position += grindDirection * grindSpeed * Time.deltaTime;

            // Check if the player has reached the end of the grind rail
            float distanceToGrindEnd = Vector3.Distance(transform.position, grindEndPoint.position);
            if (distanceToGrindEnd <= 0.1f)
            {
                StopGrind();
            }
        }
        else
        {
            // Check if the player is near a grindable object and allow them to initiate a grind
            if (Physics.Raycast(grindStartPoint.position, transform.forward, out RaycastHit hit, grindDistance, grindMask))
            {
                if (hit.collider.CompareTag("Grindable"))
                {
                    StartGrind(hit);
                }
            }
        }
    }

    private void StartGrind(RaycastHit hit)
    {
        // Set up the grind direction and rotation
        isGrinding = true;
        grindDirection = Vector3.ProjectOnPlane(transform.forward, hit.normal).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(hit.normal, Vector3.up);
        transform.rotation = targetRotation;

        // Lock the player's position to the grind rail
        Vector3 playerOffset = transform.position - hit.point;
        transform.position = hit.point + playerOffset;
    }

    private void StopGrind()
    {
        // Reset the player's position and rotation
        isGrinding = false;
        transform.position += grindDirection * 0.1f;
        transform.rotation = Quaternion.LookRotation(grindDirection, Vector3.up);
    }
}
