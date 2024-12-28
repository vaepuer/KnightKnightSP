using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private ClickArrowUI clickArrowUI;

    private void Awake()
    {
        GetComponent<ClickArrowUI>();
    }
    public void ExecuteTurn()
    {
        // Enable the arrow buttons when the player's turn starts
        clickArrowUI.StartPlayerTurn();
    }
}
