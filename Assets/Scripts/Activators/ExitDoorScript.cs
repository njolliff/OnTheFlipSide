using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if player is alive
            if (PlayerLogic.instance.isAlive)
            {
                // Load the next scene
                SceneManager.instance.LoadScene("Level Select");
            }
        }
    }
}