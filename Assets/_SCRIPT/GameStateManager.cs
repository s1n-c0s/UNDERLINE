using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public enum GameState
    {
        Start,
        Playing,
        Paused,
        Clear,
        GameOver
        // Add more states as needed
    }

    public GameState CurrentGameState { get; private set; }

    private void Awake()
    {
        /*if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    private void Start()
    {
        // Set the initial game state
        SetGameState(GameState.Start);
    }

    public void SetGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;

        // Handle state-specific logic
        switch (newGameState)
        {
            case GameState.Start:
                // Implement logic for the main menu state
                Time.timeScale = 1f; // Ensure time scale is normal
                break;

            case GameState.Playing:
                // Implement logic for the playing state
                Time.timeScale = 1f; // Ensure time scale is normal
                break;

            case GameState.Paused:
                // Implement logic for the paused state
                Time.timeScale = 0f; // Pause the game
                break;
            
            case GameState.Clear:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                // Implement logic for the game over state
                Time.timeScale = 0f; // Ensure time scale is normal
                break;
        }
    }

    // Add other methods or events as needed for your game
}