using UnityEngine;

public class CarGridIntegration : MonoBehaviour
{
    public RectTransform canvas; // Canvas objesi (UI)
    public RectTransform[] gridCells; // Grid h�crelerini temsil eden UI elemanlar� (Image'ler)
    public Camera mainCamera; // 3D kamera

    private Vector2Int gridPosition; // Araba i�in grid pozisyonu

    void Start()
    {
        // Oyunun ba��nda araba rasgele bir grid h�cresine yerle�sin
        SetRandomPositionOnGrid();
    }

    void Update()
    {
        // Araba hareket ediyorsa, UI'daki konumunu g�ncelle
        UpdateCarPositionInUI();
    }

    void SetRandomPositionOnGrid()
    {
        // Grid h�crelerinden rastgele bir h�cre se�
        int randomIndex = Random.Range(0, gridCells.Length);

        // Se�ilen h�crenin UI pozisyonunu al
        Vector2 targetPosition = gridCells[randomIndex].anchoredPosition;

        // 3D d�nya koordinatlar�na d�n��t�r
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, mainCamera.nearClipPlane));

        // Arabay� rastgele se�ilen pozisyona yerle�tir
        transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
    }

    void UpdateCarPositionInUI()
    {
        // 3D araba pozisyonunu al
        Vector3 carWorldPos = transform.position;

        // 3D d�nya pozisyonunu canvas koordinatlar�na d�n��t�r
        Vector2 screenPos = mainCamera.WorldToScreenPoint(carWorldPos);

        // Canvas'a g�re normalize et (0,0 en sol alt, 1,1 en �st sa�)
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, null, out canvasPos);

        // Grid h�cresine yerle�tir
        foreach (RectTransform gridCell in gridCells)
        {
            // Her grid h�cresinin d�nya koordinatlar�yla kar��la�t�r
            if (Vector2.Distance(gridCell.anchoredPosition, canvasPos) < 100f) // Belirli bir mesafede ise
            {
                gridPosition = new Vector2Int((int)gridCell.anchoredPosition.x, (int)gridCell.anchoredPosition.y);
                transform.position = carWorldPos; // Arabay� grid h�cresine yerle�tir
                break;
            }
        }
    }
}
