using UnityEngine;

public class SpellCastingEffect : MonoBehaviour
{
    public GameObject spellCastingEffectPrefab;
    private GameObject activeSpell; // Reference to the active spell GameObject
    public GameObject player;

    private void Update()
    {
        transform.position = player.transform.position;
    }

    public void StartCastingEffect()
    {
        // Check if an active spell already exists and destroy it
        if (activeSpell != null)
        {
            Destroy(activeSpell);
        }

        // Spawn the spell GameObject
        activeSpell = Instantiate(spellCastingEffectPrefab, transform.position, Quaternion.identity);

        // Find the player's GameObject (you may need to adjust this based on your setup)
        GameObject player = GameObject.FindWithTag("Player");

        // Parent the spell GameObject to the player's GameObject
        activeSpell.transform.SetParent(player.transform);

        // You can also adjust the spell's position relative to the player if needed
        activeSpell.transform.localPosition = new Vector3(0, 1, 0); // Adjust the position as needed
    }

    public void StopCastingEffect()
    {
        // Check if an active spell exists and destroy it
        if (activeSpell != null)
        {
            Destroy(activeSpell);
        }
    }
}
