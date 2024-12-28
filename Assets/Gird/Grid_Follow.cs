using UnityEngine;

public class Grid_Follow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Transform objectToAttach;  // The object you want to attach

    void Update()
    {
        if (player != null && objectToAttach != null)
        {
            // Attach the object to the player's position
            objectToAttach.position = player.position;
        }
    }
}
