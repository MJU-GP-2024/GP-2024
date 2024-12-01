using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public GameObject Boss_1prefab;
    public GameObject Boss_2prefab;
    public GameObject Boss_3prefab;

    Vector3 spawnPosition = new Vector3(0.0f, 7.7f, 0.0f);

    public void bossGetStage(int a)
    {
        if (a == 1)
        {
            GameObject boss_1 = Instantiate(Boss_1prefab, spawnPosition, Quaternion.identity);
        }
        else if (a == 2)
        {
            GameObject boss_2 = Instantiate(Boss_2prefab, spawnPosition, Quaternion.identity);
        }
        else if (a == 3)
        {
            GameObject boss_3 = Instantiate(Boss_3prefab, spawnPosition, Quaternion.identity);
        }

        if (bossPrefab != null)
        {
            GameObject bossInstance = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }

    }
}
