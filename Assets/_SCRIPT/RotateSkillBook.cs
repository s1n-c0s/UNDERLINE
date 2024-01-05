using UnityEngine;

public class RotateSkillBook : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust the speed as needed

    // Start is called before the first frame update
    void Start()
    {
        RotateObject(); // Call the RotateObject function when the script starts
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its Y-axis continuously
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void RotateObject()
    {
        // Generate a random angle for rotation around the Y-axis
        float randomAngle = Random.Range(0f, 360f);

        // Apply the initial rotation to the object
        transform.Rotate(new Vector3(0, randomAngle, 0));
    }
}