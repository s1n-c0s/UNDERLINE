using System.Collections.Generic;
using UnityEngine;

public class IPanelManager : MonoBehaviour
{
    public List<GameObject> panels = new List<GameObject>();
    private Stack<GameObject> panelStack = new Stack<GameObject>();

    private void Start()
    {
        // Deactivate all panels initially
        foreach (var panel in panels)
        {
            panel.SetActive(false);
        }

        // Activate the first panel
        if (panels.Count > 0)
        {
            panels[0].SetActive(true);
            panelStack.Push(panels[0]);
        }
    }

    private void Update()
    {
        // Check for the "Esc" key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void ActivatePanel(GameObject panel)
    {
        // Deactivate the current panel
        if (panelStack.Count > 0)
        {
            var currentPanel = panelStack.Peek();
            currentPanel.SetActive(false);
        }

        // Activate the new panel
        panel.SetActive(true);
        panelStack.Push(panel);
    }

    public void GoBack()
    {
        if (panelStack.Count > 1)
        {
            // Deactivate the current panel
            var currentPanel = panelStack.Pop();
            currentPanel.SetActive(false);

            // Activate the previous panel
            var previousPanel = panelStack.Peek();
            previousPanel.SetActive(true);
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit game");
    }
}