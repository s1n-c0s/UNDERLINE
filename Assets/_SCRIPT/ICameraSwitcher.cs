using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import the UI namespace
using Cinemachine;

public class ICameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera PlayerfollowCamera;
    public List<GameObject> cameras; // List of cameras to switch between
    private int currentCameraIndex = 0;

    // Reference to the UI button
    public Button switchCameraButton;
    
    private void Start()
    {
        // Attach the method to be called when the button is clicked
        switchCameraButton.onClick.AddListener(SwitchCamera);

        findPlayer(GameObject.FindGameObjectWithTag("Player"));

        // Load the selected camera index from PlayerPrefs, default to 0 if not set
        currentCameraIndex = PlayerPrefs.GetInt("SelectedCameraIndex", 0);

        // Disable all cameras except the saved one
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].SetActive(i == currentCameraIndex);
        }
    }

    private void findPlayer(GameObject player)
    {
        PlayerfollowCamera.Follow = player.transform;
    }

    public void SwitchCamera()
    {
        // Disable the current camera
        cameras[currentCameraIndex].SetActive(false);

        // Increment the camera index, and wrap around if necessary
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;

        // Enable the new current camera
        cameras[currentCameraIndex].SetActive(true);

        // Save the selected camera index to PlayerPrefs
        PlayerPrefs.SetInt("SelectedCameraIndex", currentCameraIndex);
        PlayerPrefs.Save(); // Save the PlayerPrefs data
    }
}