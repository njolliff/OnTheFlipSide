using UnityEngine;

public class BoxScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private bool isGrounded;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the box is touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        // Allow box to move horizontally when touching the player on the ground
        if (collision.gameObject.CompareTag("Player") && isGrounded)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the box is no longer touching the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }

        // Freeze horizontal box movement when not touching the player
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
    }
}