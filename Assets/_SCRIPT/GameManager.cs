using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
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
    [SerializeField] private List<ZoneManager> Zones;

    public bool isPlaying;

    [SerializeField] private TextMeshProUGUI _levelNumber;
    [SerializeField] private CanvasGroup _introPanel;
    [SerializeField] private CanvasGroup _ingamePanel;
    [SerializeField] private GameObject audioSystem;
    public GameObject portal;

    public enum GameState
    {
        Playing,
        Clear,
        GameOver
    }

    public GameState CurrentGameState { get; private set; }

    private float countdownTimer;
    private const float CountdownDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Instantiate(audioSystem).name = "GameplayAudio";
        _levelNumber.text = SceneManager.GetActiveScene().buildIndex.ToString();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void LateUpdate()
    {
        if (isPlaying && CurrentGameState == GameState.Playing)
        {
            CheckPlayerHealth();
        }
    }

    private void InitializeGame()
    {
        ClearUI();
        SetGameState(GameState.Playing);
        FadeUI(_introPanel, true, 1f, () =>
        {
            FadeUI(_introPanel, false, 1f);
            FadeUI(_ingamePanel, true, 2.5f);
        });

        ResetCountdownTimer();
        Zones.AddRange(FindObjectsOfType<ZoneManager>());

        foreach (var zone in Zones)
        {
            zone.OnZoneClear += HandleZoneClear;
        }
    }

    public void SetGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;
        isPlaying = newGameState == GameState.Playing;

        switch (newGameState)
        {
            case GameState.Clear:
                EndGame(_gameclearUI);
                break;
            case GameState.GameOver:
                EndGame(_gameoverUI);
                break;
        }
    }

    private void HandleZoneClear(ZoneManager zone)
    {
        if (AllZonesClear())
        {
            portal.SetActive(true);
        }
    }

    private bool AllZonesClear()
    {
        return Zones.TrueForAll(zone => zone.isClear);
    }

    private void CheckPlayerHealth()
    {
        var playerHealthSystem = player.GetComponent<HealthSystem>();

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

    private void ClearUI()
    {
        _gameclearUI.gameObject.SetActive(false);
        _gameoverUI.gameObject.SetActive(false);
    }

    private void FadeUI(CanvasGroup ui, bool fadeIn, float duration, TweenCallback onComplete = null)
    {
        ui.gameObject.SetActive(true);
        ui.DOFade(fadeIn ? 1 : 0, duration).OnComplete(() =>
        {
            if (!fadeIn)
            {
                ui.gameObject.SetActive(false);
            }
            onComplete?.Invoke();
        });
    }

    private void EndGame(CanvasGroup ui)
    {
        FadeUI(ui, true, 0.5f);
    }
}
