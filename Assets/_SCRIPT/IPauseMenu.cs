using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class IPauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private bool isGameOver = false; // Added flag to track game over state
    public List<GameObject> panels;  // List of panels to manage
    private GameObject activePanel;  // Currently active panel

    private TimeLimitController timeLimitController; // Reference to the TimeLimitController

    private void Start()
    {
        SetTimeScaleAndPause(false);
        DeactivateAllMenus();

        // Find and assign the TimeLimitController reference
        timeLimitController = FindObjectOfType<TimeLimitController>();
    }

    private void Update()
    {
        // Update isGameOver from TimeLimitController
        if (timeLimitController != null)
        {
            isGameOver = timeLimitController.IsGameOver;
        }

        if (!isGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        SetTimeScaleAndPause(true);
        ActivateMenu(panels[0]);  // Assuming the first panel is the pause menu
    }

    public void ResumeGame()
    {
        SetTimeScaleAndPause(false);
        DeactivateAllMenus();
    }

    public void OpenSettingsPanel()
    {
        DeactivateAllMenus();
        ActivateMenu(panels[1]);  // Assuming the second panel is the settings panel
    }

    public void BackToPauseMenu()
    {
        DeactivateAllMenus();
        ActivateMenu(panels[0]);  // Assuming the first panel is the pause menu
    }

    public void RestartGame()
    {
        SetTimeScaleAndPause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

    public void SetTimeScaleAndPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        isPaused = pause;
    }

    private void DeactivateAllMenus()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        activePanel = null;
    }

    private void ActivateMenu(GameObject menu)
    {
        DeactivateAllMenus();
        menu.SetActive(true);
        activePanel = menu;
    }
}
