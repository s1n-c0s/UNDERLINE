using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class CinemachineZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float zoomOutFOV = 40f; // Target FOV for zoom out
    public float NormalFOV = 50f;
    public float zoomSpeed = 5f;
    public float rotationSpeed = 10f; // Speed of rotation

    private Quaternion initialRotation;

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.m_Lens.FieldOfView = NormalFOV;
        initialRotation = virtualCamera.transform.localRotation; // Store the initial rotation
    }

    private void OnEnable()
    {
        GameManager.OnGameEnd += HandleGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnd -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        StartCoroutine(ZoomInAndRotate());
    }

    private IEnumerator ZoomInAndRotate()
    {
        while (virtualCamera.m_Lens.FieldOfView > zoomOutFOV)
        {
            // Zoom in
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, zoomOutFOV, Time.deltaTime * zoomSpeed);

            // Rotate right
            float rotationAmount = rotationSpeed * Time.deltaTime;
            virtualCamera.transform.localRotation = Quaternion.Euler(
                virtualCamera.transform.localEulerAngles.x,
                virtualCamera.transform.localEulerAngles.y + rotationAmount,
                virtualCamera.transform.localEulerAngles.z
            );

            yield return null;
        }

        // Ensure final FOV and rotation are set
        virtualCamera.m_Lens.FieldOfView = zoomOutFOV;
        virtualCamera.transform.localRotation = Quaternion.Euler(
            virtualCamera.transform.localEulerAngles.x,
            virtualCamera.transform.localEulerAngles.y,
            virtualCamera.transform.localEulerAngles.z
        );
    }
}