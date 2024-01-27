using TMPro;
using UnityEngine;
using DG.Tweening;

public class ICombo : MonoBehaviour
{
    public static ICombo Instance { get; private set; }

    public GameObject comboPanel;
    public TextMeshProUGUI comboText;

    private int hitcombo = 0;
    private float timer = 0f;
    public float comboResetTime = 2f;
    public float fadeDuration = 0.5f;

    private CanvasGroup comboCanvasGroup;

    private void Awake()
    {
        Instance = this;
        comboCanvasGroup = comboPanel.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        HideComboPanel();
    }

    private void LateUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= comboResetTime)
        {
            ResetCombo();
        }
    }

    public void IncreaseCombo()
    {
        comboText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 10, 1f);
        hitcombo++;
        comboText.text = hitcombo.ToString();
        timer = 0f;

        // Only fade in if the combo panel is not already active
        if (!comboPanel.activeSelf)
        {
            FadeInComboPanel();
        }
    }

    public void ResetCombo()
    {
        FadeOutComboPanel();
        timer = 0f;
    }

    private void FadeInComboPanel()
    {
        comboPanel.SetActive(true);
        comboCanvasGroup.DOFade(1f, fadeDuration);
    }

    private void FadeOutComboPanel()
    {
        // Only initiate fade-out if the combo panel is currently active
        if (comboPanel.activeSelf)
        {
            comboCanvasGroup.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                comboPanel.SetActive(false);
                hitcombo = 0;
                comboText.text = hitcombo.ToString();
            });
        }
    }

    private void HideComboPanel()
    {
        comboPanel.SetActive(false);
    }
}