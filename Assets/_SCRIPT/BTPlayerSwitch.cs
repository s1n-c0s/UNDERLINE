using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTPlayerSwitch : MonoBehaviour
{
    [SerializeField] private List<Sprite> playerIcons;
    private Button switchPlayerButton;
    private PlayerSwitcher playerSwitcher;
    private Image buttonImage;

    private void Start()
    {
        InitializeComponents();

        switchPlayerButton.onClick.AddListener(SwitchAndUpdateButton);

        PlayerSwitcher.OnPlayerSwitch += UpdateButtonImage;

        UpdateButtonImage(playerSwitcher.players[playerSwitcher.GetCurrentPlayerIndex()]);
    }

    private void OnDestroy()
    {
        PlayerSwitcher.OnPlayerSwitch -= UpdateButtonImage;
    }

    private void InitializeComponents()
    {
        switchPlayerButton = GetComponent<Button>();
        playerSwitcher = FindObjectOfType<PlayerSwitcher>();
        buttonImage = switchPlayerButton.GetComponent<Image>();

        if (!switchPlayerButton || !playerSwitcher || !buttonImage)
        {
            Debug.LogError("Missing required component.");
            enabled = false;
        }
    }

    private void SwitchAndUpdateButton()
    {
        playerSwitcher.SwitchToNextPlayer();
    }

    private void UpdateButtonImage(GameObject currentPlayer)
    {
        int currentIndex = playerSwitcher.GetCurrentPlayerIndex();
        if (currentIndex >= 0 && currentIndex < playerIcons.Count)
        {
            buttonImage.sprite = playerIcons[currentIndex];
        }
        else
        {
            Debug.LogError("Invalid player index for icon update: " + currentIndex);
        }
    }
}