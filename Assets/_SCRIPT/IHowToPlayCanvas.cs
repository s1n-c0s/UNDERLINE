using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class IHowToPlayCanvas : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public RectTransform fingerUI;
    public float countdownDuration = 3f;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (fingerUI != null)
        {
            // Move fingerUI in the local Y-axis using DOTween
            MoveFingerUI();
        }
    }

    private void MoveFingerUI()
    {
        fingerUI.DOLocalMoveY(0f, 1f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        // Update the countdown timer and clamp it to zero
        float countdownTimer = Mathf.Max(0f, countdownDuration -= Time.deltaTime);

        // Check for touch input or countdown reaching zero
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || countdownTimer == 0f)
        {
            HideCanvasWithFade();
        }
    }

    private void HideCanvasWithFade()
    {
        // Use null conditional operator to ensure the CanvasGroup component is not null
        canvasGroup?.DOFade(0f, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }
}