using DG.Tweening;
using UnityEngine;
using Lean.Pool;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPlaying;
    public GameObject player;
    public EnemyDetectorArea enemyDetectorArea;
    
    [Header("Ingame Ui")]
    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private CanvasGroup _introPanel;
    [SerializeField] private CanvasGroup _ingamePanel;

    [Header("Audio System")]
    [SerializeField] private bool isMuted;
    [SerializeField] private GameObject BG_audio;
    [SerializeField] private GameObject SFX_audio;
    
    [Header("Endgame Setting")]
    public GameObject gameclearFX;
    [SerializeField] private CanvasGroup _gameclearUI;
    [Space(10)]
    public GameObject gameoverFX;
    [SerializeField] private CanvasGroup _gameoverUI;

    public enum GameState
    {
        Playing,
        Clear,
        GameOver
    }

    public GameState CurrentGameState { get; private set; }

    private float countdownTimer;
    private const float CountdownDuration = 2f;
    private float delayBeforeWinCheck = 2f;

    private void Awake()
    {
        // Instantiate the audio system
        GameObject BG_Audio = Instantiate(BG_audio);
        GameObject SFX_Audio = Instantiate(SFX_audio);

        _levelNumber.text = SceneManager.GetActiveScene().buildIndex.ToString();

        // Load mute preference
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
    }

    private void Start()
    {
        Clear_UI();
        CurrentGameState = GameState.Playing;
        UI_introFade();
        ResetCountdownTimer();

        // Apply the mute setting
        ApplyMute();
    }

    private void LateUpdate()
    {
        if (isPlaying && CurrentGameState == GameState.Playing)
        {
            isKillAllEnemy();
            CheckPlayerHealth();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMute();
        }
    }

    public void SetGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;

        switch (newGameState)
        {
            case GameState.Playing:
                isPlaying = true;
                break;
            case GameState.Clear:
                //Show(VFXclear);
                UI_gameEnd(_gameclearUI);
                isPlaying = false;
                break;
            case GameState.GameOver:
                //Show(VFXgameover);
                UI_gameEnd(_gameoverUI);
                isPlaying = false;
                break;
        }
    }

    private void isKillAllEnemy()
    {
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
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0f)
            {
                SetGameState(GameState.GameOver);
                player.SetActive(false);
                Instantiate(playerHealthSystem.fx_die, player.transform.position, Quaternion.identity);
            }
        }
        else
        {
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

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        ApplyMute();
    }

    private void ApplyMute()
    {
        SFX_Manager.Instance.ToggleMute(isMuted);
        BGAudioManager.Instance.ToggleMute(isMuted);
    }
}
