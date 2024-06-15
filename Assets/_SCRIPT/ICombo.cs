using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class ICombo : MonoBehaviour
{
    public static ICombo Instance { get; private set; }

    public GameObject comboPanel;
    public TextMeshProUGUI comboText;

    public int hitcombo = 0;
    private float comboTimer = 0f;
    public float comboResetTime = 2f;
    public float fadeDuration = 0.5f;

    private CanvasGroup comboCanvasGroup;

    private bool isInPanicMode = false;

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
        // Update combo timer
        if (!isInPanicMode)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer >= comboResetTime)
            {
                ResetCombo();
            }
        }
    }

    public void IncreaseCombo()
    {
        hitcombo++;
        comboText.text = hitcombo.ToString();
        comboTimer = 0f; // Reset combo timer on combo increase

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
            // Fade out combo panel first, then reset hitcombo
            FadeOutComboPanel(() =>
            {
                hitcombo = 0;
                comboText.text = hitcombo.ToString();
            });
        }
        else
        {
            // Directly reset hitcombo if in panic mode
            hitcombo = 0;
            comboText.text = hitcombo.ToString();
        }
    }

    private void FadeInComboPanel()
    {
        comboPanel.SetActive(true);
        comboCanvasGroup.alpha = 0f; // Ensure alpha is 0 before fade-in
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
}
