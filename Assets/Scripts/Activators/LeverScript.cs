using System.Collections;
using UnityEngine;

public class LeverScript : MonoBehaviour
{
    public enum ActivatorEffect { WorldReflection, MapReflection };
    [SerializeField] private ActivatorEffect activatorEffect;

    // PUBLIC
    public bool reflectXAxis, reflectYAxis;

    // PRIVATE
    private GameObject map;
    [SerializeField] private HingeJoint2D hinge;
    [SerializeField]  private Rigidbody2D rb;
    private bool canActivate = true, isXFlipped = false;
    private int activeSide = 0;
    
    void Start()
    {
        map = GameObject.Find("Map");

        // Subscribe to the X reflection event
        if (EventManager.instance != null)
        {
            EventManager.instance.onYReflection += Flip;
            EventManager.instance.onXReflection += Flip; 
        }
    }

    void OnDisable()
    {
        // Unsubscribe to the X reflection event
        if (EventManager.instance != null)
        {
            EventManager.instance.onYReflection -= Flip;
            EventManager.instance.onXReflection -= Flip; 
        }
    }

    void FixedUpdate()
    {
        if (hinge != null)
        {
            float angle = hinge.jointAngle;
    
            if (!isXFlipped)
            {
                NormalLogic(angle);
            }
            else
            {
                FlippedLogic(angle);
            }
        }
    }

    private void NormalLogic(float angle)
    {
        if (Mathf.Abs(angle - hinge.limits.min) < 1f && activeSide != 1)
        {
            rb.angularVelocity = 0; // Stop any rotation
            rb.linearVelocity = Vector2.zero; // Stop any movement

            if (canActivate && (activeSide == -1 || activeSide == 0))
            {
                StartCoroutine(ActivateEffect());
                activeSide = 1;
            }
        }
        else if (Mathf.Abs(angle - hinge.limits.max) < 1f && activeSide != -1)
        {
            rb.angularVelocity = 0; // Stop any rotation
            rb.linearVelocity = Vector2.zero; // Stop any movement

            if (canActivate && (activeSide == 1 || activeSide == 0))
            {
                StartCoroutine(ActivateEffect());
                activeSide = -1;
            }
        }
    }

    private void FlippedLogic(float angle)
    {
        if (Mathf.Abs(angle - hinge.limits.min) < 1f && activeSide != -1)
        {
            rb.angularVelocity = 0; // Stop any rotation
            rb.linearVelocity = Vector2.zero; // Stop any movement

            if (canActivate && (activeSide == 1 || activeSide == 0))
            {
                StartCoroutine(ActivateEffect());
                activeSide = -1;
            }
        }
        else if (Mathf.Abs(angle - hinge.limits.max) < 1f && activeSide != 1)
        {
            rb.angularVelocity = 0; // Stop any rotation
            rb.linearVelocity = Vector2.zero; // Stop any movement

            if (canActivate && (activeSide == -1 || activeSide == 0))
            {
                StartCoroutine(ActivateEffect());
                activeSide = 1;
            }
        }
    }

    private void Flip()
    {
        isXFlipped = !isXFlipped;
    }

    private IEnumerator ActivateEffect()
    {
        canActivate = false;

        switch (activatorEffect)
        {
            case ActivatorEffect.WorldReflection:
                EventManager.instance.WorldReflection(reflectXAxis, reflectYAxis, map);
                break;
            case ActivatorEffect.MapReflection:
                EventManager.instance.MapReflection(reflectXAxis, reflectYAxis, map);
                break;
        }

        yield return new WaitForSeconds(2f);
        canActivate = true;
    }
}