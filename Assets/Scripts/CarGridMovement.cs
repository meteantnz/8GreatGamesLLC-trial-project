using UnityEngine;

public class CarGridMovement : MonoBehaviour
{
    public Vector2Int gridPosition; // Grid içindeki konumu (örneðin 2,3)
    public float moveSpeed = 5f; // Hareket hýzý
    public int gridWidth = 5; // Grid geniþliði
    public int gridHeight = 6; // Grid yüksekliði

    private Vector2Int moveDirection = Vector2Int.up; // Baþlangýçta yukarý hareket edecek
    private float moveCooldown = 0.5f; // Hareket süresi
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

            // Grid sýnýrlarýný kontrol et
            if (nextPosition.x >= 0 && nextPosition.x < gridWidth && nextPosition.y >= 0 && nextPosition.y < gridHeight)
            {
                gridPosition = nextPosition;
                UpdateCarPosition();
            }
        }
    }

    void UpdateCarPosition()
    {
        // Grid pozisyonunu dünya koordinatlarýna çevir (Örn: her hücre 1 birim uzaklýkta)
        transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
    }
}
