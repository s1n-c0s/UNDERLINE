using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro; // Reference to TextMeshProUGUI component
    private bool isTweening = false; // Flag to track if a tween animation is currently playing

    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Method to update text with scale effect
    public void UpdateTextWithScaleEffect(string newText)
    {
        // Avoid starting a new tween if one is already in progress
        if (isTweening)
            return;

        isTweening = true;

        // Punch scale effect
        textMeshPro.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 0, 1f).OnComplete(() =>
        {
            // Update text
            textMeshPro.text = newText;

            // Scale back to original size
            textMeshPro.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                isTweening = false;
            });
        });
    }
}