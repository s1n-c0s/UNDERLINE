using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    private GameObject player; // Reference to the player
    public HealthSystem healthSystem; // Reference to HealthSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component

    private bool isScaling = false;

    void Start()
    {
        // Subscribe to the OnPlayerSwitch event
        PlayerSwitcher.OnPlayerSwitch += HandlePlayerSwitch;

        // Initial setup
        if (healthSystem == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            healthSystem = player.GetComponent<HealthSystem>();
        }

        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void OnDestroy()
    {
        // Unsubscribe from the OnPlayerSwitch event to avoid memory leaks
        PlayerSwitcher.OnPlayerSwitch -= HandlePlayerSwitch;
    }

    void Update()
    {
        int currentHealth = healthSystem.GetCurrentHealth();

        if (textMeshPro.text != currentHealth.ToString())
        {
            if (!isScaling)
            {
                StartCoroutine(UpdateTextWithScaleEffect(currentHealth.ToString()));
            }
        }
    }

    private IEnumerator UpdateTextWithScaleEffect(string newText)
    {
        isScaling = true;

        // Punch scale effect
        textMeshPro.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 0, 1f);

        yield return new WaitForSeconds(0.3f); // Wait for the punch scale effect to finish

        // Update text
        textMeshPro.text = newText;

        // Scale back to original size
        textMeshPro.transform.DOScale(Vector3.one, 0.3f);

        yield return new WaitForSeconds(0.3f); // Wait for the scale back effect to finish

        isScaling = false;
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
