using UnityEngine;
using System.Collections.Generic;

public class ScrewdriverSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("List of empty GameObjects where screwdriver can spawn")]
    public List<Transform> spawnPoints = new List<Transform>();
    
    [Tooltip("The screwdriver prefab to spawn")]
    public GameObject screwdriverPrefab;
    
    [Tooltip("Should we spawn a screwdriver when the game starts?")]
    public bool spawnOnStart = true;
    
    [Header("Rotation Settings")]
    [Tooltip("Random rotation range on X axis")]
    public Vector2 xRotationRange = new Vector2(-30f, 30f);
    
    [Tooltip("Random rotation range on Y axis")]
    public Vector2 yRotationRange = new Vector2(0f, 360f);
    
    [Tooltip("Random rotation range on Z axis")]
    public Vector2 zRotationRange = new Vector2(-30f, 30f);

    private GameObject currentScrewdriver;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnScrewdriver();
        }
    }

    public void SpawnScrewdriver()
    {
        // Destroy existing screwdriver if one exists
        if (currentScrewdriver != null)
        {
            Destroy(currentScrewdriver);
        }

        // Check if we have spawn points and a prefab
        if (spawnPoints.Count == 0 || screwdriverPrefab == null)
        {
            Debug.LogWarning("ScrewdriverSpawner: No spawn points or prefab assigned!");
            return;
        }

        // Select a random spawn point
        int randomIndex = Random.Range(0, spawnPoints.Count);
        Transform selectedSpawn = spawnPoints[randomIndex];

        // Spawn the screwdriver
        currentScrewdriver = Instantiate(
            screwdriverPrefab, 
            selectedSpawn.position, 
            selectedSpawn.rotation
        );

        // Apply random rotation
        float xRot = Random.Range(xRotationRange.x, xRotationRange.y);
        float yRot = Random.Range(yRotationRange.x, yRotationRange.y);
        float zRot = Random.Range(zRotationRange.x, zRotationRange.y);
        
        currentScrewdriver.transform.Rotate(xRot, yRot, zRot);
    }

    // Call this method to spawn a new screwdriver at a random location
    public void RespawnScrewdriver()
    {
        SpawnScrewdriver();
    }

    // Editor button to test spawning in Play mode
    [ContextMenu("Test Spawn Screwdriver")]
    private void TestSpawn()
    {
        SpawnScrewdriver();
    }
}