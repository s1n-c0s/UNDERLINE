using DG.Tweening;
using UnityEngine;
using Lean.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject VFXclear;
    public GameObject VFXgameover;

    public GameObject player;
    public EnemyDetectorArea enemyDetectorArea;
    public bool isPlaying;

    [SerializeField] private CanvasGroup _introPanel;
    [SerializeField] private CanvasGroup _ingamePanel;
        
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
        UI_introFade();
        /*_introPanel.DOFade(1, 1f).OnComplete(() => _introPanel.DOFade(0f,1f).OnComplete(() 
            => _introPanel.gameObject.SetActive(false)));*/
        ResetCountdownTimer();
    }

    private void LateUpdate()
    {
        if (isPlaying && CurrentGameState == GameState.Playing)
        {
            isKillAllEnemy();
            CheckPlayerHealth();
        }
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
                //Show(VFXclear);
                isPlaying = false;
                break;

            case GameState.GameOver:
                Debug.Log("Game Over");
                //Show(VFXgameover);
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
    
    private void Show(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    
    private void UI_introFade()
    {
        _introPanel.DOFade(1, 1f)
            .OnComplete(() => _introPanel.DOFade(0f, 1f)
                .OnComplete(() => _introPanel.gameObject.SetActive(false)));
        _ingamePanel.DOFade(1f, 1f);
    }
}
