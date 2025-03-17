using UnityEngine;

public class CarControlSystem : MonoBehaviour
{
    public GameObject frontPart;  // �n par�a
    public GameObject middlePart; // Orta par�a
    public GameObject rearPart;   // Arka par�a

    private GridManager gridManager;
    private GridCell frontCell;   // �n par�a hangi h�crede
    private GridCell middleCell;  // Orta par�a hangi h�crede
    private GridCell rearCell;    // Arka par�a hangi h�crede

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // Arabay� ba�latmak i�in grid �zerindeki h�crelere yerle�tir
    public void PlaceCarOnGrid(int startX, int startY)
    {
        frontCell = gridManager.GetCellAt(startX + 1, startY);  // Araba front k�sm� bir h�cre sa�a kayacak
        middleCell = gridManager.GetCellAt(startX, startY);      // Orta par�a
        rearCell = gridManager.GetCellAt(startX - 1, startY);    // Araba rear k�sm� bir h�cre sola kayacak

        // Arabay� h�crelere yerle�tir
        frontCell.carPart = frontPart;
        middleCell.carPart = middlePart;
        rearCell.carPart = rearPart;

        frontPart.transform.position = frontCell.transform.position;
        middlePart.transform.position = middleCell.transform.position;
        rearPart.transform.position = rearCell.transform.position;
    }
}
