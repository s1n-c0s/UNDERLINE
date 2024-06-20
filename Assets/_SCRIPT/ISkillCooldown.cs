using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ISkillCooldown : MonoBehaviour
{
    [SerializeField] private SkillTurnSystem _skillTurnSystem; // Reference to SkillTurnSystem
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component

    private bool isTweening = false; // Flag to track if a tween animation is currently playing

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        UpdateText(); // Initialize text on start
    }

    private void Update()
    {
        int currentTurn = _skillTurnSystem.GetCurrentValue();

        if (textMeshPro.text != currentTurn.ToString())
        {
            UpdateTextWithTween(currentTurn.ToString());
        }
    }

    private void UpdateTextWithTween(string newText)
    {
        // Avoid starting a new tween if one is already in progress
        if (isTweening)
            return;

        isTweening = true;

        // Punch scale effect
        textMeshPro.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 0, 1f)
            .OnComplete(() =>
            {
                // Update text
                textMeshPro.text = newText;

                // Scale back to original size
                textMeshPro.transform.DOScale(Vector3.one, 0.3f)
                    .OnComplete(() => isTweening = false); // Reset tweening flag after complete
            });
    }

    // Directly update text without tween
    private void UpdateText()
    {
        textMeshPro.text = _skillTurnSystem.GetCurrentValue().ToString();
    }
}