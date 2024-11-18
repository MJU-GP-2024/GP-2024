using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject Messerschmitt;
    public GameObject F_16;


    public float spawnInterval = 1.0f;
    public float spawnY = -4.5f;

    public int pattern = 1;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1.5f, spawnInterval);
    }

    void SpawnEnemy()
    {
            int enemyType = Random.Range(0, this.pattern); // 적 유형 선택
        switch (enemyType)
        {
            case 0:
                SpawnTypeA();
                break;
            case 1:
                SpawnTypeB();
                break;
            case 2:
                SpawnTypeC();
                break;
        }
    }

    void SpawnTypeA() {
        for (int i = 0; i < 4; i++) {
            float spawnX = Random.Range(-4.0f, 4.0f);
            Vector3 spawnPosition = new Vector3(spawnX, 5.5f, 0f);
            Instantiate(Messerschmitt, spawnPosition);
        }
    }

    IEnumerator SpawnTypeB()
    {
        float spawnDelay = 0.2f;
        float spawnX = Random.Range(-3.5f, 3.5f);
        Vector3 spawnPosition = new Vector3(spawnX, 5.5f, 0f);

        for (int i = 0; i < 4; i++)
        {
            Instantiate(Messerschmitt, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay); // 0.2초 대기
        }
    }


    void SpawnTypeC() {
        // Randomly choose a side: -7 (left) or 7 (right)
                float spawnX = Random.Range(0, 2) == 0 ? -6f : 6f;
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
                Instantiate(F_16, spawnPosition, Quaternion.identity);

    }




}