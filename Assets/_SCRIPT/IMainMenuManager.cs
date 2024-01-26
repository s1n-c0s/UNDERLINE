using System.Collections;
using UnityEngine;
using DG.Tweening;

public class IMainMenuManager : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.5f;
    [SerializeField] private CanvasGroup[] canvasGroups;

    private int currentPanelIndex = 0;

    private void Start()
    {
        OpenPanel(0);
    }

    public void OpenPanel(int panelIndex)
    {
        if (!IsValidPanelIndex(panelIndex) || panelIndex == currentPanelIndex)
        {
            return;
        }

        CloseCurrentPanel();
        OpenSelectedPanel(panelIndex);
    }

    private bool IsValidPanelIndex(int panelIndex)
    {
        return panelIndex >= 0 && panelIndex < canvasGroups.Length;
    }

    private void CloseCurrentPanel()
    {
        CanvasGroup currentPanel = canvasGroups[currentPanelIndex];
        currentPanel.DOFade(0f, fadeTime).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() => SetPanelActive(currentPanel, false));
    }

    private void OpenSelectedPanel(int panelIndex)
    {
        CanvasGroup selectedPanel = canvasGroups[panelIndex];
        SetPanelActive(selectedPanel, true);
        selectedPanel.alpha = 0f;
        selectedPanel.DOFade(1f, fadeTime).SetEase(Ease.InQuad).SetUpdate(true);

        currentPanelIndex = panelIndex;
    }

    private void SetPanelActive(CanvasGroup panel, bool active)
    {
        panel.gameObject.SetActive(active);
    }
}