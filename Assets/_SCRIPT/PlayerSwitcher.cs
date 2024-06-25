using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerSwitch;
    public List<GameObject> players = new List<GameObject>();
    
    [SerializeField] private int currentPlayerIndex = 0;
    [SerializeField] private float radius = 20f;
    [SerializeField] private int forwardOffset = 5;

    private void Start()
    {
        for (int i = 1; i < players.Count; i++)
        {
            players[i].SetActive(false);
            PositionPlayerInFront(players[i], players[i].transform, forwardOffset);
        }

        if (players.Count == 0)
        {
            enabled = false;
            Debug.LogError("No players found with the 'Player' tag.");
        }
        else
        {
            // Manually activate the initial player and trigger the OnPlayerSwitch event
            players[currentPlayerIndex].SetActive(true);
            players[currentPlayerIndex].GetComponent<PlayerController>().enabled = true;
            OnPlayerSwitch?.Invoke(players[currentPlayerIndex]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchToNextPlayer();
        }
    }

    public void SwitchPlayer(int newIndex)
    {
        if (newIndex < 0 || newIndex >= players.Count)
        {
            Debug.LogError("Invalid player index: " + newIndex);
            return;
        }

        Rigidbody shadowRb = players[1].GetComponent<Rigidbody>();
        Collider shadowCollider = players[1].GetComponent<Collider>();
        float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
      
        players[currentPlayerIndex].GetComponent<PlayerController>().enabled = false;

        if (distance >= radius)
        {
            PositionPlayerInFront(players[1], players[0].transform, forwardOffset);
        }

        if (newIndex == 1)
        {
            shadowRb.constraints = RigidbodyConstraints.FreezeRotation;
            shadowCollider.isTrigger = false;
        }
        else
        {
            shadowRb.constraints = RigidbodyConstraints.FreezeAll;
            shadowCollider.isTrigger = true;
        }

        players[newIndex].SetActive(true);
        players[newIndex].GetComponent<PlayerController>().enabled = true;

        currentPlayerIndex = newIndex;

        OnPlayerSwitch?.Invoke(players[newIndex]);
    }

    public void SwitchToNextPlayer()
    {
        int nextPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        SwitchPlayer(nextPlayerIndex);
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }

    private void PositionPlayerInFront(GameObject player, Transform referenceTransform, float offsetDistance)
    {
        Vector3 offset = referenceTransform.position + referenceTransform.forward * offsetDistance;
        player.transform.position = offset;
    }
}
