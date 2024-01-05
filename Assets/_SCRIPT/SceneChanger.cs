using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Function to change the scene
    public void ChangeToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void ChangeToScene(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    public void Nextlevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}