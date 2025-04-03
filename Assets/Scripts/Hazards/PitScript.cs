using UnityEngine;

public class PitScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && EventManager.instance != null)
            EventManager.instance.KillPlayer();
    }
}