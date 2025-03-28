using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    // PUBLIC
    public string nextSceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if player is alive
            if (PlayerLogic.instance.isAlive)
            {
                // Load the next scene
                SceneManager.instance.LoadScene(nextSceneName);
            }
        }
    }
}