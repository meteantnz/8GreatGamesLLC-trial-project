using UnityEngine;

public class CarGridIntegration : MonoBehaviour
{
    public RectTransform canvas; // Canvas objesi (UI)
    public RectTransform[] gridCells; // Grid hücrelerini temsil eden UI elemanlarý (Image'ler)
    public Camera mainCamera; // 3D kamera

    private Vector2Int gridPosition; // Araba için grid pozisyonu

    void Start()
    {
        // Oyunun baþýnda araba rasgele bir grid hücresine yerleþsin
        SetRandomPositionOnGrid();
    }

    void Update()
    {
        // Araba hareket ediyorsa, UI'daki konumunu güncelle
        UpdateCarPositionInUI();
    }

    void SetRandomPositionOnGrid()
    {
        // Grid hücrelerinden rastgele bir hücre seç
        int randomIndex = Random.Range(0, gridCells.Length);

        // Seçilen hücrenin UI pozisyonunu al
        Vector2 targetPosition = gridCells[randomIndex].anchoredPosition;

        // 3D dünya koordinatlarýna dönüþtür
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, mainCamera.nearClipPlane));

        // Arabayý rastgele seçilen pozisyona yerleþtir
        transform.position = new Vector3(worldPosition.x, 0, worldPosition.z);
    }

    void UpdateCarPositionInUI()
    {
        // 3D araba pozisyonunu al
        Vector3 carWorldPos = transform.position;

        // 3D dünya pozisyonunu canvas koordinatlarýna dönüþtür
        Vector2 screenPos = mainCamera.WorldToScreenPoint(carWorldPos);

        // Canvas'a göre normalize et (0,0 en sol alt, 1,1 en üst sað)
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPos, null, out canvasPos);

        // Grid hücresine yerleþtir
        foreach (RectTransform gridCell in gridCells)
        {
            // Her grid hücresinin dünya koordinatlarýyla karþýlaþtýr
            if (Vector2.Distance(gridCell.anchoredPosition, canvasPos) < 100f) // Belirli bir mesafede ise
            {
                gridPosition = new Vector2Int((int)gridCell.anchoredPosition.x, (int)gridCell.anchoredPosition.y);
                transform.position = carWorldPos; // Arabayý grid hücresine yerleþtir
                break;
            }
        }
    }
}
