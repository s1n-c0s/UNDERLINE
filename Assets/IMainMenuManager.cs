using System.Collections;
using UnityEngine;
using DG.Tweening;

public class IMainMenuManager : MonoBehaviour
{
    private int currentPanelIndex = 0;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private CanvasGroup[] canvasGroups;

    private void Start()
    {
        // Call OpenPanel to open the first panel (you can customize this based on your requirements)
        OpenPanel(0);
    }

    public void OpenPanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= canvasGroups.Length || panelIndex == currentPanelIndex)
        {
            //Debug.LogWarning("Invalid panel index or already open");
            return;
        }

        // Close the current panel
        CanvasGroup currentPanel = canvasGroups[currentPanelIndex];
        currentPanel.DOFade(0f, fadeTime).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() => currentPanel.gameObject.SetActive(false));

        // Open the selected panel
        CanvasGroup selectedPanel = canvasGroups[panelIndex];
        selectedPanel.gameObject.SetActive(true);
        selectedPanel.alpha = 0f;
        selectedPanel.DOFade(1f, fadeTime).SetEase(Ease.InQuad).SetUpdate(true);

        // Update the current panel index
        currentPanelIndex = panelIndex;
    }
}