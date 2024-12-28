using UnityEngine;

public class ObscureObject : MonoBehaviour
{
    public GameObject objectToObscure;
    public float depthBelowWorld = -100.0f; // Adjust this value based on your scene's scale

    private Vector3 originalPosition;

    private void Start()
    {
        if (objectToObscure == null)
        {
            Debug.LogError("Object to obscure is not assigned.");
            enabled = false; // Disable the script to avoid errors
            return;
        }

        originalPosition = objectToObscure.transform.position;
    }

    public void ObscureObjectOnClick()
    {
        Vector3 newPosition = originalPosition;
        newPosition.y = depthBelowWorld;
        objectToObscure.transform.position = newPosition;
    }
}
