using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileArc : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform playerTransform;
    public float arcHeight = 5f; // Height of the arc
    public int numPoints = 10; // Number of points

    private GameObject arcPointsParent; // The parent object for arc points
    private List<GameObject> arcPoints = new List<GameObject>();
    private Camera mainCamera;
    private Vector3 targetPosition;

    [SerializeField] private float floorHeightFloat;

    private bool shouldHideParent = false; // A flag to control whether the parent object should be hidden.

    void Awake()
    {
        mainCamera = Camera.main; // Get the main camera

        // Create a new empty GameObject to parent the arc points
        arcPointsParent = new GameObject("ArcPointsParent");

        CreateArcPoints();
    }

    void CreateArcPoints()
    {
        for (int i = 0; i < numPoints; i++)
        {
            GameObject newPoint = Instantiate(projectilePrefab);
            newPoint.transform.SetParent(arcPointsParent.transform); // Parent the point to the arcPointsParent
            arcPoints.Add(newPoint);
        }
    }

    void Update()
    {
        if (shouldHideParent)
        {
            HideParentObject();
        }
        else
        {
            targetPosition = GetMouseWorldPosition();
            ShowParentObject();
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z - playerTransform.position.z);
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    public void SetHideParent(bool shouldHide)
    {
        shouldHideParent = shouldHide;
    }

    void ShowParentObject()
    {
        arcPointsParent.transform.position = Vector3.zero; // Set the parent's position back to zero to make it visible
    }

    void HideParentObject()
    {
        arcPointsParent.transform.position = new Vector3(arcPointsParent.transform.position.x, -1000.0f, arcPointsParent.transform.position.z);
        // Move the parent object far below the game world to obscure it from view
    }
}
