using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public GridManager gridManager;  // Grid sistemine eriþim
    public List<Transform> carParts; // Arabanýn parçalarý (0: Ön, 1: Orta, 2: Arka)
    public float moveSpeed = 5f; // Hareket hýzý
    public float partDistance = 1.5f; // Parçalar arasýndaki mesafe

    private bool isDragging = false;
    private Transform selectedPart; // Seçilen parça
    private Vector3 initialClickOffset; // Fare ile týklama arasýndaki mesafe

    void Start()
    {
        // Arabayý gridde baþlat
        for (int i = 0; i < carParts.Count; i++)
        {
            // Parçayý en yakýn grid pozisyonuna yerleþtir
            carParts[i].position = gridManager.GetNearestGridPosition(carParts[i].position);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Fare ile araba parçalarýna týklanýp týklanmadýðýný kontrol et
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // Eðer týklanan obje bir araba parçasýysa, o parçayý seç
                for (int i = 0; i < carParts.Count; i++)
                {
                    if (hit.transform == carParts[i])
                    {
                        selectedPart = carParts[i];
                        isDragging = true;

                        // Fare ile týklama arasýndaki mesafeyi kaydet
                        initialClickOffset = selectedPart.position - hit.point;
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Sürüklemeyi býrakýnca durdur
            isDragging = false;
            selectedPart = null;
        }

        if (isDragging && selectedPart != null)
        {
            MoveCar();
        }
    }

    void MoveCar()
    {
        // Mouse'un dünya koordinatýný al
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.y = 0; // Y eksenini sýfýrlýyoruz (2D düzlemde kalmasý için)

        // Fare ile týklama arasýndaki mesafeyi de ekle
        Vector3 targetPosition = mouseWorldPosition + initialClickOffset;

        // En yakýn grid pozisyonunu bul
        targetPosition = gridManager.GetNearestGridPosition(targetPosition);

        // Seçilen araba parçasýný hareket ettir
        if (Vector3.Distance(selectedPart.position, targetPosition) > 0.1f)
        {
            selectedPart.position = targetPosition;

            // Diðer parçalarýn hareketini güncelle
            UpdateOtherPartsPositions();
        }
    }

    void UpdateOtherPartsPositions()
    {
        // Diðer parçalarýn, ilk parça ile mesafeyi koruyarak hareket etmesini saðla
        for (int i = 1; i < carParts.Count; i++)
        {
            // Önceki parçanýn pozisyonuna göre yeni pozisyonu hesapla
            Vector3 direction = carParts[i - 1].position - carParts[i].position;
            carParts[i].position = carParts[i - 1].position - direction.normalized * partDistance;

            // Grid ile uyumlu olmasýný saðla
            carParts[i].position = gridManager.GetNearestGridPosition(carParts[i].position);
        }
    }
}
