using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSwitch; // Event to notify when the player switches

    [SerializeField] private List<GameObject> players = new List<GameObject>(); // List to hold all player GameObjects
    [SerializeField] private int currentPlayerIndex = 0; // Index of the current player
    [SerializeField] private bool firstSwitch = true; // To track if it is the first switch
    [SerializeField] private float radius = 20f; // Distance threshold
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 0, 5); // Position offset for player placement

    private void Start()
    {
        // Ensure all players except the first one are inactive and positioned correctly
        for (int i = 1; i < players.Count; i++)
        {
            players[i].SetActive(false);
            players[i].transform.position = players[0].transform.position + positionOffset;
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

    private void Update()
    {
        // Check for input to switch players (you can customize the input key as needed)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchToNextPlayer();
        }
    }

    private void SwitchPlayer(int newIndex)
    {
        if (newIndex < 0 || newIndex >= players.Count)
        {
            Debug.LogError("Invalid player index: " + newIndex);
            return;
        }

        Rigidbody shadowRb = players[1].GetComponent<Rigidbody>();
        float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);

        // Deactivate the current player
        if (firstSwitch)
        {
            players[0].SetActive(false);
            firstSwitch = false;
        }
        else
        {
            players[currentPlayerIndex].GetComponent<PlayerController>().enabled = false;
        }

        // Reset the position of the shadow player if it exceeds the radius
        if (distance >= radius)
        {
            players[1].transform.position = players[0].transform.position + positionOffset;
        }

        // Update shadow player's constraints and collider based on the new index
        if (newIndex == 1)
        {
            shadowRb.constraints = RigidbodyConstraints.FreezeRotation;
            players[1].GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            shadowRb.constraints = RigidbodyConstraints.FreezeAll;
            players[1].GetComponent<Collider>().isTrigger = true;
        }

        // Activate the new player
        players[newIndex].SetActive(true);
        players[newIndex].GetComponent<PlayerController>().enabled = true;

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
