using UnityEngine;
using System.Collections.Generic;

public class CarManager : MonoBehaviour
{
    public GridManager gridManager;
    public List<Transform> carParts; // Arabanýn parçalarý
    private CarMovement carMovement;

    public List<Transform> stickmanSeats; // Stickmanlerin oturacaðý boþ noktalar
    private List<GameObject> stickmenOnBoard = new List<GameObject>(); // Arabaya alýnan stickmanler

    void Start()
    {
        carMovement = GetComponent<CarMovement>();

        if (carMovement != null)
        {
            carMovement.Initialize(gridManager, carParts, this);
        }
    }

    public void AddStickmanToCar(GameObject stickman)
    {
        if (stickmenOnBoard.Count < stickmanSeats.Count)
        {
            Transform seat = stickmanSeats[stickmenOnBoard.Count];
            stickman.transform.position = seat.position;
            stickman.transform.parent = seat; // Arabaya baðlý kalmasý için

            stickmenOnBoard.Add(stickman);
        }
    }
}
