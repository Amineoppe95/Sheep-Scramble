using UnityEngine;

public class SheepManager : MonoBehaviour
{
    [Header("Sheep Settings")]
    public GameObject sheepPrefab;       // Prefab to instantiate
    public Transform spawnPoint;         // Transform where sheep will spawn
    public int sheepCount = 5;           // Total number of sheep to spawn
    public float spawnCooldown = 2f;     // Time interval between spawns

    private int spawnedSheep = 0;        // Number of sheep already spawned
    private float spawnTimer = 0f;       // Timer to track cooldown

    void Update()
    {
        // Spawn sheep if the cooldown has elapsed and the limit isn't reached
        if (spawnedSheep < sheepCount)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnCooldown)
            {
                SpawnSheep();
                spawnTimer = 0f; // Reset the timer
            }
        }
    }

    void SpawnSheep()
    {
        // Instantiate a sheep at the spawn point's position and rotation
        GameObject Sheep_prefab =  Instantiate(sheepPrefab, spawnPoint.position, spawnPoint.rotation);
        Sheep_prefab.transform.parent = transform;
        spawnedSheep++;
    }
}
