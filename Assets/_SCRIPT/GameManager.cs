using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Lean.Pool;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameclearFX;
    [SerializeField] private CanvasGroup _gameclearUI; 
    
    public GameObject gameoverFX;
    [SerializeField] private CanvasGroup _gameoverUI;

    public GameObject player;
    [SerializeField] private List<ZoneManager> Zones;
    
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
        ResetCountdownTimer();

        Zones.AddRange(FindObjectsOfType<ZoneManager>());

        // Subscribe to zone clear events
        foreach (ZoneManager zone in Zones)
        {
            zone.OnZoneClear += HandleZoneClear;
        }
    }

    private void LateUpdate()
    {
        if (isPlaying && CurrentGameState == GameState.Playing)
        {
            CheckPlayerHealth();
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
                UI_gameEnd(_gameclearUI);
                isPlaying = false;
                break;

            case GameState.GameOver:
                UI_gameEnd(_gameoverUI);
                isPlaying = false;
                break;
        }
    }

    private void HandleZoneClear(ZoneManager zone)
    {
        // Check if all zones are clear
        if (AllZonesClear())
        {
            SetGameState(GameState.Clear);
            Debug.Log(CurrentGameState);
        }
    }

    private bool AllZonesClear()
    {
        foreach (ZoneManager zone in Zones)
        {
            if (!zone.isClear)
            {
                return false;
            }
        }
        return true;
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
                player.SetActive(false);
                Instantiate(playerHealthSystem.fx_die, player.transform.position, Quaternion.identity);
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
