using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;  // Grid h�cresi prefab'�
    public float cellSize = 1f;        // H�cre boyutu
    public int gridWidth = 5;          // Yatay h�cre say�s�
    public int gridHeight = 6;         // Dikey h�cre say�s�

    private GridCell[,] gridCells;     // Grid h�crelerini tutacak 2D dizi

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

    // Grid'i olu�turma
    void CreateGrid()
    {
        gridCells = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // H�cre prefab'�n� instantiate et
                GameObject cell = Instantiate(gridCellPrefab, new Vector2(x * cellSize, y * cellSize), Quaternion.identity);
                cell.name = "Cell_" + x + "_" + y;

                // GridCell bile�enini al ve gridCells dizisine kaydet
                GridCell cellComponent = cell.GetComponent<GridCell>();
                cellComponent.gridPosition = new Vector2Int(x, y);
                gridCells[x, y] = cellComponent;
            }
        }
    }

    // Belirli bir h�creyi al
    public GridCell GetCellAt(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return gridCells[x, y];
        }
        return null;
    }
}
