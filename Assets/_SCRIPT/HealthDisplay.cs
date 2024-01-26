using DG.Tweening;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    private GameObject player; // Reference to Player
    public HealthSystem healthSystem; // Reference to HealthSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component

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
            textMeshPro.text = currentHealth.ToString();
            
            // Add punch scale effect when the value changes
            textMeshPro.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 0, 1f);
        }
    }
}