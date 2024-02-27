using UnityEngine;

public class SamuraiMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float stopDuration = 5.0f;

    [SerializeField] private Vector3 startPoint = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 endPoint = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 target;
    private bool isMoving = true;

    public bool canOpenKatana = false;
    public GameObject katana;

    private void Start()
    {
        target = endPoint; // Start from the end point
    }

    private void Update()
    {
        if (isMoving)
        {
            // Move the Samurai towards the target (Modified to move along the z-axis)
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, target.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);

            // When the Samurai reaches the target, stop
            if (transform.position == newPosition)
            {
                // Stop moving for 2 seconds
                isMoving = false;
                OpenKatana();
                StartCoroutine(WaitAndRotate());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndRotate()
    {
        // Wait for stopDuration seconds
        yield return new WaitForSeconds(stopDuration);

        // Change target
        if (target == startPoint)
            target = endPoint;
        else
            target = startPoint;

        // Rotate 180 degrees when moving back to the start or end point
        transform.Rotate(Vector3.up, 180.0f);

        // Reset to start a new movement
        isMoving = true;

        // Close the Katana when starting to move again
        CloseKatana();
    }

    public void OpenKatana()
    {
        canOpenKatana = true;
        katana.SetActive(canOpenKatana);
    }

    public void CloseKatana()
    {
        canOpenKatana = false;
        katana.SetActive(canOpenKatana);
    }
}
