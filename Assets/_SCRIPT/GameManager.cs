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
        MainMenu,
        Playing,
        Clear,
        GameOver
        // Add more states as needed
    }

    public GameState CurrentGameState { get; private set; }

    private float countdownTimer;
    private const float CountdownDuration = 3f;

    private float clearCheckInterval = 1f;
    private float clearCheckTimer;

    private void Awake()
    {
        //InitializeSingleton();
        SetGameState(GameState.MainMenu);
    }

    private void Start()
    {
        if (isPlaying)
        {
            StartGame();
        }
    }

    private void LateUpdate()
    {
        if (isPlaying && CurrentGameState == GameState.Playing)
        {
            isKillAllEnemy();
            CheckPlayerHealth();
        }
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
        ResetCountdownTimer();
        ResetClearCheckTimer();
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
            case GameState.Clear:
                Debug.Log("Game Clear");
                ShowEffect(VFXclear);
                break;

            case GameState.GameOver:
                Debug.Log("Game Over");
                ShowEffect(VFXgameover);
                break;
        }
    }

    private void isKillAllEnemy()
    {
        if (enemyDetectorArea.GetCurrentEnemy() == 0)
        {
            SetGameState(GameState.Clear);
            ResetClearCheckTimer();
        }
        else
        {
            // Check for clear state at specified intervals
            clearCheckTimer -= Time.deltaTime;
            if (clearCheckTimer <= 0f)
            {
                ResetClearCheckTimer();
            }
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

    private void ResetClearCheckTimer()
    {
        clearCheckTimer = clearCheckInterval;
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
