using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;   // H�crenin grid �zerindeki pozisyonu
    public GameObject carPart;         // Bu h�crenin �zerinde duran araba par�as� (varsa)

    // H�crede araba par�as� olup olmad���n� kontrol et
    public bool HasCarPart()
    {
        return carPart != null;
    }
}
