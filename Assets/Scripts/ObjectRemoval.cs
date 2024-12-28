using UnityEngine;

public class ObjectRemoval : MonoBehaviour
{
    public string objectTag = "ArcPrefab"; // Set this to the tag of your spawned objects

    // Add a method that can be called to remove the objects
    public void RemoveObjects()
    {
        GameObject[] objectsToRemove = GameObject.FindGameObjectsWithTag(objectTag);

        foreach (GameObject obj in objectsToRemove)
        {
            Destroy(obj);
        }
    }
}
