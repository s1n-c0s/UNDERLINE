 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    float xRot, yRot = 0f;

    public Rigidbody ball;

    public float rotationSpeed = 5f;

    public float shootPower = 30f;

    public LineRenderer line;

    private void Update()
    {
        transform.position = ball.position;
        
        if (Input.GetMouseButton(0))
        {
            xRot += Input.GetAxis("Mouse X")*rotationSpeed;
            yRot += Input.GetAxis("Mouse Y")*rotationSpeed;
            if (yRot< -35f)
            {
                yRot = -35f;
            }
            transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
            line.gameObject.SetActive(true);
            line.SetPosition(0,transform.position);
            line.SetPosition(1, transform.position + transform.forward * 4f);
            ;

        }
        if (Input.GetMouseButtonUp(0)) { 
            ball.velocity=transform.forward*shootPower;
            line.gameObject.SetActive(false);
        }
    }
}
