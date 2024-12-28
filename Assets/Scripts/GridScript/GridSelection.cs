using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSelection : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform cursor;  // Reference to your cursor object.

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the screen coordinates to world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Calculate the snapped position
        Vector3 snappedPosition = CalculateSnappedPosition(worldPosition);

        // Update the cursor's position
        cursor.position = snappedPosition;  // Update the cursor's position.
    }

    Vector3 CalculateSnappedPosition(Vector3 inputPosition)
    {
        // Round the input position to the nearest grid cell
        Vector3Int gridPosition = grid.WorldToCell(inputPosition);
        return grid.CellToWorld(gridPosition);
    }
}
