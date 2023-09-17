using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Function to change the scene
    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}