using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    // PUBLIC
    public bool isAlive = true;
    public Transform spawnPoint;

    // PRIVATE
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private PlayerInput playerInput;
    private float gravityScale;

    // Initialize as singleton instance in DontDestroyOnLoad
    public static PlayerLogic instance;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        // Get components
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        gravityScale = rb.gravityScale;
        sprite = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();

        // Subscribe spawn function to scene loaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += Spawn;
    }
    void OnDisable()
    {
        if (instance == this)
            instance = null;

        // Unsubscribe spawn function from scene loaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= Spawn;
    }

    // SPAWN (INITIALIZATION) ----------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Spawn(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
    {
        // Initialize player at spawn point if the scene is a level
        if (scene.name.Contains("Level"))
        {
            // Set position
            if (spawnPoint != null)
                transform.position = spawnPoint.position;

            // Enable sprite
            sprite.enabled = true;

            // Switch input map to player controls
            playerInput.SwitchCurrentActionMap("Player");
        }

        // If the scene is not a level disable player visuals and switch to UI controls  
        else
        {
            // Disable sprite
            sprite.enabled = false;

            // Swap input map to UI
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    // DEATH ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Die()
    {
        // Set alive to false
        isAlive = false;

        // Reset velocity to 0 and disable gravity
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        // TODO: Play death animation
        sprite.color = new Color(1, 0, 0, 1); // Red color

        // Respawn player after a 1s delay
        Invoke(nameof(Respawn), 1f);
    }

    private void Respawn()
    {
        // Reset color to normal
        sprite.color = new Color(1, 1, 1, 1); // White color

        // Reset position
        transform.position = spawnPoint.position;

        // Enable gravity
        rb.gravityScale = gravityScale;

        // TODO: Play spawn animation

        // Set alive to true
        isAlive = true;
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