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
        //textMeshPro.text = "Health: " + healthSystem.GetCurrentHealth();
        textMeshPro.text = "" + healthSystem.GetCurrentHealth();
    }
}