using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [SerializeField] private int spawnerID = 0;
    private GameObject playerPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Load Player prefab
        playerPrefab = Resources.Load<GameObject>("Player/Player");

        // Subscribe to spawn player event
        if (EventManager.instance != null)
            EventManager.instance.onSpawnPlayer += SpawnPlayer;

        // Perform initial player spawn
        SpawnPlayer(0);
    }

    void OnDisable()
    {
        // Unsubscribe from spawn player event
        if (EventManager.instance != null)
            EventManager.instance.onSpawnPlayer -= SpawnPlayer;
    }

    private void SpawnPlayer(int spawnerID)
    {
        if (spawnerID == this.spawnerID)
        {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
    }
}