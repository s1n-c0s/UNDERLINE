using System.Collections;
using UnityEngine;

public class shaderFadeWall : MonoBehaviour
{
    [SerializeField] private Material scaleMaterial;
    [SerializeField] private float targetScale = 0.35f;
    [SerializeField] private float scaleDuration = 1.0f;
    [SerializeField] private AnimationCurve fadeCurve;

    private float initialScale = 1.0f;
    private float currentScale;
    private bool isScaling = false;

    void OnEnable()
    {
        currentScale = initialScale;
        isScaling = true;
        StartCoroutine(ScaleOverTime());
    }

    private IEnumerator ScaleOverTime()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / scaleDuration);
            currentScale = Mathf.Lerp(initialScale, targetScale, fadeCurve.Evaluate(normalizedTime));
            scaleMaterial.SetFloat("_FadeScale", currentScale);
            yield return null;
        }

        // Ensure final scale is set correctly
        scaleMaterial.SetFloat("_FadeScale", targetScale);
        isScaling = false;
    }

    public void ResetScale()
    {
        StopAllCoroutines(); // Stop any ongoing scaling coroutine
        currentScale = initialScale;
        scaleMaterial.SetFloat("_FadeScale", initialScale);
        isScaling = false;
    }
}