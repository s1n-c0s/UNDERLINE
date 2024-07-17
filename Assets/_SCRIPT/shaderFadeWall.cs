using System.Collections;
using UnityEngine;

public class shaderFadeWall : MonoBehaviour
{
    [SerializeField] private Material scaleMaterial;
    [SerializeField] private float targetScale = 0.35f;
    [SerializeField] private float fadeInDuration = 1.0f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeCurve;

    private float initialScale = 1.0f;
    private float currentScale;
    private bool isScaling = false;
    private bool hasFadedIn = false;

    public delegate void FadeCompleteHandler(shaderFadeWall sender);
    public event FadeCompleteHandler OnFadeOutComplete;

    void OnEnable()
    {
        if (!hasFadedIn)
        {
            StartFadeIn();
        }
    }

    public void StartFadeIn()
    {
        if (!hasFadedIn)
        {
            currentScale = initialScale;
            isScaling = true;
            StartCoroutine(FadeOverTime(initialScale, targetScale, fadeInDuration, false));
            hasFadedIn = true;
        }
    }

    public void StartFadeOut()
    {
        currentScale = targetScale;
        isScaling = true;
        StartCoroutine(FadeOverTime(targetScale, initialScale, fadeOutDuration, true));
    }

    private IEnumerator FadeOverTime(float startScale, float endScale, float duration, bool isFadeOut)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            currentScale = Mathf.Lerp(startScale, endScale, fadeCurve.Evaluate(normalizedTime));
            scaleMaterial.SetFloat("_FadeScale", currentScale);
            yield return null;
        }

        // Ensure final scale is set correctly
        scaleMaterial.SetFloat("_FadeScale", endScale);
        isScaling = false;

        if (isFadeOut)
        {
            OnFadeOutComplete?.Invoke(this);
        }
    }

    public void ResetScale()
    {
        StopAllCoroutines(); // Stop any ongoing scaling coroutine
        currentScale = initialScale;
        scaleMaterial.SetFloat("_FadeScale", initialScale);
        isScaling = false;
        hasFadedIn = false;
    }
}
