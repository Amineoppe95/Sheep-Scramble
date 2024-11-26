using System.Collections.Generic;
using UnityEngine;

public class SheepManager : MonoBehaviour
{
    public static SheepManager instance;

    [Header("Sheep Settings")]
    public GameObject sheepPrefab;       // Prefab to instantiate
    public Transform spawnPoint;         // Transform where sheep will spawn
    public int sheepCount = 5;           // Total number of sheep to spawn
    public float spawnCooldown = 2f;     // Time interval between spawns

    private int spawnedSheep = 0;        // Number of sheep already spawned
    private float spawnTimer = 0f;       // Timer to track cooldown

    public GameObject FirstSheep = null; //{ get; private set; } 
    private bool firstSheepAssigned = false; 
    public List<GameObject> Sheeps = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

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
        Sheeps.Add(Sheep_prefab);
        Sheep_prefab.transform.parent = transform;

        if (!firstSheepAssigned && Sheeps.Count > 0 )
        {
            print("Asssing sheep 0 ");
            AssignFirstSheep(Sheeps[0]);
        }

        spawnedSheep++;
    }

    #region SheepSheepLogic
    private void AssignFirstSheep(GameObject sheepInstance)
    {
        FirstSheep = sheepInstance;
        firstSheepAssigned = true; // Mark the first sheep as assigned
    }

    public void ResetFirstSheep()
    {
        Sheeps.Remove(Sheeps[0]);

        if (Sheeps.Count>0)
        { FirstSheep = Sheeps[0]; }
        else
        { FirstSheep = null; }

       
        firstSheepAssigned = false; // Mark the first sheep as assigned
        
    }
     
     

    #endregion

}
