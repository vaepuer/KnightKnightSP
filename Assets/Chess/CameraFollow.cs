using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 rotateByAngles = new Vector3(0f, 90f, 0f);
    public float rotationSpeed = 5f;

    private Quaternion targetRotation;
    private bool isRotating = false;

    // Update is called once per frame
    private void Update()
    {
        if (isRotating)
        {
            // Use SLerp to interpolate between the current rotation and the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if we are close enough to the target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.01f)
            {
                transform.rotation = targetRotation;
                isRotating = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Set the target rotation using SLerp
                targetRotation = Quaternion.Euler(transform.eulerAngles + rotateByAngles);
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                // Set the target rotation using SLerp
                targetRotation = Quaternion.Euler(transform.eulerAngles - rotateByAngles);
                isRotating = true;
            }
        }
    }
}
