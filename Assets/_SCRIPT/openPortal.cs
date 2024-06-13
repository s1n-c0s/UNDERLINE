using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openPortal : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameManager.Instance.SetGameState(GameManager.GameState.Clear);
        }
    }
}