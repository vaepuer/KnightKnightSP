using UnityEngine;
using UnityEngine.UI;

public class ButtonVisibility : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void SetVisibility(bool visible)
    {
        button.interactable = visible;
        button.gameObject.SetActive(visible);
    }
}
