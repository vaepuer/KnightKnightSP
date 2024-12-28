using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public UnitController unitController;  // Reference to the UnitController script
    public float stopThreshold = 0.1f; // Threshold to consider the unit stopped
    public float smoothTime = 0.3f;  // Smoothing time for the camera movement

    private Vector3 offset;   // Offset distance between the unit and camera
    private bool isFollowing = false;
    private Vector3 lastUnitPosition;
    private Transform selectedUnit;
    private Vector3 velocity = Vector3.zero; // Velocity for the SmoothDamp function

    void Start()
    {
        // Initialize variables
        selectedUnit = null;
        offset = transform.position; // Initial camera position
    }

    void LateUpdate()
    {
        // Check if a unit is selected
        if (unitController.unitSelected)
        {
            if (selectedUnit != unitController.selectedUnit)
            {
                selectedUnit = unitController.selectedUnit;
                offset = transform.position - selectedUnit.position;
                lastUnitPosition = selectedUnit.position;
                isFollowing = false;
            }

            // Check if the selected unit has stopped moving
            if ((selectedUnit.position - lastUnitPosition).magnitude <= stopThreshold)
            {
                if (!isFollowing)
                {
                    isFollowing = true;
                }
            }
            else
            {
                isFollowing = false;
            }

            lastUnitPosition = selectedUnit.position;

            // Follow the selected unit if it has stopped moving
            if (isFollowing)
            {
                Vector3 targetPosition = selectedUnit.position + offset;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
        }
    }
}