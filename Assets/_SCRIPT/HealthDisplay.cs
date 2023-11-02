using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public HealthSystem healthSystem; // Reference to HealthSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component

    void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //textMeshPro.text = "Health: " + healthSystem.GetCurrentHealth();
        textMeshPro.text = "" + healthSystem.GetCurrentHealth();
    }
}