using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target; // The object around which the camera will rotate
    public float rotationSpeed = 5.0f; // The speed of camera rotation
    public float smoothTime = 0.2f; // The smoothing time for the rotation

    private float currentRotation = 0.0f; // The current rotation of the camera
    private bool isRotating = false; // Flag to indicate whether the camera is currently rotating
    private Quaternion targetRotation; // The target rotation for the camera

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0 && !isRotating)
        {
            // Rotate the camera by 90 degrees in the direction of the horizontal input
            currentRotation -= 90 * Mathf.Sign(horizontalInput);
            targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentRotation, 0);
            isRotating = true;
        }

        if (horizontalInput == 0 && isRotating)
        {
            isRotating = false;
        }

        // Smoothly rotate the camera towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * rotationSpeed);
    }
}
