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
    private HingeJoint2D hinge;
    private Rigidbody2D rb;
    private bool canActivate = true, isFlipped = false;
    private int activeSide = 0;
    
    void Start()
    {
        map = GameObject.Find("Map");
        hinge = GetComponent<HingeJoint2D>();    
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float angle = hinge.jointAngle;
        
        // Lock the lever in place if it's near the limits and activate effect
        if (!isFlipped)
        {
            if (Mathf.Abs(angle - hinge.limits.min) < 1f)
            {
                rb.angularVelocity = 0;
                if (canActivate && (activeSide == -1 || activeSide == 0))
                {
                    StartCoroutine(ActivateEffect());
                    activeSide = 1;
                }
            }
            else if (Mathf.Abs(angle - hinge.limits.max) < 1f)
            {
                rb.angularVelocity = 0;
                if (canActivate && (activeSide == 1 || activeSide == 0))
                {
                    StartCoroutine(ActivateEffect());
                    activeSide = -1;
                }
            }
        }
        else
        {
            if (Mathf.Abs(angle - hinge.limits.min) < 1f)
            {
                rb.angularVelocity = 0;
                if (canActivate && (activeSide == 1 || activeSide == 0))
                {
                    StartCoroutine(ActivateEffect());
                    activeSide = -1;
                }
            }
            else if (Mathf.Abs(angle - hinge.limits.max) < 1f)
            {
                rb.angularVelocity = 0;
                if (canActivate && (activeSide == -1 || activeSide == 0))
                {
                    StartCoroutine(ActivateEffect());
                    activeSide = 1;
                }
            }
        }
    }

    private IEnumerator ActivateEffect()
    {
        canActivate = false;

        switch (activatorEffect)
        {
            case ActivatorEffect.WorldReflection:
                EventManager.instance.WorldReflection(reflectXAxis, reflectYAxis, map);
                isFlipped = !isFlipped;
                break;
            case ActivatorEffect.MapReflection:
                EventManager.instance.MapReflection(reflectXAxis, reflectYAxis, map);
                isFlipped = !isFlipped;
                break;
        }

        yield return new WaitForSeconds(2f);
        canActivate = true;
    }
}