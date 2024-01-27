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
        playBT = GetComponent<Button>();
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
            playBT.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 10, 1f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    isTweening = false;
                });
        }
    }
}