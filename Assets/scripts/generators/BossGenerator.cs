using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public GameObject Boss1prefab;
    public GameObject Boss2prefab;
    public GameObject Boss3prefab;

    Vector3 spawnPosition = new Vector3(0.0f, 6.48f, 0.0f);

    public void bossGetStage(int stage)
    {
        GameObject bossPrefab = null;

        if (stage == 1)
        {
            bossPrefab = Boss1prefab;
        }
        else if (stage == 2)
        {
            bossPrefab = Boss2prefab;
        }
        else if (stage == 3)
        {
            bossPrefab = Boss3prefab;
        }

        if (bossPrefab != null)
        {
            GameObject bossInstance = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }

    }
}
