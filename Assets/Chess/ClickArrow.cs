using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickArrow : MonoBehaviour
{
    private Camera mainCamera;
    private Transform playerTransform;
    private bool isMoving = false;
    private Vector3 targetPosition;
    private float moveSpeed = 5.0f; // Adjust this speed as needed

    // Grid size, set to your desired grid distance
    public float gridSize = 5.0f;

    // Reference to clickable objects
    public GameObject[] clickableObjects;

    private bool isClickableEnabled = true;

    void Start()
    {
        mainCamera = Camera.main;
        playerTransform = transform;
    }

    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0) && isClickableEnabled)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ClickableObjects")))
            {
                if (hit.transform.IsChildOf(playerTransform))
                {
                    // Determine the direction to move based on the hit normal
                    Vector3 moveDirection = GetMoveDirection(hit.normal);

                    if (moveDirection != Vector3.zero)
                    {
                        // Calculate the target position by rounding to the nearest grid position
                        Vector3 offset = moveDirection * gridSize;
                        targetPosition = playerTransform.position + offset;
                        isMoving = true;

                        // Disable clickable objects
                        ToggleClickableObjects(false);
                    }
                }
            }
        }

        if (isMoving)
        {
            // Move the player towards the target position
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(playerTransform.position, targetPosition) < 0.01f)
            {
                isMoving = false;

                // Re-enable clickable objects
                ToggleClickableObjects(true);
            }
        }
    }

    // Determine the direction based on the hit normal
    private Vector3 GetMoveDirection(Vector3 normal)
    {
        if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y) && Mathf.Abs(normal.x) > Mathf.Abs(normal.z))
        {
            return new Vector3(Mathf.Sign(normal.x), 0, 0);
        }
        else if (Mathf.Abs(normal.y) > Mathf.Abs(normal.x) && Mathf.Abs(normal.y) > Mathf.Abs(normal.z))
        {
            return new Vector3(0, Mathf.Sign(normal.y), 0);
        }
        else
        {
            return new Vector3(0, 0, Mathf.Sign(normal.z));
        }
    }

    // Round a position to the nearest grid position
    private Vector3 RoundToGrid(Vector3 input)
    {
        return new Vector3(
            Mathf.Round(input.x / gridSize) * gridSize,
            input.y,
            Mathf.Round(input.z / gridSize) * gridSize
        );
    }

    // Toggle the clickable objects' colliders and visibility
    private void ToggleClickableObjects(bool enable)
    {
        isClickableEnabled = enable;

        foreach (GameObject obj in clickableObjects)
        {
            Collider col = obj.GetComponent<Collider>();
            Renderer renderer = obj.GetComponent<Renderer>();

            if (col != null)
            {
                col.enabled = enable;
            }

            if (renderer != null)
            {
                renderer.enabled = enable;
            }
        }
    }
}
