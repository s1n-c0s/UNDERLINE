using UnityEngine;

public class ShurikenMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float duration = 5f;
    private float elapsedTime;
    private float baseSpeed;

    public void SetSpeed(float newSpeed)
    {
        baseSpeed = newSpeed;
        elapsedTime = 0f;
    }

    private void Update()
    {
        // Increment elapsed time and calculate normalized time
        elapsedTime += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

        // Calculate the current speed based on the animation curve
        float currentSpeed = baseSpeed * speedCurve.Evaluate(normalizedTime);

        // Move the shuriken
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
    }
}