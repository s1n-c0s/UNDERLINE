using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class IPunchBT : MonoBehaviour
{
    [SerializeField] private Button playBT;
    //[SerializeField] private String sceneName;
    //[SerializeField] private Image  mainMenu;

    private bool isTweening = false; // Flag to track if a tween is currently running

    void Start()
    {
        //playBT = gameObject.GetComponent<Button>();
        // Subscribe to the button click event
        playBT.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Check if a tween is currently running
        if (!isTweening)
        {
            // Set the flag to indicate that a tween is starting
            isTweening = true;

            // Use DOTween to apply punch scale effect to the button
            playBT.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 1f, 5, 1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // Optional: OnComplete callback for additional actions
                    // Do something when the punch scale animation is complete

                    // Reset the flag after the tween is complete
                    isTweening = false;
                    /*mainMenu.DOFade(0, 1);
                    LoadNextScene();*/
                });
        }
    }
    
    /*void LoadNextScene()
    {
        // Load your next scene
        SceneManager.LoadScene(sceneName);
    }*/
}