using UnityEngine;
using System.Collections.Generic;

public class StickmanManager : MonoBehaviour
{
    public GameObject stickmanPrefab;
    public int numberOfStickmen = 8;
    public float stickmanSpacing = 3f;
    public static StickmanManager Instance;
    private List<GameObject> stickmen = new List<GameObject>();

    private List<GameObject> spawnedStickmen = new List<GameObject>();


    void Awake()
    {
        Instance = this;
    }

    public void SpawnStickmen(Vector3 spawnPosition, Quaternion rotation)
    {
        for (int i = 0; i < numberOfStickmen; i++)
        {
            Vector3 offset = rotation * new Vector3(0, 0, -(stickmanSpacing * (i + 1)));
            GameObject stickman = Instantiate(stickmanPrefab, spawnPosition + offset, rotation);
            spawnedStickmen.Add(stickman);
        }
    }

    public void RemoveAllStickmen()
    {
        foreach (GameObject stickman in spawnedStickmen)
        {
            Destroy(stickman);
        }
        spawnedStickmen.Clear();
    }
}
