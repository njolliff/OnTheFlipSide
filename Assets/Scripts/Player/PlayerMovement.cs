using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // PUBLIC
    public float movementSpeed, jumpStrength;
    public Vector2 maxVelocity;

    // PRIVATE
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movementInput;
    private bool isGrounded;
    private float numJumps = 2;

    void Awake()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Flip player sprite
        if (rb.linearVelocityX > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (rb.linearVelocityX < 0)
            transform.localScale = new Vector3(1, 1, 1);

        // Play animations
        SetAnimation();
        
        // Move player
        rb.AddForceX(movementInput.x * movementSpeed);
        
        // Limit velocity
        rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocityX, -maxVelocity.x, maxVelocity.x), Mathf.Clamp(rb.linearVelocityY, -maxVelocity.y, maxVelocity.y));
    }

    private void SetAnimation()
    {
        anim.SetBool("Walking", isGrounded && rb.linearVelocityX != 0); // Walking if moving and grounded
    }

    // Input functions
    public void OnMove(InputValue playerInput)
    {
        movementInput = playerInput.Get<Vector2>();
    }

    public void OnJump()
    {
        if (numJumps > 0)
        {
            if (isGrounded)
                numJumps--; // Decrement jump count
            else
                numJumps = 0; // Only allow 1 jump if player is already in the air

            // Apply jump force as impulse
            rb.AddForceY(jumpStrength, ForceMode2D.Impulse);
        }
    }

    // Ground check
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            numJumps = 2;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}