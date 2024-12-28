using UnityEngine;

[ExecuteInEditMode]
public class GridVisualiser : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(10, 10); // Set the size of your grid.
    public float cellSize = 1.0f; // Set the size of your grid cells.

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // Change the color of the grid points.
        for (float x = 0; x < gridSize.x; x += cellSize)
        {
            for (float y = 0; y < gridSize.y; y += cellSize)
            {
                Vector3 position = new Vector3(x, y, 0);
                Gizmos.DrawSphere(transform.position + position, 0.1f); // Adjust the size of the points.
            }
        }
    }
}
