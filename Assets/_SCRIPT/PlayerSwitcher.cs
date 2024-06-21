using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSwitch; // Event to notify when the player switches

    [SerializeField] private List<GameObject> players; // Array to hold all player GameObjects
    [SerializeField] private int currentPlayerIndex = 0; // Index of the current player

    void Start()
    {
        /*players.AddRange(GameObject.FindGameObjectsWithTag("Player"));*/
        
        // Ensure all players except the first one are inactive and positioned correctly
        for (int i = 1; i < players.Count; i++)
        {
            players[i].SetActive(false);
            players[i].transform.position = players[0].transform.position;
        }

        // Ensure at least one player is present
        if (players.Count == 0)
        {
            enabled = false; // Disable the script to prevent errors
            Debug.LogError("No players found with the 'Player' tag.");
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
        // Ensure the new index is within bounds
        if (newIndex < 0 || newIndex >= players.Count)
        {
            Debug.LogError("Invalid player index: " + newIndex);
            return;
        }

        // Deactivate the current player
        if (players.Count > currentPlayerIndex && players[currentPlayerIndex] != null)
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
        int nextPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        SwitchPlayer(nextPlayerIndex);
    }
}
