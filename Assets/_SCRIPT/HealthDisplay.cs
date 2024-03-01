using System.Collections;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    private GameObject player; // Reference to Player
    public HealthSystem healthSystem; // Reference to HealthSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component

    private bool isScaling = false;

    void Start()
    {
        if (healthSystem == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            healthSystem = player.GetComponent<HealthSystem>();
        }
        
        textMeshPro = GetComponent<TextMeshProUGUI>();
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
}