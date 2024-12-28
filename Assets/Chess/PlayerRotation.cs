using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Vector3 previousPosition;
    private Quaternion targetRotation;
    public float rotationSpeed = 5.0f; // Adjust this speed as needed

    void Start()
    {
        previousPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate the movement direction
        Vector3 movementDirection = (transform.position - previousPosition).normalized;

        if (movementDirection != Vector3.zero)
        {
            // Calculate the target rotation based on the movement direction
            targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        }

        // Smoothly interpolate the rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Update the previous position for the next frame
        previousPosition = transform.position;
    }
}
