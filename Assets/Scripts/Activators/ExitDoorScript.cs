using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorScript : MonoBehaviour
{
    public int level;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if player is alive
            if (PlayerLogic.instance.isAlive)
            {
                // Mark level as complete and unlock next level
                LevelManager.instance.CompleteLevel(level);
                LevelManager.instance.UnlockLevel(level + 1);

                // Reset player spawner
                EventManager.instance.SetSpawner(0);

                // Load the next scene
                SceneManager.LoadScene("Level Select");
            }
        }
    }
}