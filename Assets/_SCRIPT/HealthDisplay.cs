using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem; // Reference to HealthSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component
    private TextAnimator textAnimator; // Reference to TextAnimator component

    private bool isPlayer = false; // To check if this display is for a player

    void Start()
    {
        gameObject.AddComponent<TextAnimator>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textAnimator = GetComponent<TextAnimator>();

        if (healthSystem.CompareTag("Player"))
        {
            // Subscribe to the OnPlayerSwitch event if this is a player health display
            isPlayer = true;
            PlayerSwitcher.OnPlayerSwitch += HandlePlayerSwitch;

            // Initial setup
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                healthSystem = player.GetComponent<HealthSystem>();
            }
        }
        else
        {
            // For enemies, directly set the health system
            if (healthSystem != null)
            {
                textMeshPro.text = healthSystem.GetCurrentHealth().ToString();
            }
        }

        // Initial display update for player or enemy
        if (healthSystem != null)
        {
            textMeshPro.text = healthSystem.GetCurrentHealth().ToString();
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the OnPlayerSwitch event to avoid memory leaks
        if (isPlayer)
        {
            PlayerSwitcher.OnPlayerSwitch -= HandlePlayerSwitch;
        }
    }

    void Update()
    {
        if (healthSystem != null)
        {
            int currentHealth = healthSystem.GetCurrentHealth();

            if (textMeshPro.text != currentHealth.ToString())
            {
                textAnimator.UpdateTextWithScaleEffect(currentHealth.ToString());
            }
        }
    }

    private void HandlePlayerSwitch(GameObject newPlayer)
    {
        // Update the reference to the healthSystem when the player switches
        healthSystem = newPlayer.GetComponent<HealthSystem>();

        // Optionally, you can immediately update the display to show the new player's health
        if (healthSystem != null)
        {
            textMeshPro.text = healthSystem.GetCurrentHealth().ToString();
        }
    }
}
