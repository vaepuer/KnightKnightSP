using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume; // assign this in the inspector
    [SerializeField] private float changeAmount = 0.1f; // amount to change the post-processing effect by
    [SerializeField] private float resetSpeed = 1f; // speed at which to reset the post-exposure value back to its original value
    private ColorAdjustments colorAdjust; // the color adjustments effect to adjust
    [SerializeField] private float currentPostExposure; // the current post-exposure value
    private float originalPostExposure; // the original post-exposure value
    private bool isMoving = false; // flag to track if the object is moving

    private void Start()
    {
        if (!postProcessingVolume) // if the post-processing volume isn't assigned, find it in the scene
        {
            postProcessingVolume = FindObjectOfType<Volume>();
        }

        if (postProcessingVolume.profile.TryGet(out colorAdjust)) // try to get the color adjustments effect from the post-processing profile
        {
            currentPostExposure = colorAdjust.postExposure.value; // set the current post-exposure value
            originalPostExposure = currentPostExposure; // set the original post-exposure value
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // if the up arrow key is pressed
        {
            currentPostExposure += changeAmount; // increase the post-exposure value
            colorAdjust.postExposure.Override(currentPostExposure); // apply the new post-exposure value to the effect
            isMoving = true; // set the moving flag to true
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) // if the down arrow key is pressed
        {
            currentPostExposure += changeAmount; // decrease the post-exposure value
            colorAdjust.postExposure.Override(currentPostExposure); // apply the new post-exposure value to the effect
            isMoving = true; // set the moving flag to true
        }

        if (!isMoving) // if the object is not moving
        {
            currentPostExposure = Mathf.Lerp(currentPostExposure, originalPostExposure, resetSpeed * Time.deltaTime); // gradually reset the post-exposure value back to its original value
            colorAdjust.postExposure.Override(currentPostExposure); // apply the new post-exposure value to the effect
        }
    }

    private void FixedUpdate()
    {
        isMoving = GetComponent<Rigidbody>().velocity.magnitude > 0; // check if the object is moving
    }
}
