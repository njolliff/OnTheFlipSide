using UnityEngine;

public class SceneManager : MonoBehaviour
{
    // Initialize as singleton instance in DontDestroyOnLoad
    public static SceneManager instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    void OnDisable()
    {
        if (instance == this)
            instance = null;
    }

    public void LoadScene(string sceneName)
    {
        // Load the specified scene by name
        if (sceneName != null)
        {
            Debug.LogError("Scene name not provided.");
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
