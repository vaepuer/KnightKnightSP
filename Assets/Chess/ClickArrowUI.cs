using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickArrowUI : MonoBehaviour
{
    [Header("Stuff")]
    private bool isMoving = false;
    private Vector3 targetPosition;
    [SerializeField] private float moveSpeed = 5.0f; // Adjust this speed as needed

    [Header("Grid Movement")]
    public float maxRaycastDistance = 10.0f;
    public float gridSize = 5.0f;
    public Button[] clickableButtons;
    public ButtonVisibility[] buttonVisibilities;

    [Header("Stamina Handle")]
    public int maxStamina = 3; // Set the maximum stamina
    public int currentStamina; // Track the current stamina

    [Header("UI Handling")]
    public StaminaUI staminaUI;
    public WalkedIntoWall walkedIntoWall;

    [Header("Animation Handling")]
    public Animator playerAnimator;

    [Header("Markers")]
    public Transform minXMarker; // Reference to the GameObject representing the minimum X bound
    public Transform maxXMarker; // Reference to the GameObject representing the maximum X bound
    public Transform minZMarker; // Reference to the GameObject representing the minimum Z bound
    public Transform maxZMarker; // Reference to the GameObject representing the maximum Z bound

    public LayerMask obstacleLayer; // Define the layer(s) for obstacles

    private bool isClickableEnabled = true;
    private bool isCastingSpell = false;

    void Start()
    {
        foreach (Button button in clickableButtons)
        {
            button.onClick.AddListener(() => HandleButtonClick(button));
        }

        currentStamina = maxStamina;


    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Ensure the player stays within the playable area defined by the GameObject markers
            float clampedX = Mathf.Clamp(transform.position.x, minXMarker.position.x, maxXMarker.position.x);
            float clampedZ = Mathf.Clamp(transform.position.z, minZMarker.position.z, maxZMarker.position.z);
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);

            DisableUIButtonsIfObstructed();

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
                ToggleClickableButtons(true);
                playerAnimator.Play("Pal_Idle");
            }
        }

        CastingSpell();
    }

    bool IsConjuringAnimationPlaying()
    {
        // Replace "Conjuring" with the actual name of your animation state.
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Pal_Conjure");
    }

    void CastingSpell()
    {
        if (!isCastingSpell && IsConjuringAnimationPlaying())
        {
            isCastingSpell = true;
            //castingEffect.StartCastingEffect();
            ToggleClickableButtons(false, 0.0f);
            Debug.Log("Arrows Hidden");
        }
        else if (isCastingSpell && !IsConjuringAnimationPlaying())
        {
            isCastingSpell = false;
            ToggleClickableButtons(true, 1.0f);
            Debug.Log("Arrows Shown");
        }
    }

    void DisableUIButtonsIfObstructed()
    {
        RaycastHit hit;
        Vector3 playerPosition = transform.position;
        bool canMoveFurther = false;

        foreach (Button button in clickableButtons)
        {
            Vector3 buttonPosition = button.transform.position;
            Vector3 buttonDirection = buttonPosition - playerPosition;

            // Visualize the raycast (add this line)
            Debug.DrawRay(playerPosition, buttonDirection, Color.red, 0.2f); // Red line for visualization

            if (Physics.Raycast(playerPosition, buttonDirection, out hit, maxRaycastDistance, obstacleLayer))
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
                canMoveFurther = true; // The player can move further in this direction
            }

            // Check if the button's direction goes outside of the markers
            if (!IsWithinMarkers(playerPosition + GetMoveDirection(button.transform) * gridSize))
            {
                button.interactable = false;
            }
        }

        // If the player can't move any further, disable all buttons
        if (!canMoveFurther || currentStamina == 0)
        {
            foreach (Button button in clickableButtons)
            {
                button.interactable = false;
            }
        }
    }

    void EnableArrowButtons()
    {
        foreach (Button button in clickableButtons)
        {
            button.interactable = true;
        }
    }

    void HandleButtonClick(Button button)
    {
        if (!isMoving && isClickableEnabled && currentStamina > 0)
        {
            Vector3 moveDirection = GetMoveDirection(button.transform);
            Vector3 potentialTargetPosition = transform.position + moveDirection * gridSize;

            if (moveDirection != Vector3.zero && IsWithinMarkers(potentialTargetPosition))
            {
                if (!CheckForObstacles(potentialTargetPosition))
                {
                    targetPosition = potentialTargetPosition;
                    isMoving = true;
                    ToggleClickableButtons(false);
                    currentStamina--; // Decrement the stamina

                    // Trigger the "Walk" animation
                    playerAnimator.Play("Pal_Walk");
                }
            }
        }
    }

    bool IsWithinMarkers(Vector3 position)
    {
        // Check if the position is within the markers
        return position.x >= minXMarker.position.x && position.x <= maxXMarker.position.x &&
               position.z >= minZMarker.position.z && position.z <= maxZMarker.position.z;
    }

    Vector3 GetMoveDirection(Transform buttonTransform)
    {
        Vector3 buttonPosition = buttonTransform.position;
        Vector3 playerPosition = transform.position;
        Vector3 moveDirection = Vector3.zero;

        Vector3 positionDifference = buttonPosition - playerPosition;

        if (Mathf.Abs(positionDifference.x) > Mathf.Abs(positionDifference.z))
        {
            moveDirection = new Vector3(Mathf.Sign(positionDifference.x), 0, 0);
        }
        else
        {
            moveDirection = new Vector3(0, 0, Mathf.Sign(positionDifference.z));
        }

        return moveDirection;
    }

    void ToggleClickableButtons(bool enable, float alpha = 1.0f)
    {
        isClickableEnabled = enable;

        foreach (ButtonVisibility buttonVisibility in buttonVisibilities)
        {
            buttonVisibility.SetVisibility(enable);
        }

        // Change the visibility (alpha) of the buttons
        foreach (Button button in clickableButtons)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha;
            }
        }
    }

    bool CheckForObstacles(Vector3 target)
    {
        Vector3 playerPosition = transform.position;
        Vector3 direction = (target - playerPosition).normalized;
        float distance = Vector3.Distance(playerPosition, target);

        if (Physics.Raycast(playerPosition, direction, distance, obstacleLayer))
        {
            int randomIndex = Random.Range(0, walkedIntoWall.obstacleMessages.Length);
            walkedIntoWall.ShowFloatingUI(walkedIntoWall.obstacleMessages[randomIndex]);
            return true;
        }
        return false;
    }

    public void StartPlayerTurn()
    {
        currentStamina = maxStamina; // Reset the player's stamina at the start of their turn
        ToggleClickableButtons(true);
        EnableArrowButtons(); // Re-enable arrow buttons at the start of the turn
    }
}
