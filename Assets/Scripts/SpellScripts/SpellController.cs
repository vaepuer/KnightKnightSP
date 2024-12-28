using UnityEngine;
using UnityEngine.UI;

public class SpellController : MonoBehaviour
{
    public GameObject spell; // Reference to the spell object
    public Button spellButton; // Reference to the UI button
    public Animator spellAnimator; // Reference to the spell's animator

    private bool isSpellActive = false;

    private void Start()
    {
        // Attach a click event listener to the button
        spellButton.onClick.AddListener(ToggleSpell);
    }

    private void ToggleSpell()
    {
        isSpellActive = !isSpellActive; // Toggle the spell's active state

        // Enable or disable the spell object based on the state
        spell.SetActive(isSpellActive);

        //Update the button text based on the state
        //spellButton.GetComponentInChildren<Text>().text = isSpellActive ? "Disable Spell" : "Enable Spell";

        // Control the spell's animation
        if (spellAnimator != null)
        {
            spellAnimator.SetBool("IsConjuring", isSpellActive);
        }
    }
}
