using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;   // Hücrenin grid üzerindeki pozisyonu
    public GameObject carPart;         // Bu hücrenin üzerinde duran araba parçasý (varsa)

    // Hücrede araba parçasý olup olmadýðýný kontrol et
    public bool HasCarPart()
    {
        return carPart != null;
    }
}
