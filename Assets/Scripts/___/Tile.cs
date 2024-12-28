using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool blocked;

    public Vector2Int cords;

    GridManager gridManager;
    public MeshRenderer tileMesh;


    void Start()
    {
        SetCords();
        tileMesh = GetComponentInChildren<MeshRenderer>();
        if (blocked)
        {
            gridManager.BlockNode(cords);
            //And Turn Tiles Off
            //tileMesh.enabled = false;
        }
    }

    private void SetCords()
    {
        gridManager = FindObjectOfType<GridManager>();
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;

        cords = new Vector2Int(x / gridManager.UnityGridSize, z / gridManager.UnityGridSize);
    }
}