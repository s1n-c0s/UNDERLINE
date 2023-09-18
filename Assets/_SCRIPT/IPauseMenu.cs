using UnityEngine;
using UnityEngine.SceneManagement;

public class IPauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject settingsPanel;

    private void Start()
    {
        SetTimeScaleAndPause(false);
        DeactivateAllMenus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        ActivateMenu(pauseMenu);
    }

    public void ResumeGame()
    {
        SetTimeScaleAndPause(false);
        DeactivateAllMenus();
    }

    public void OpenSettingsPanel()
    {
        DeactivateAllMenus();
        ActivateMenu(settingsPanel);
    }

    public void BackToPauseMenu()
    {
        DeactivateAllMenus();
        ActivateMenu(pauseMenu);
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

    private void SetTimeScaleAndPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        isPaused = pause;
    }

    private void DeactivateAllMenus()
    {
        pauseMenu.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void ActivateMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
}