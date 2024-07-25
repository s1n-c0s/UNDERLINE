using UnityEngine;
using UnityEngine.UI;

public class ToggleMuteButton : MonoBehaviour
{
    public Button toggleButton;

    void Start()
    {
        if (toggleButton == null)
        {
            toggleButton = GetComponent<Button>();
        }

        toggleButton.onClick.AddListener(ToggleMute);
        
    }

    void ToggleMute()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMute();
        }
    }
}