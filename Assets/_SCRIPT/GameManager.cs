using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
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
    [SerializeField] private List<JailObj> _jailObjs;

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

    private void OnDestroy()
    {
        Cleanup();
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
        Zones.Sort((z1, z2) => z1.zoneOrder.CompareTo(z2.zoneOrder)); // Ensure zones are sorted by order

        foreach (ZoneManager zone in Zones)
        {
            zone.OnZoneClear += HandleZoneClear;
            zone.ActivateEnemies(false); // Ensure enemies are inactive initially
        }

        // Activate the first zone's enemies
        if (Zones.Count > 0)
        {
            Zones[0].ActivateEnemies(true);
        }

        _jailObjs.AddRange(FindObjectsOfType<JailObj>()); // Find all JailObj instances

        // Subscribe to OnJailUnlock event for each jail
        foreach (var jail in _jailObjs)
        {
            jail.OnJailUnlock += HandleJailUnlock;
        }
    }

    private void Cleanup()
    {
        foreach (var jail in _jailObjs)
        {
            jail.OnJailUnlock -= HandleJailUnlock;
        }

        foreach (var zone in Zones)
        {
            zone.OnZoneClear -= HandleZoneClear;
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
        // Find the index of the cleared zone
        int clearedZoneIndex = Zones.IndexOf(zone);
        if (clearedZoneIndex != -1 && clearedZoneIndex < Zones.Count - 1)
        {
            // Activate enemies in the next zone
            Zones[clearedZoneIndex + 1].ActivateEnemies(true);
        }

        CheckGameProgress();
    }

    private void HandleJailUnlock(JailObj jail)
    {
        CheckGameProgress();
    }

    private void CheckGameProgress()
    {
        if (AllZonesClear() && AllJailsUnlocked())
        {
            portal.SetActive(true);
        }
    }

    private bool AllZonesClear()
    {
        return Zones.TrueForAll(zone => zone.isClear);
    }

    private bool AllJailsUnlocked()
    {
        return _jailObjs.TrueForAll(jail => !jail.IsLocked);
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
                playerHealthSystem.Die();
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
