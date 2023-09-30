using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeLimitController : MonoBehaviour
{
    public float totalTime = 60f;  // Total time in seconds
    private float currentTime;     // Current time left
    private bool isGameOver = false;  // Flag to track game over state

    public TextMeshProUGUI timeText;  // Reference to the TextMeshPro component
    public GameObject gameOverUI;     // Reference to the game over UI GameObject
    public IPauseMenu pauseMenuScript;  // Reference to the IPauseMenu script

    private void Start()
    {
        currentTime = totalTime;  // Initialize the timer

        // Update the UI TextMeshPro with the initial time
        UpdateUIText();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            // Update the timer only if the game is not over
            currentTime -= Time.deltaTime;

            // Update the UI TextMeshPro every frame
            UpdateUIText();

            // Check if time has run out
            if (currentTime <= 0f)
            {
                GameOver();
            }
        }
    }

    private void UpdateUIText()
    {
        // Update the UI TextMeshPro with the current time
        timeText.text = $"Time: {Mathf.CeilToInt(currentTime)}s";
    }
    
    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    public void GameOver()
    {
        // Implement your game over logic here
        Debug.Log("Game Over!");
        isGameOver = true;

        // Activate the game over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Pause the game through the IPauseMenu script
        if (pauseMenuScript != null)
        {
            pauseMenuScript.SetTimeScaleAndPause(true);
        }

        // You might want to load a game over scene, display a game over menu, etc.
        // Example: SceneManager.LoadScene("GameOverScene");
    }

    public void RestartGame()
    {
        currentTime = totalTime;  // Reset the timer
        isGameOver = false;  // Reset the game over state

        // Update the UI TextMeshPro with the reset time
        UpdateUIText();

        // Deactivate the game over UI if it's active
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }
}
