using UnityEngine;
using System.Collections.Generic;

public class CarMovement : MonoBehaviour
{
    private GridManager gridManager;
    private List<Transform> carParts;
    private CarManager carManager;
    PassiveSpawner passiveSpawner;
    private Vector3 offset;
    private bool isDragging = false;
    private List<Vector3> previousPositions = new List<Vector3>();
    private bool isReversed = false;
    public float rotationSpeed = 5f;
    public List<Transform> seatPositions; // Arabanın içindeki boş koltuk yerleri
    private List<GameObject> stickmenOnEdge = new List<GameObject>(); // Edge Object etrafındaki stickmanler



    public void Initialize(GridManager manager, List<Transform> parts, CarManager carMgr)
    {
        gridManager = manager;
        carParts = parts;
        carManager = carMgr;

        foreach (Transform part in carParts)
        {
            previousPositions.Add(part.position);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                int partIndex = carParts.IndexOf(hit.transform);

                if (partIndex == 0 || partIndex == carParts.Count - 1)
                {
                    isDragging = true;
                    offset = hit.transform.position - GetMouseWorldPosition();

                    isReversed = (partIndex == carParts.Count - 1);
                    if (isReversed) ReverseCarParts();
                }
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 targetPos = GetMouseWorldPosition() + offset;
            MoveCarToGrid(targetPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldPos = ray.GetPoint(distance);
            float clampedX = Mathf.Clamp(worldPos.x, 0, (gridManager.width - 1) * gridManager.cellSize);
            float clampedZ = Mathf.Clamp(worldPos.z, 0, (gridManager.height - 1) * gridManager.cellSize);
            return new Vector3(clampedX, 0, clampedZ);
        }

        return Vector3.zero;
    }

    void MoveCarToGrid(Vector3 targetPos)
    {
        Vector3 nearestGridPos = gridManager.GetNearestGridPosition(targetPos);

        if (nearestGridPos == carParts[0].position) return;
        if (nearestGridPos == carParts[1].position || nearestGridPos == carParts[2].position) return;

        previousPositions.Insert(0, carParts[0].position);
        previousPositions.RemoveAt(previousPositions.Count - 1);

        if (IsPositionOccupied(nearestGridPos))
        {
            nearestGridPos = FindAlternativeDirection();
        }

        UpdateCarRotation(nearestGridPos - carParts[0].position);
        carParts[0].position = nearestGridPos;

        for (int i = 1; i < carParts.Count; i++)
        {
            Vector3 directionToPrevious = carParts[i - 1].position - carParts[i].position;
            float angle = Mathf.Atan2(directionToPrevious.x, directionToPrevious.z) * Mathf.Rad2Deg;
            angle = Mathf.Round(angle / 90) * 90;

            carParts[i].rotation = Quaternion.Euler(0, angle, 0);
            carParts[i].position = previousPositions[i - 1];
        }

        CheckForEdgeObjectCollision();

        if (gridManager.IsEdgeObjectAtPosition(carParts[0].position))
        {
            FindStickmenNearEdgeObject(carParts[0].position); // Yakındaki Stickman’leri bul
            TransferStickmenToCar(); // Stickman'leri arabaya yerleştir
        }
    }
    void FindStickmenNearEdgeObject(Vector3 edgePosition)
    {
        stickmenOnEdge.Clear(); // Önceki veriyi temizle

        Collider[] colliders = Physics.OverlapSphere(edgePosition, gridManager.cellSize); // Yakındaki nesneleri bul
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Stickman")) // Eğer çöp adam ise
            {
                stickmenOnEdge.Add(col.gameObject); // Listeye ekle
            }
        }
    }
    void UpdateCarRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion newRotation = Quaternion.Euler(0, angle, 0);

        foreach (Transform part in carParts)
        {
            part.rotation = newRotation;
        }
    }

    void ReverseCarParts()
    {
        previousPositions.Reverse();
    }

    Vector3 FindAlternativeDirection()
    {
        Vector3 headPos = carParts[0].position;
        Vector3[] directions = {
            new Vector3(gridManager.cellSize, 0, 0),
            new Vector3(-gridManager.cellSize, 0, 0),
            new Vector3(0, 0, gridManager.cellSize),
            new Vector3(0, 0, -gridManager.cellSize)
        };

        foreach (var dir in directions)
        {
            Vector3 newPos = headPos + dir;
            if (!IsPositionOccupied(newPos) && IsWithinGrid(newPos))
            {
                return newPos;
            }
        }

        return headPos;
    }

    bool IsPositionOccupied(Vector3 position)
    {
        foreach (Transform part in carParts)
        {
            if (part.position == position) return true;
        }
        return false;
    }

    bool IsWithinGrid(Vector3 position)
    {
        return position.x >= 0 && position.x < gridManager.width * gridManager.cellSize &&
               position.z >= 0 && position.z < gridManager.height * gridManager.cellSize;
    }

    void CheckForEdgeObjectCollision()
    {
        foreach (Transform part in carParts)
        {
            Vector3 gridPosition = gridManager.GetNearestGridPosition(part.position);

            if (gridManager.IsEdgeObjectAtPosition(gridPosition))
            {
                StickmanManager.Instance.MoveStickmenToCar(carManager);
                break;
            }
        }
    }
    void TransferStickmenToCar()
    {
        if (stickmenOnEdge.Count == 0) return; // Eğer taşınacak stickman yoksa çık

        int seatIndex = 0; // Koltukları sırayla doldurmak için

        foreach (GameObject stickman in stickmenOnEdge)
        {
            if (seatIndex >= seatPositions.Count) break; // Eğer boş koltuk kalmadıysa dur

            stickman.transform.position = seatPositions[seatIndex].position; // Koltuğa taşı
            stickman.transform.SetParent(transform); // Arabaya bağla (çocuk nesne yap)
            seatIndex++;
        }

        stickmenOnEdge.Clear(); // Stickman'ler arabaya geçti, listeyi temizle
    }
}
