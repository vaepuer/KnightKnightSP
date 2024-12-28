using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform[] points; // An array of two points for the object to move between
    public float duration = 1.0f; // The duration of the movement
    private float startTime; // The time when the movement started
    private int currentIndex = 0; // The current point in the array
    private Vector3 startPos; // The starting position of the movement
    private bool isMoving = false; // Whether the object is currently moving or not

    [Header("Jiggle Physics ;)")] 
    [SerializeField]private float jiggleDuration = 0.25f;
    [SerializeField]private float jiggleMagnitude = 0.1f;

    void Start()
    {
        startPos = transform.position; // Get the starting position of the object
    }

    void Update()
    {
        if (isMoving) // Check if the object is currently moving
        {
            float t = (Time.time - startTime) / duration; // Calculate the progress of the movement
            Vector3 newPosition = Vector3.Lerp(startPos, points[currentIndex].position, t); // Calculate the new position using Lerp
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition; // Set the new position of the object

            if (t >= 1.0f) // Check if the movement is complete
            {
                isMoving = false; // Set isMoving to false
                startTime = 0; // Reset the start time
                startPos = transform.position; // Set the new starting position

                // Add jiggle animation
                StartCoroutine(JiggleAnimation());
            }
        }
        else // Object is not currently moving
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // Check if the up arrow key was pressed
            {
                if (currentIndex == points.Length - 1) // Check if the object is at the top point
                {
                    return; // Do nothing
                }

                currentIndex++; // Increment the current index
                startTime = Time.time; // Set the start time to the current time
                startPos = transform.position; // Set the starting position to the current position
                isMoving = true; // Set isMoving to true
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)) // Check if the down arrow key was pressed
            {
                if (currentIndex == 0) // Check if the object is at the bottom point
                {
                    return; // Do nothing
                }

                currentIndex--; // Decrement the current index
                startTime = Time.time; // Set the start time to the current time
                startPos = transform.position; // Set the starting position to the current position
                isMoving = true; // Set isMoving to true
            }
        }
    }

    IEnumerator JiggleAnimation()
    {
        // Define the jiggle parameters
        

        Vector3 startPos = transform.position; // Get the starting position of the object

        float elapsedTime = 0f;
        while (elapsedTime < jiggleDuration)
        {
            // Calculate the jiggle amount based on the elapsed time
            float t = elapsedTime / jiggleDuration;
            float jiggleAmount = Mathf.Lerp(jiggleMagnitude, 0f, t);

            // Calculate a random offset for the jiggle
            float xOffset = Random.Range(-jiggleAmount, jiggleAmount);
            float yOffset = Random.Range(-jiggleAmount, jiggleAmount);

            // Apply the jiggle offset to the object's position
            Vector3 newPosition = startPos + new Vector3(xOffset, yOffset, 0f);
            transform.position = newPosition;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Set the object's position to the end point
        transform.position = points[currentIndex].position;
    }

}
