using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Kill player on contact
            PlayerLogic.instance.Die(); 
        }
    }
}