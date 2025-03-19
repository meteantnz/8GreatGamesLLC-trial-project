using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public GridManager gridManager;
    public List<Transform> carParts; // 3 parçadan oluşan araba
    private Vector3 offset;
    private bool isDragging = false;
    private List<Vector3> previousPositions = new List<Vector3>(); // Önceki pozisyonları saklayan liste
    private bool isReversed = false; // Araba yönünü ters çevirmek için

    void Start()
    {
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

                    // Eğer arkadaki parçaya tıklandıysa ters yönde hareket et
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

        previousPositions.Insert(0, carParts[0].position);
        previousPositions.RemoveAt(previousPositions.Count - 1);

        if (IsPositionOccupied(nearestGridPos))
        {
            nearestGridPos = FindAlternativeDirection();
        }

        carParts[0].position = nearestGridPos;

        for (int i = 1; i < carParts.Count; i++)
        {
            carParts[i].position = previousPositions[i - 1];
        }
    }

    void ReverseCarParts()
    {
        carParts.Reverse();
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
}
