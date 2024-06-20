using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class ICombo : MonoBehaviour
{
    public static ICombo Instance { get; private set; }

    public GameObject comboPanel;
    public TextMeshProUGUI comboText;

    private CanvasGroup comboCanvasGroup;
    private int hitcombo = 0;
    private float comboTimer = 0f;
    private float fadeDuration = 0.5f;

    private bool isInPanicMode = false;
    private float panicDuration = 8f;
    private float panicExtendDuration = 2f;

    private void Awake()
    {
        Instance = this;
        comboCanvasGroup = comboPanel.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        HideComboPanel();
    }

    private void Update()
    {
        if (!isInPanicMode)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= panicDuration)
            {
                // Reset combo and fade out combo panel when panic duration ends
                FadeOutComboPanel(() =>
                {
                    ResetCombo();
                });
            }
        }
    }

    public void IncreaseCombo()
    {
        hitcombo++;
        comboText.text = hitcombo.ToString();
        comboTimer = 0f;

        comboText.transform.DOKill();
        comboText.transform.localScale = Vector3.one;
        comboText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 10, 1f).SetUpdate(true);

        if (!comboPanel.activeSelf && !isInPanicMode)
        {
            FadeInComboPanel();
        }
    }

    public void ResetCombo()
    {
        if (!isInPanicMode)
        {
            FadeOutComboPanel(() =>
            {
                hitcombo = 0;
                comboText.text = hitcombo.ToString();
            });
        }
        else
        {
            hitcombo = 0;
            comboText.text = hitcombo.ToString();
        }
    }

    private void FadeInComboPanel()
    {
        comboPanel.SetActive(true);
        comboCanvasGroup.alpha = 0f;
        comboCanvasGroup.DOFade(1f, fadeDuration);
    }

    private void FadeOutComboPanel(Action onComplete = null)
    {
        if (comboPanel.activeSelf)
        {
            comboCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                comboPanel.SetActive(false);
                onComplete?.Invoke();
            });
        }
        else
        {
            onComplete?.Invoke();
        }
    }

    private void HideComboPanel()
    {
        comboPanel.SetActive(false);
    }

    // Update panic durations
    public void UpdatePanicDurations(float duration, float extendDuration)
    {
        panicDuration = duration;
        panicExtendDuration = extendDuration;
    }

    // Set panic mode
    public void SetPanicMode(bool panic)
    {
        isInPanicMode = panic;
        if (isInPanicMode)
        {
            comboTimer = 0f;
        }
        else
        {
            // If panic mode ends, reset combo and fade out combo panel
            ResetCombo();
        }
    }

    // Get current combo count
    public int GetComboCount()
    {
        return hitcombo;
    }
}
