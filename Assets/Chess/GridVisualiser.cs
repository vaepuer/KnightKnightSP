using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GridVisualizer : MonoBehaviour
{
    // Reference to the ClickToMove script
    public ClickArrow clickToMoveScript;

    // Color for grid lines
    public Color gridColor = Color.cyan;

    private void OnDrawGizmos()
    {
        if (clickToMoveScript == null)
        {
            Debug.LogWarning("GridVisualizer: Please assign a ClickToMove script.");
            return;
        }

        Gizmos.color = gridColor;

        // Draw vertical grid lines
        for (float x = -10.0f; x <= 10.0f; x += clickToMoveScript.gridSize)
        {
            Gizmos.DrawLine(new Vector3(x, 0, -10.0f), new Vector3(x, 0, 10.0f));
        }

        // Draw horizontal grid lines
        for (float z = -10.0f; z <= 10.0f; z += clickToMoveScript.gridSize)
        {
            Gizmos.DrawLine(new Vector3(-10.0f, 0, z), new Vector3(10.0f, 0, z));
        }
    }
}
