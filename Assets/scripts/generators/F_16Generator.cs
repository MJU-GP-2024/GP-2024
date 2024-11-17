using UnityEngine;

public class F_16Generator : MonoBehaviour
{
    public GameObject F_16Prefab;
    public float spawnInterval = 2.0f;
    public float spawnY = -4.5f;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        // Randomly choose a side: -7 (left) or 7 (right)
        float spawnX = Random.Range(0, 2) == 0 ? -6f : 6f;
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Instantiate(F_16Prefab, spawnPosition, Quaternion.identity);
    }
}
