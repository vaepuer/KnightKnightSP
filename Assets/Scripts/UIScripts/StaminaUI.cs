using UnityEngine;
using TMPro;

public class StaminaUI : MonoBehaviour
{
    public ClickArrowUI clickArrowUI;
    public TextMeshProUGUI currentStaminaText;

    void Awake()
    {
        currentStaminaText.text = "[Stamina : " + clickArrowUI.maxStamina + "]";
    }

    void LateUpdate()
    {
        currentStaminaText.text = "[Stamina : " + clickArrowUI.currentStamina + "]";
    }
}
