using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject Messerschmitt;
    public GameObject F_16;


    public float spawnInterval = 2.0f; //적 출현 패턴 발동 딜레이
    public float spawnY = -6.5f;

    public int pattern = 3; //스테이지에 따른 적 출현 패턴

    void SpawnEnemy()
    {
            int enemyType = Random.Range(0, 3); // 적 유형 선택
            Debug.Log("enemyType: " + enemyType);
        switch (enemyType)
        {

            case 0:
                SpawnTypeA();
                break;
            case 1:
                StartCoroutine(SpawnTypeB());
                break;
            case 2:
                SpawnTypeC();
                break;
        }
    }


    void SpawnTypeA() {
        for (int i = 0; i < 4; i++) {
            float spawnX = Random.Range(-4.0f, 4.0f);
            Vector3 spawnPosition = new Vector3(spawnX, 5.5f, 0.0f);
            GameObject enemy = Instantiate(Messerschmitt, spawnPosition, Quaternion.Euler(0f, 0f, 180f));
            int rotateSpeed = Random.Range(0, 20);
            enemy.GetComponent<MesserschmittController>().change(rotateSpeed, 1.0f, 2.0f, 3f);


        }
    }

    IEnumerator SpawnTypeB()
    {
        float spawnDelay = 0.2f;
        float spawnX = Random.Range(-3.5f, 3.5f);
        Vector3 spawnPosition = new Vector3(spawnX, 5.5f, 0f);
        int rotateSpeed;
        int turn = Random.Range(0,6);
        if(turn==0) {
            rotateSpeed = 0;
        }
        else{
            rotateSpeed = Random.Range(15, 40);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject enemy = Instantiate(Messerschmitt, spawnPosition, Quaternion.Euler(0f, 0f, 180f));
            enemy.GetComponent<MesserschmittController>().change(rotateSpeed, 1.0f, 2.0f, 5f);
            yield return new WaitForSeconds(spawnDelay); // 0.2초 대기
        }
    }


    void SpawnTypeC() {
        // Randomly choose a side: -7 (left) or 7 (right)
                float spawnX = Random.Range(0, 2) == 0 ? -6f : 6f;
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
                Instantiate(F_16, spawnPosition, Quaternion.identity);

    }

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1.5f, spawnInterval);
    }

    void Update() {

    }







}