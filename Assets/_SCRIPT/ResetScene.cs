using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    void Update()
    {
        // Check if the "R" key is pressed.
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the current scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}