using System.Collections;
using UnityEngine;

public class enemyDimyoRollTime : MonoBehaviour
{
    public float rotationAngle = 90f; // Degrees to rotate
    public float delayBetweenRotations = 1f; // Delay between rotations

    private bool isRotating = false;

    private void OnEnable()
    {
        StartCoroutine(RotateRepeatedly());
    }

    private void OnDisable()
    {
        StopCoroutine(RotateRepeatedly());
        isRotating = false;
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