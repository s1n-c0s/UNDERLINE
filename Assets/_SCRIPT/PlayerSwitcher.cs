using System;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSwitch; // Event to notify when the player switches

    public GameObject[] players; // Array to hold all player GameObjects
    private int currentPlayerIndex = 0; // Index of the current player

    void Start()
    {
        // Ensure at least one player is present
        if (players.Length == 0)
        {
            Debug.LogError("No players assigned. Please assign players in the Unity Editor.");
            enabled = false; // Disable the script to prevent errors
        }
        else
        {
            // Activate the initial player
            SwitchPlayer(currentPlayerIndex);
        }
    }

    void Update()
    {
        // Check for input to switch players (you can customize the input key as needed)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchToNextPlayer();
        }
    }

    void SwitchPlayer(int newIndex)
    {
        // Deactivate the current player
        if (players.Length > currentPlayerIndex) // Safety check
        {
            players[currentPlayerIndex].SetActive(false);
        }

        // Activate the new player
        players[newIndex].SetActive(true);

        // Trigger the OnPlayerSwitch event
        OnPlayerSwitch?.Invoke(players[newIndex]);

        // Update the current player index
        currentPlayerIndex = newIndex;
    }
    
    public void SwitchToNextPlayer()
    {
        // Calculate the index of the next player, wrapping around if necessary
        int nextPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        SwitchPlayer(nextPlayerIndex);
    }
}