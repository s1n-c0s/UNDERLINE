using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTPlayerSwitch : MonoBehaviour
{
    [SerializeField] private List<Sprite> _iconPlayer; // List of player icons
    private Button switchPlayerBT; // Reference to the button component
    private PlayerSwitcher playerSwitcher; // Reference to the PlayerSwitcher component
    private Image buttonImage; // Reference to the Image component on the button

    void Start()
    {
        switchPlayerBT = GetComponent<Button>();
        playerSwitcher = FindObjectOfType<PlayerSwitcher>();
        buttonImage = switchPlayerBT.GetComponent<Image>();

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

        if (buttonImage == null)
        {
            Debug.LogError("Image component not found on the Button");
            return;
        }

        switchPlayerBT.onClick.AddListener(SwitchAndUpdateButton);

        // Subscribe to the OnPlayerSwitch event
        PlayerSwitcher.OnPlayerSwitch += UpdateButtonImage;

        // Set the initial image
        UpdateButtonImage(playerSwitcher.players[playerSwitcher.GetCurrentPlayerIndex()]);
    }

    private void OnDestroy()
    {
        PlayerSwitcher.OnPlayerSwitch -= UpdateButtonImage;
    }

    private void SwitchAndUpdateButton()
    {
        playerSwitcher.SwitchToNextPlayer();
    }

    private void UpdateButtonImage(GameObject currentPlayer)
    {
        int currentIndex = playerSwitcher.GetCurrentPlayerIndex();
        if (currentIndex >= 0 && currentIndex < _iconPlayer.Count)
        {
            buttonImage.sprite = _iconPlayer[currentIndex];
        }
        else
        {
            Debug.LogError("Invalid player index for icon update: " + currentIndex);
        }
    }
}
