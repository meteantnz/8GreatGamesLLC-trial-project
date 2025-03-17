using UnityEngine;

public class CarControlSystem : MonoBehaviour
{
    public GameObject frontPart;  // Ön parça
    public GameObject middlePart; // Orta parça
    public GameObject rearPart;   // Arka parça

    private GridManager gridManager;
    private GridCell frontCell;   // Ön parça hangi hücrede
    private GridCell middleCell;  // Orta parça hangi hücrede
    private GridCell rearCell;    // Arka parça hangi hücrede

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
    }

    // Arabayý baþlatmak için grid üzerindeki hücrelere yerleþtir
    public void PlaceCarOnGrid(int startX, int startY)
    {
        frontCell = gridManager.GetCellAt(startX + 1, startY);  // Araba front kýsmý bir hücre saða kayacak
        middleCell = gridManager.GetCellAt(startX, startY);      // Orta parça
        rearCell = gridManager.GetCellAt(startX - 1, startY);    // Araba rear kýsmý bir hücre sola kayacak

        // Arabayý hücrelere yerleþtir
        frontCell.carPart = frontPart;
        middleCell.carPart = middlePart;
        rearCell.carPart = rearPart;

        frontPart.transform.position = frontCell.transform.position;
        middlePart.transform.position = middleCell.transform.position;
        rearPart.transform.position = rearCell.transform.position;
    }
}
