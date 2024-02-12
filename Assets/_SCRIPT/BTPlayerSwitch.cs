using UnityEngine;
using UnityEngine.UI;

public class BTPlayerSwitch : MonoBehaviour
{
    private Button switchPlayerBT; // This will reference the button component on the same GameObject.
    private PlayerSwitcher playerSwitcher; // This will reference the PlayerSwitcher component.

    void Start()
    {
        // Get the Button component from this GameObject
        switchPlayerBT = GetComponent<Button>();

        // Attempt to find the PlayerSwitcher component in the scene
        // Assuming there is one PlayerSwitcher component managing players.
        playerSwitcher = FindObjectOfType<PlayerSwitcher>(); // This finds the PlayerSwitcher component in the scene

        if (switchPlayerBT == null)
        {
            Debug.LogError("Button component not found on the GameObject");
            return;
        }

        if (playerSwitcher == null)
        {
            Debug.LogError("PlayerSwitcher component not found in the scene");
            return;
        }

        // Add a listener to the button to call the SwitchToNextPlayer method on click
        switchPlayerBT.onClick.AddListener(playerSwitcher.SwitchToNextPlayer); // Assuming you have this method public in your PlayerSwitcher
    }
}