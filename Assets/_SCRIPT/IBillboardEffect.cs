using UnityEngine;

public class IBillboardEffect : MonoBehaviour
{   
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
        /*transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);*/
    }
}