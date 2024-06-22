using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSwitch; // Event to notify when the player switches

    [SerializeField] private List<GameObject> players; // Array to hold all player GameObjects
    [SerializeField] private int currentPlayerIndex = 0; // Index of the current player
    [SerializeField] private bool firstSwitch = true; // To track if it is the first switch
    [SerializeField] private float Radius = 20f;

    void Start()
    {
        /*players.AddRange(GameObject.FindGameObjectsWithTag("Player"));*/
        
        // Ensure all players except the first one are inactive and positioned correctly
        for (int i = 1; i < players.Count; i++)
        {
            players[i].SetActive(false);
            players[i].transform.position = players[0].transform.position + new Vector3(0, 0, 5);
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
        Rigidbody shadow_rb =players[1].GetComponent<Rigidbody>();
        float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
        // Ensure the new index is within bounds
        if (newIndex < 0 || newIndex >= players.Count)
        {
            Debug.LogError("Invalid player index: " + newIndex);
            return;
        }

        // Only deactivate players[0] if it's not the first switch
        if (firstSwitch)
        {
            players[0].SetActive(false);
            firstSwitch = false;
        }
        else
        {
            players[currentPlayerIndex].GetComponent<PlayerController>().enabled = false;
        }

        if (distance >= Radius)
        {
            players[1].transform.position = players[0].transform.position + new Vector3(0, 0, 5);
        }

       

        if (newIndex == 1)
        {
            shadow_rb.constraints = RigidbodyConstraints.FreezeRotation;
            players[1].GetComponent<Collider>().isTrigger = false;
        }
        else
        {
            shadow_rb.constraints = RigidbodyConstraints.FreezeAll;
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
