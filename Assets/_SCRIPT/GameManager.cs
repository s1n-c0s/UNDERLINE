using DG.Tweening;
using UnityEngine;
using Lean.Pool;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameclearFX;
    [SerializeField] private CanvasGroup _gameclearUI; 
    
    public GameObject gameoverFX;
    [SerializeField] private CanvasGroup _gameoverUI;

    public GameObject player;
    public EnemyDetectorArea enemyDetectorArea;
    public bool isPlaying;
    
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private CanvasGroup _introPanel;
    [SerializeField] private CanvasGroup _ingamePanel;
    [SerializeField] private GameObject audioSystem;
    
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
    
    private void Awake()
    {
        GameObject GameplayAudio = Instantiate(audioSystem);
        GameplayAudio.name = "GameplayAudio";
        
        _levelNumber.text = SceneManager.GetActiveScene().buildIndex.ToString();
    }
    private void Start()
    {
        Clear_UI();
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
                //Debug.Log("Playing");
                isPlaying = true;
                break;
            case GameState.Clear:
                //Debug.Log("Game Clear");
                //Show(VFXclear);
                UI_gameEnd(_gameclearUI);
                isPlaying = false;
                break;

            case GameState.GameOver:
                //Debug.Log("Game Over");
                //Show(VFXgameover);
                UI_gameEnd(_gameoverUI);
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
        _introPanel.gameObject.SetActive(true);
        _introPanel.DOFade(1, 1f)
            .OnComplete(() => _introPanel.DOFade(0f, 1f)
                .OnComplete(() => _introPanel.gameObject.SetActive(false)));
        _ingamePanel.DOFade(1f, 2.5f);
    }
    
    private void UI_gameEnd(CanvasGroup ui)
    {
        ui.gameObject.SetActive(true);
        ui.DOFade(1, 0.5f);
    }
    
    private void Clear_UI()
    {
        _gameclearUI.gameObject.SetActive(false);
        _gameoverUI.gameObject.SetActive(false);
    }
}
