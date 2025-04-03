using System.Collections;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    // PUBLIC
    public bool isAlive = true;

    // PRIVATE
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    // Initialize as singleton instance
    public static PlayerLogic instance;
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Get components
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // Subscribe to events
        if (EventManager.instance != null)
        {
            EventManager.instance.onKillPlayer += InvokeDie;
        }
    }
    void OnDestroy()
    {
        if (instance == this)
            instance = null;

        // Unsubscribe to events
        if (EventManager.instance != null)
        {
            EventManager.instance.onKillPlayer -= InvokeDie;
        }
    }

    // DEATH ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void InvokeDie()
    {
        StartCoroutine(Die());
    }
    
    private IEnumerator Die()
    {
        // Set alive to false
        isAlive = false;

        // Reset velocity to 0 and disable gravity
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        // TODO: Play death animation
        sprite.color = new Color(1, 0, 0, 1); // Red color

        // Destroy player
        yield return new WaitForSeconds(0.85f);
        Destroy(gameObject);
    }

    // GETTERS & SETTERS ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Movement Speed
    public float GetMovementSpeed()
    {
        return playerMovement.movementSpeed;
    }
    public void SetMovementSpeed(float newSpeed)
    {
        playerMovement.movementSpeed = newSpeed;
    }

    // JumpStrength
    public float GetJumpStrength()
    {
        return playerMovement.jumpStrength;
    }
    public void SetJumpStrength(float newStrength)
    {
        playerMovement.jumpStrength = newStrength;
    }

    // Max Velocity
    public Vector2 GetMaxVelocity()
    {
        return playerMovement.maxVelocity;
    }
    public void SetMaxVelocity(Vector2 newMaxVelocity)
    {
        playerMovement.maxVelocity = newMaxVelocity;
    }

    // Grounded
    public bool IsGrounded()
    {
        return playerMovement.isGrounded;
    }
}