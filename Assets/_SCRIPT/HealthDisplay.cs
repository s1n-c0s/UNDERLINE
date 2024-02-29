using DG.Tweening;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component
    public HealthSystem healthSystem;
    private int lastHealth; // Cache the last health value to minimize updates

    void Start()
    {
        // Initialize the text component
        textMeshPro = GetComponent<TextMeshProUGUI>();

        // Attempt to find and assign the health system if not set in the inspector
        if (healthSystem == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                healthSystem = player.GetComponent<HealthSystem>();
            }
        }

        // Initialize last health to force update in the first frame
        lastHealth = healthSystem != null ? healthSystem.GetCurrentHealth() : -1;
    }

    void Update()
    {
        if (healthSystem == null) return; // Early exit if healthSystem is not set

        int currentHealth = healthSystem.GetCurrentHealth();
        if (currentHealth != lastHealth) // Check if health has changed
        {
            // Update the text only if the health value has changed
            textMeshPro.text = currentHealth.ToString();
            lastHealth = currentHealth; // Update lastHealth to the new value

            // Play punch scale animation
            textMeshPro.transform.DOKill(true); // Kill any previous animations on the transform
            textMeshPro.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 0, 0.5f).SetEase(Ease.OutElastic);
        }
    }
}