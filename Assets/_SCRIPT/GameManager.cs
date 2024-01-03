using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject VFXclear;
    public GameObject VFXgameover;

    public GameObject player;
    public EnemyDetectorArea enemyDetectorArea;
    public bool isPlaying;

    public enum GameState
    {
        Playing,
        Clear,
        GameOver
        // Add more states as needed
    }

    public GameState CurrentGameState { get; private set; }

    private float countdownTimer;
    private const float CountdownDuration = 2f;
    
    private float delayBeforeWinCheck = 2f; // Adjust the delay as needed
    
    private void Start()
    {
        CurrentGameState = GameState.Playing;
    }

    private void LateUpdate()
    {
        if (isPlaying && CurrentGameState == GameState.Playing)
        {
            isKillAllEnemy();
            CheckPlayerHealth();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
        ResetCountdownTimer();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;

        switch (newGameState)
        {
            case GameState.Playing:
                Debug.Log("Playing");
                isPlaying = true;
                break;
            case GameState.Clear:
                Debug.Log("Game Clear");
                ShowEffect(VFXclear);
                isPlaying = false;
                break;

            case GameState.GameOver:
                Debug.Log("Game Over");
                ShowEffect(VFXgameover);
                isPlaying = false;
                break;
        }
    }

    private void isKillAllEnemy()
    {
        // Delay the win check
        delayBeforeWinCheck -= Time.deltaTime;
    
        if (delayBeforeWinCheck <= 0f && enemyDetectorArea.GetCurrentEnemy() == 0)
        {
            SetGameState(GameState.Clear);
        }
    }

    private void CheckPlayerHealth()
    {
        HealthSystem playerHealthSystem = player.GetComponent<HealthSystem>();

        if (playerHealthSystem.GetCurrentHealth() == 0)
        {
            // Countdown when player's health is zero
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                SetGameState(GameState.GameOver);
                ParticleSystem playerDieFx = LeanPool.Spawn(playerHealthSystem.fx_die, Vector3.up + player.transform.position, Quaternion.identity);
                LeanPool.Despawn(playerDieFx, 3f);
            }
        }
        else
        {
            // Reset countdown when player's health is not zero
            ResetCountdownTimer();
        }
    }

    private void ResetCountdownTimer()
    {
        countdownTimer = CountdownDuration;
    }
    
    private void ShowEffect(GameObject effectPrefab)
    {
        if (effectPrefab != null)
        {
            // Your code to show the effect (e.g., activating VFXclear or VFXgameover)
            effectPrefab.SetActive(true);
        }
    }
}
