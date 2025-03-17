using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 5;  // Yatay
    public int height = 6; // Dikey
    public float cellSize = 1.5f; // Grid h�cre boyutu
    public GameObject cellPrefab; // H�creyi temsil eden prefab (iste�e ba�l�)

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
                cell.name = $"Cell {x},{y}";  // H�cre ismi
                gridArray[x, y] = cell.transform;
            }
        }
    }

    // D�nya koordinat�n� grid h�cresine yuvarla
    public Vector3 GetNearestGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int z = Mathf.RoundToInt(worldPosition.z / cellSize);

        // Koordinatlar grid s�n�rlar� i�inde mi?
        x = Mathf.Clamp(x, 0, width - 1);
        z = Mathf.Clamp(z, 0, height - 1);

        return new Vector3(x * cellSize, 0, z * cellSize);
    }

    // Belirli bir grid h�cresinin pozisyonunu d�nd�r
    public Vector3 GetGridPosition(int x, int y)
    {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }
}
