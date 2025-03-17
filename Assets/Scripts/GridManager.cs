using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;  // Yatay
    public int height = 6; // Dikey
    public float cellSize = 1.5f; // Grid hücre boyutu
    public GameObject cellPrefab; // Hücreyi temsil eden prefab (isteðe baðlý)

    private Transform[,] gridArray; // Grid'in objeleri saklanacak

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        gridArray = new Transform[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize); // 3D pozisyon hesaplama
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.name = $"Cell {x},{y}";  // Hücre ismi
                gridArray[x, y] = cell.transform;
            }
        }
    }

    // Dünya koordinatýný grid hücresine yuvarla
    public Vector3 GetNearestGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int z = Mathf.RoundToInt(worldPosition.z / cellSize);

        // Koordinatlar grid sýnýrlarý içinde mi?
        x = Mathf.Clamp(x, 0, width - 1);
        z = Mathf.Clamp(z, 0, height - 1);

        return new Vector3(x * cellSize, 0, z * cellSize);
    }

    // Belirli bir grid hücresinin pozisyonunu döndür
    public Vector3 GetGridPosition(int x, int y)
    {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }
}
