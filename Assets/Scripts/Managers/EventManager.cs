using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private int currentSpawner = 0;

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

    // Player Spawn Point
    public void SetSpawner(int spawnerID)
    {
        currentSpawner = spawnerID;
    }

    // Player Spawning
    public event Action<int> onSpawnPlayer;
    public void SpawnPlayer()
    {
        if (onSpawnPlayer != null)
            onSpawnPlayer(currentSpawner);
    }

    // Player Death
    public event Action onKillPlayer;
    public void KillPlayer()
    {
        if (onKillPlayer != null)
        {
            onKillPlayer();
            Invoke(nameof(SpawnPlayer), 1f);
        }
    }

    // Events for reflections
    public event Action onXReflection;
    public event Action onYReflection;

    // World Reflection
    public void WorldReflection(bool reflectXAxis, bool reflectYAxis, GameObject map)
    {
        if (reflectXAxis) // X-Axis Reflection
        {
            // Reflect the map vertically
            Vector3 map_scale = map.transform.localScale;
            map_scale.y *= -1;
            map.transform.localScale = map_scale; 

            // Reflect the player's position
            PlayerLogic.instance.transform.position = new Vector2(PlayerLogic.instance.transform.position.x, -PlayerLogic.instance.transform.position.y); 

            // Call the reflection event
            if (onXReflection != null)
                onXReflection();
        }
        if (reflectYAxis) // Y-Axis Reflection
        {
            // Reflect the map horizontally
            Vector3 map_scale = map.transform.localScale;
            map_scale.x *= -1;
            map.transform.localScale = map_scale; 

            // Reflect the player's position
            PlayerLogic.instance.transform.position = new Vector2(-PlayerLogic.instance.transform.position.x, PlayerLogic.instance.transform.position.y);

            // Call the reflection event
            if (onYReflection != null)
                onYReflection();
        }
    }

    // Map Reflection
    public void MapReflection(bool reflectXAxis, bool reflectYAxis, GameObject map)
    {
        if (reflectXAxis) // X-Axis Reflection
        {
            // Reflect the map vertically
            Vector3 map_scale = map.transform.localScale;
            map_scale.y *= -1;
            map.transform.localScale = map_scale; 

            // Call the reflection event
            if (onXReflection != null)
                onXReflection();
        }
        if (reflectYAxis) // Y-Axis Reflection
        {
            // Reflect the map horizontally
            Vector3 map_scale = map.transform.localScale;
            map_scale.x *= -1;
            map.transform.localScale = map_scale;

            // Call the reflection event
            if (onYReflection != null)
                onYReflection();
        }
    }
}