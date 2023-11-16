using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ICloudAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] _cloudLists;
    private float minMovementDistance = 100f;
    private float maxMovementDistance = 500f;
    private float minMovementDuration = 1f;
    private float maxMovementDuration = 3f;

    private void Start()
    {
        // Call the method to start cloud movement
        StartCloudMovement();
    }

    private void StartCloudMovement()
    {
        foreach (Transform cloudTransform in _cloudLists)
        {
            // Randomize movement distance and duration for each cloud
            float randomDistance = Random.Range(minMovementDistance, maxMovementDistance);
            float randomDuration = Random.Range(minMovementDuration, maxMovementDuration);

            // Randomly choose between moving left or right
            int randomDirection = Random.Range(0, 2);
            float targetX = randomDirection == 0 ? cloudTransform.position.x - randomDistance : cloudTransform.position.x + randomDistance;

            // Move the cloud to the random position and back to the original position
            cloudTransform.DOMoveX(targetX, randomDuration)
                .SetLoops(-1, LoopType.Yoyo) // Loop the movement back and forth
                .SetEase(Ease.Linear); // Use linear easing for constant speed
        }
    }
}