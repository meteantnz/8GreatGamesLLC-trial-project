using UnityEngine;
using System.Collections.Generic;

public class PassiveSpawner : MonoBehaviour
{
    public GameObject edgeObjectPrefab;
    public int maxEdgeObjectsToSpawn = 3;
    public float spawnOffset = 0.3f;

    private GridManager gridManager;
    private Transform[,] gridArray;
    private StickmanManager stickmanManager;

    public void Initialize(GridManager manager, Transform[,] grid, StickmanManager stickmanMgr)
    {
        gridManager = manager;
        gridArray = grid;
        stickmanManager = stickmanMgr;

        SpawnEdgeObjects();
    }

    void SpawnEdgeObjects()
    {
        List<Vector3> edgePositions = new List<Vector3>();

        for (int x = 0; x < gridManager.width; x++)
        {
            for (int y = 0; y < gridManager.height; y++)
            {
                Vector3 cellPosition = gridArray[x, y].position;

                if (x == 0) edgePositions.Add(cellPosition + new Vector3(-gridManager.cellSize / 2 + spawnOffset, 0, 0));
                if (x == gridManager.width - 1) edgePositions.Add(cellPosition + new Vector3(gridManager.cellSize / 2 - spawnOffset, 0, 0));
                if (y == 0) edgePositions.Add(cellPosition + new Vector3(0, 0, -gridManager.cellSize / 2 + spawnOffset));
                if (y == gridManager.height - 1) edgePositions.Add(cellPosition + new Vector3(0, 0, gridManager.cellSize / 2 - spawnOffset));
            }
        }

        int objectsToSpawn = Mathf.Min(maxEdgeObjectsToSpawn, edgePositions.Count);

        for (int i = 0; i < objectsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, edgePositions.Count);
            Vector3 spawnPosition = edgePositions[randomIndex];

            GameObject spawnedObject = Instantiate(edgeObjectPrefab, spawnPosition, Quaternion.identity);

            Vector3 cellPosition = gridManager.GetNearestGridPosition(spawnPosition);
            Vector3 direction = cellPosition - spawnPosition;
            Quaternion rotation = Quaternion.LookRotation(direction);
            spawnedObject.transform.rotation = rotation;

            edgePositions.RemoveAt(randomIndex);

            int x = Mathf.RoundToInt(spawnPosition.x / gridManager.cellSize);
            int z = Mathf.RoundToInt(spawnPosition.z / gridManager.cellSize);

            if (gridArray[x, z] != null)
            {
                BoxCollider boxCollider = gridArray[x, z].gameObject.AddComponent<BoxCollider>();
                boxCollider.size = new Vector3(gridManager.cellSize, 1f, gridManager.cellSize);
            }

            if (stickmanManager != null)
            {
                stickmanManager.SpawnStickmen(spawnPosition, rotation);
            }
        }
    }
}
