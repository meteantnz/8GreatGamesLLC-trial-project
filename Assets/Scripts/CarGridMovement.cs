using UnityEngine;

public class CarGridMovement : MonoBehaviour
{
    public Vector2Int gridPosition; // Grid i�indeki konumu (�rne�in 2,3)
    public float moveSpeed = 5f; // Hareket h�z�
    public int gridWidth = 5; // Grid geni�li�i
    public int gridHeight = 6; // Grid y�ksekli�i

    private Vector2Int moveDirection = Vector2Int.up; // Ba�lang��ta yukar� hareket edecek
    private float moveCooldown = 0.5f; // Hareket s�resi
    private float moveTimer = 0f;

    void Start()
    {
        UpdateCarPosition();
    }

    void Update()
    {
        HandleInput();
        MoveCar();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2Int.right;
    }

    void MoveCar()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveCooldown)
        {
            moveTimer = 0f;
            Vector2Int nextPosition = gridPosition + moveDirection;

            // Grid s�n�rlar�n� kontrol et
            if (nextPosition.x >= 0 && nextPosition.x < gridWidth && nextPosition.y >= 0 && nextPosition.y < gridHeight)
            {
                gridPosition = nextPosition;
                UpdateCarPosition();
            }
        }
    }

    void UpdateCarPosition()
    {
        // Grid pozisyonunu d�nya koordinatlar�na �evir (�rn: her h�cre 1 birim uzakl�kta)
        transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
    }
}
