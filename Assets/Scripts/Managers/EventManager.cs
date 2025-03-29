using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Initialize as singleton
    public static EventManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
            Destroy(gameObject);
    }
    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    // World Reflection
    public event Action onWorldReflection;
    public void WorldReflection(bool reflectXAxis, bool reflectYAxis, GameObject map)
    {
        if (reflectXAxis) // X-Axis Reflection
        {
            map.transform.localScale = new Vector3(map.transform.localScale.x, -map.transform.localScale.y, map.transform.localScale.z); // Reflect the world vertically
            PlayerLogic.instance.transform.position = new Vector2(PlayerLogic.instance.transform.position.x, -PlayerLogic.instance.transform.position.y); // Reflect the player's position
        }
        if (reflectYAxis) // Y-Axis Reflection
        {
            map.transform.localScale = new Vector3(-map.transform.localScale.x, map.transform.localScale.y, map.transform.localScale.z); // Reflect the world horizontally
            PlayerLogic.instance.transform.position = new Vector2(-PlayerLogic.instance.transform.position.x, PlayerLogic.instance.transform.position.y); // Reflect the player's position
        }
        if (onWorldReflection != null && (reflectXAxis || reflectYAxis))
            onWorldReflection();
    }

    // Map Reflection
    public event Action onMapReflection;
    public void MapReflection(bool reflectXAxis, bool reflectYAxis, GameObject map)
    {
        if (reflectXAxis) // X-Axis Reflection
            map.transform.localScale = new Vector3(map.transform.localScale.x, -map.transform.localScale.y, map.transform.localScale.z); // Reflect the world vertically
        if (reflectYAxis) // Y-Axis Reflection
            map.transform.localScale = new Vector3(-map.transform.localScale.x, map.transform.localScale.y, map.transform.localScale.z); // Reflect the world horizontally

        if (onMapReflection != null && (reflectXAxis || reflectYAxis))
            onMapReflection();
    }
}