using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    public Transform selectedUnit;
    public bool unitSelected = false;

    Animator selectedAnimator; // Animator for the currently selected unit

    List<Node> path = new List<Node>();

    GridManager gridManager;
    Pathfinding pathFinder;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<Pathfinding>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            bool hasHit = Physics.Raycast(ray, out hit);

            if (hasHit)
            {
                Debug.Log("Raycast hit: " + hit.transform.name);

                if (hit.transform.tag == "Tile")
                {
                    if (unitSelected)
                    {
                        Vector2Int targetCords = hit.transform.GetComponent<Tile>().cords;
                        Vector2Int startCords = new Vector2Int((int)selectedUnit.transform.position.x, (int)selectedUnit.transform.position.z) / gridManager.UnityGridSize;
                        pathFinder.SetNewDestination(startCords, targetCords);
                        RecalculatePath(true);
                    }
                }
                if (hit.transform.tag == "Unit")
                {
                    selectedUnit = hit.transform;
                    selectedAnimator = selectedUnit.GetComponent<Animator>(); // Get the Animator component of the selected unit
                    unitSelected = true;

                    if (selectedAnimator != null)
                    {
                        Debug.Log("Animator found for unit: " + selectedUnit.name);
                    }
                    else
                    {
                        Debug.LogError("No Animator found for unit: " + selectedUnit.name);
                    }
                }
            }
        }
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();
        if (resetPath)
        {
            coordinates = pathFinder.StartCords;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        StopAllCoroutines();
        path.Clear();
        path = pathFinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            Vector3 startPosition = selectedUnit.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].cords);

            // Adjust the end position to the correct height
            RaycastHit hit;
            if (Physics.Raycast(endPosition + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity))
            {
                endPosition.y = hit.point.y;
            }

            float travelPercent = 0f;

            selectedUnit.LookAt(endPosition);

            if (selectedAnimator != null)
            {
                selectedAnimator.SetBool("IsWalking", true);
                Debug.Log("Setting IsWalking to true for " + selectedUnit.name);
            }

            // Move to the target position
            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * movementSpeed;
                selectedUnit.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }

            // If the next node is on a different Y level, teleport to the new height
            if (i < path.Count - 1)
            {
                Vector3 nextPosition = gridManager.GetPositionFromCoordinates(path[i + 1].cords);
                if (Physics.Raycast(nextPosition + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity))
                {
                    nextPosition.y = hit.point.y;
                }

                if (Mathf.Abs(endPosition.y - nextPosition.y) > 0.1f)
                {
                    selectedUnit.position = new Vector3(nextPosition.x, nextPosition.y, nextPosition.z);
                }
            }

            if (selectedAnimator != null)
            {
                selectedAnimator.SetBool("IsWalking", false);
                Debug.Log("Setting IsWalking to false for " + selectedUnit.name);
            }

            Debug.Log("Reached destination: " + endPosition);
        }
    }
}
