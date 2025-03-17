using UnityEngine;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public GridManager gridManager;  // Grid sistemine eri�im
    public List<Transform> carParts; // Araban�n par�alar� (0: �n, 1: Orta, 2: Arka)
    public float moveSpeed = 5f; // Hareket h�z�
    public float partDistance = 1.5f; // Par�alar aras�ndaki mesafe

    private bool isDragging = false;
    private Transform selectedPart; // Se�ilen par�a
    private Vector3 initialClickOffset; // Fare ile t�klama aras�ndaki mesafe

    void Start()
    {
        // Arabay� gridde ba�lat
        for (int i = 0; i < carParts.Count; i++)
        {
            // Par�ay� en yak�n grid pozisyonuna yerle�tir
            carParts[i].position = gridManager.GetNearestGridPosition(carParts[i].position);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Fare ile araba par�alar�na t�klan�p t�klanmad���n� kontrol et
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                // E�er t�klanan obje bir araba par�as�ysa, o par�ay� se�
                for (int i = 0; i < carParts.Count; i++)
                {
                    if (hit.transform == carParts[i])
                    {
                        selectedPart = carParts[i];
                        isDragging = true;

                        // Fare ile t�klama aras�ndaki mesafeyi kaydet
                        initialClickOffset = selectedPart.position - hit.point;
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // S�r�klemeyi b�rak�nca durdur
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
        // Mouse'un d�nya koordinat�n� al
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.y = 0; // Y eksenini s�f�rl�yoruz (2D d�zlemde kalmas� i�in)

        // Fare ile t�klama aras�ndaki mesafeyi de ekle
        Vector3 targetPosition = mouseWorldPosition + initialClickOffset;

        // En yak�n grid pozisyonunu bul
        targetPosition = gridManager.GetNearestGridPosition(targetPosition);

        // Se�ilen araba par�as�n� hareket ettir
        if (Vector3.Distance(selectedPart.position, targetPosition) > 0.1f)
        {
            selectedPart.position = targetPosition;

            // Di�er par�alar�n hareketini g�ncelle
            UpdateOtherPartsPositions();
        }
    }

    void UpdateOtherPartsPositions()
    {
        // Di�er par�alar�n, ilk par�a ile mesafeyi koruyarak hareket etmesini sa�la
        for (int i = 1; i < carParts.Count; i++)
        {
            // �nceki par�an�n pozisyonuna g�re yeni pozisyonu hesapla
            Vector3 direction = carParts[i - 1].position - carParts[i].position;
            carParts[i].position = carParts[i - 1].position - direction.normalized * partDistance;

            // Grid ile uyumlu olmas�n� sa�la
            carParts[i].position = gridManager.GetNearestGridPosition(carParts[i].position);
        }
    }
}
