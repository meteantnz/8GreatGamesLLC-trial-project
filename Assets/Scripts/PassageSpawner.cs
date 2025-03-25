using UnityEngine;
using System.Collections.Generic;
public class GridCell : MonoBehaviour
{
    public bool isOccupied = false; // Hücrenin occupation durumu
}
public class PassageSpawner : MonoBehaviour
{
    public GameObject passageObjectPrefab;
    public int maxPassageObjectsToSpawn = 3;
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

        int objectsToSpawn = Mathf.Min(maxPassageObjectsToSpawn, edgePositions.Count);

        for (int i = 0; i < objectsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, edgePositions.Count);
            Vector3 spawnPosition = edgePositions[randomIndex];

            GameObject spawnedObject = Instantiate(passageObjectPrefab, spawnPosition, Quaternion.identity);

            // Hücrenin pozisyonunu al
            Vector3 cellPosition = gridManager.GetNearestGridPosition(spawnPosition);

            // Hücrenin baðlý olduðu hücreyi bul ve ilgili bool deðeri ekle
            int x = Mathf.RoundToInt(spawnPosition.x / gridManager.cellSize);
            int z = Mathf.RoundToInt(spawnPosition.z / gridManager.cellSize);

            if (gridArray[x, z] != null)
            {
                // Passage objesinin baðlý olduðu hücreye ait bool deðiþkenini ekleyelim
                GridCell cell = gridArray[x, z].GetComponent<GridCell>();
                if (cell != null)
                {
                    cell.isOccupied = true;  // Hücrede bir passage objesi olduðu için true yapýyoruz
                }
            }

            // Obje yönünü ayarla
            Vector3 direction = cellPosition - spawnPosition;
            Quaternion rotation = Quaternion.LookRotation(direction);
            spawnedObject.transform.rotation = rotation;

            edgePositions.RemoveAt(randomIndex);

            if (stickmanManager != null)
            {
                stickmanManager.SpawnStickmen(spawnPosition, rotation);
            }
        }
    }

    // Araba hücrenin üzerine geldiðinde, bool deðerini kontrol etmek için bir fonksiyon ekleyebiliriz
    public void CheckCarInCell(Vector3 carPosition)
    {
        // Aracýn bulunduðu hücrenin pozisyonunu bul
        int x = Mathf.RoundToInt(carPosition.x / gridManager.cellSize);
        int z = Mathf.RoundToInt(carPosition.z / gridManager.cellSize);

        // Hücredeki passage objesinin bool deðerini kontrol et
        if (gridArray[x, z] != null)
        {
            GridCell cell = gridArray[x, z].GetComponent<GridCell>();
            if (cell != null)
            {
                
                // Hücreye araba girdi mi? 
                if (cell.isOccupied)
                {
                    // Burada istenilen iþlemi yapabilirsiniz
                    Debug.Log("Araba, hücreye girdi.");
                }
            }
        }
    }
}

