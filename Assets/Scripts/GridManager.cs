using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;
    public int height = 6;
    public float cellSize = 1.5f;
    public GameObject cellPrefab;
    public PassiveSpawner passiveSpawner;
    public StickmanManager stickmanManager;
    private List<Vector3> edgeObjectPositions = new List<Vector3>(); // Edge objelerin pozisyonlarý


    private Transform[,] gridArray;

    void Start()
    {
        GenerateGrid();

        if (passiveSpawner != null)
        {
            passiveSpawner.Initialize(this, gridArray, stickmanManager);
        }
    }

    void GenerateGrid()
    {
        gridArray = new Transform[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.name = $"Cell {x},{y}";
                gridArray[x, y] = cell.transform;
            }
        }
    }

    public Vector3 GetNearestGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int z = Mathf.RoundToInt(worldPosition.z / cellSize);

        x = Mathf.Clamp(x, 0, width - 1);
        z = Mathf.Clamp(z, 0, height - 1);

        return new Vector3(x * cellSize, 0, z * cellSize);
    }
    public bool IsEdgeObjectAtPosition(Vector3 position)
    {
        foreach (Vector3 edgePos in edgeObjectPositions)
        {
            if (Vector3.Distance(edgePos, position) < cellSize * 0.5f) // Yakýnsa ayný hücrede say
            {
                return true;
            }
        }
        return false;
    }

}
