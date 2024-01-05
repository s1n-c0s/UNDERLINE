using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICameraSwitcher : MonoBehaviour
{
    [SerializeField] private Cinemachine.CinemachineVirtualCamera PlayerfollowCamera;
    public List<GameObject> cameras; // List of cameras to switch between
    private int currentCameraIndex = 0;

    private void Start()
    {
        findPlayer(GameObject.FindGameObjectWithTag("Player"));
        // Disable all cameras except the initial one
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].gameObject.SetActive(i == 0);
        }
    }

    private void Update()
    {
        // Check for input to switch cameras
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera();
        }
    }
    
    private void findPlayer(GameObject player)
    {
        PlayerfollowCamera.Follow = player.transform;
    }

    public void SwitchCamera()
    {
        // Disable the current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Increment the camera index, and wrap around if necessary
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;

        // Enable the new current camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}