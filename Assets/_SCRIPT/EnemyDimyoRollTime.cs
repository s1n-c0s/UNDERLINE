using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDimyoRollTime : MonoBehaviour
{
    public float rotationAngle = 90f; // องศาที่จะหมุน
    public float delayBetweenRotations = 1f; // ระยะเวลาหยุดหมุน

    private bool isRotating = false;

    private void Start()
    {
        StartCoroutine(RotateRepeatedly());
    }

    IEnumerator RotateRepeatedly()
    {
        while (true)
        {
            if (!isRotating)
            {
                isRotating = true;
                float currentRotation = 0f;

                while (currentRotation < rotationAngle)
                {
                    float rotationAmount = Mathf.Min(rotationAngle - currentRotation, Time.deltaTime * rotationAngle);
                    transform.Rotate(Vector3.up, rotationAmount);
                    currentRotation += rotationAmount;

                    yield return null;
                }

                yield return new WaitForSeconds(delayBetweenRotations);
                isRotating = false;
            }

            yield return null;
        }
    }
}
