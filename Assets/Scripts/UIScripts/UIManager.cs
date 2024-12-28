using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public ProjectileArc projectileArc; // Reference to the ProjectileArc script

    public GameObject spellCastingButton; // Reference to your spell casting button

    public void StartCastingSpell()
    {
        if (projectileArc != null)
        {
            //projectileArc.StartCastingEffect(); // Trigger the spell casting effect in the ProjectileArc script
        }
    }

    public void StopCastingSpell()
    {
        if (projectileArc != null)
        {
            //projectileArc.StopCastingEffect(); // Stop the spell casting effect in the ProjectileArc script
        }
    }
}
