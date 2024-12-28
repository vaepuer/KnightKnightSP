using UnityEngine;

public class SpellProjectilePrediction : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform playerTransform;
    public float arcValue;

    void Update()
    {
        if (Input.GetButton("CastSpellButton")) // Replace with your casting input
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 playerPosition = playerTransform.position;

            // Set the line positions
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, playerPosition);
            lineRenderer.SetPosition(1, mousePosition);

            // Offset the line vertically to create an arc effect
            Vector3 midpoint = (playerPosition + mousePosition) / 2;
            midpoint.y += arcValue; // Adjust the height as needed
            lineRenderer.SetPosition(1, midpoint);

            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
