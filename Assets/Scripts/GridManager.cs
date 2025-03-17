using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;  // Grid hücresi prefab'ý
    public float cellSize = 1f;        // Hücre boyutu
    public int gridWidth = 5;          // Yatay hücre sayýsý
    public int gridHeight = 6;         // Dikey hücre sayýsý

    private GridCell[,] gridCells;     // Grid hücrelerini tutacak 2D dizi

    void Start()
    {
        CreateGrid();


        if (gridCellPrefab == null)
        {
            Debug.LogError("gridCellPrefab is not assigned in the inspector!");
        }
        else
        {
            CreateGrid();
        }


    }

    // Grid'i oluþturma
    void CreateGrid()
    {
        gridCells = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Hücre prefab'ýný instantiate et
                GameObject cell = Instantiate(gridCellPrefab, new Vector2(x * cellSize, y * cellSize), Quaternion.identity);
                cell.name = "Cell_" + x + "_" + y;

                // GridCell bileþenini al ve gridCells dizisine kaydet
                GridCell cellComponent = cell.GetComponent<GridCell>();
                cellComponent.gridPosition = new Vector2Int(x, y);
                gridCells[x, y] = cellComponent;
            }
        }
    }

    // Belirli bir hücreyi al
    public GridCell GetCellAt(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return gridCells[x, y];
        }
        return null;
    }
}
