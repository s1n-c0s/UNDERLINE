using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    public GameObject gameplayPanel; // Reference to the gameplay panel
    public GameObject menuGroupPanel; // Reference to the menu group panel
    private bool isActive = false; // Flag to track the menu panel's visibility

    void Start()
    {
        // Ensure the menu panel is initially deactivated
        SetPanelActive(menuGroupPanel, false);

        // Set initial states for other panels if needed
        SetPanelActive(gameplayPanel, true);
    }

    public void ShowPanel()
    {
        // Toggle the isActive flag and set the menu panel's active state accordingly
        isActive = !isActive;

        // Use a single method to set the active state of the menu group panel without fade effect
        SetPanelActive(menuGroupPanel, isActive);

        // Set the gameplay panel active state based on the opposite of the isActive flag without fade effect
        SetPanelActive(gameplayPanel, !isActive);
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            // Set the panel active state without fade effect
            panel.SetActive(active);
        }
    }
}