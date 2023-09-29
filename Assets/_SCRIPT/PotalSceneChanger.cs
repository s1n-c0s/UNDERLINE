using UnityEngine;
using UnityEngine.SceneManagement;

public class PotalSceneChanger : MonoBehaviour
{
    // Set the name of the scene you want to load when the player enters the trigger.
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player (you can customize the tag or layer).
        if (other.CompareTag("Player"))
        {
            // Load the specified scene.
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
