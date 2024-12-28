using UnityEngine;
using System;
using System.Collections;

public class OrthographicSizeSwitcher : MonoBehaviour
{
    public Camera targetCamera;
    public float orthoSize1 = 5f; // Initial orthographic size
    public float orthoSize2 = 10f; // Second orthographic size
    public float switchDuration = 1.0f; // Duration of the size transition
    public float shakeIntensity = 0.1f; // Intensity of camera shake

    private PlacementSystem placementSystem;

    private bool isSwitching = false;
    private float startTime;
    private float startSize;
    private float targetSize;
    private bool hasShaken = false;

    void Start()
    {
        // Initialize the camera's orthographic size
        targetCamera.orthographicSize = orthoSize1;
    }

    public void OnButtonClick()
    {
        // Check if a size switch is not already in progress
        if (!isSwitching)
        {
            // Toggle the flag to indicate a size switch is in progress
            isSwitching = true;

            // Set the start time and size
            startTime = Time.time;
            startSize = targetCamera.orthographicSize;

            // Determine the target size
            if (targetCamera.orthographicSize == orthoSize1)
                targetSize = orthoSize2;
            else
                targetSize = orthoSize1;
        }

        // Add camera jiggle effect
        StartCoroutine(JiggleCamera(shakeIntensity, 0.1f));
    }

    void Update()
    {
        // Check if a size switch is in progress
        if (isSwitching)
        {
            // Calculate the time elapsed since the switch started
            float elapsedTime = Time.time - startTime;

            // Calculate the new orthographic size using Slerp
            float newSize = Mathf.Lerp(startSize, targetSize, Mathf.SmoothStep(0f, 1f, elapsedTime / switchDuration));

            // Update the camera's orthographic size
            targetCamera.orthographicSize = newSize;

            // Check if the switch is complete
            if (elapsedTime >= switchDuration)
            {
                isSwitching = false;
                hasShaken = false;
            }
        }
    }

    IEnumerator JiggleCamera(float intensity, float duration)
    {
        float startTime = Time.time;
        Vector3 originalPosition = targetCamera.transform.position;

        while (Time.time - startTime < duration)
        {
            Vector3 jiggleOffset = UnityEngine.Random.insideUnitSphere * intensity;
            targetCamera.transform.position = originalPosition + jiggleOffset;
            yield return null;
        }

        // Reset the camera's position
        targetCamera.transform.position = originalPosition;
    }
}
