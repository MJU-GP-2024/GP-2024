using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject Messerschmitt;
    public GameObject F_16;
    public GameObject Mosqutio;
    public GameObject Lighting;
    private Coroutine LCoroutine;


    public float spawnInterval = 2.0f; //적 출현 패턴 발동 딜레이

    public int pattern = 2; //스테이지에 따른 적 출현 패턴

    public void patternchange(int a)
    {
        this.pattern = a;
    }

    void SpawnEnemy()
    {
        int enemyType = Random.Range(0, pattern); // 적 유형 선택
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
            case 3:
                StartCoroutine(SpawnTypeD());
                break;
            case 4:
                SpawnTypeE();
                break;
        }
    }


    void SpawnTypeA()
    { //messersc적기 한번에 3대 생성 패턴
        for (int i = 0; i < 4; i++)
        {
            float spawnX = Random.Range(-4.0f, 4.0f);
            Vector3 spawnPosition = new Vector3(spawnX, 5.5f, 0.0f);
            GameObject enemy = Instantiate(Messerschmitt, spawnPosition, Quaternion.Euler(0f, 0f, 180f));
            int rotateSpeed = Random.Range(0, 20);
            enemy.GetComponent<MesserschmittController>().change(rotateSpeed, 1.0f, 2.0f, 3f);


        }
    }

    IEnumerator SpawnTypeB() //messersc적기 일직선 4개 패턴
    {
        float spawnDelay = 0.2f;
        float spawnX = Random.Range(-3.5f, 3.5f);
        Vector3 spawnPosition = new Vector3(spawnX, 5.5f, 0f);
        int rotateSpeed;
        int turn = Random.Range(0, 6);
        if (turn == 0)
        {
            rotateSpeed = 0;
        }
        else
        {
            rotateSpeed = Random.Range(15, 40);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject enemy = Instantiate(Messerschmitt, spawnPosition, Quaternion.Euler(0f, 0f, 180f));
            enemy.GetComponent<MesserschmittController>().change(rotateSpeed, 1.0f, 2.0f, 5f);
            yield return new WaitForSeconds(spawnDelay); // 0.2초 대기
        }
    }

    void SpawnTypeC()
    { //f-16 패턴
      // Randomly choose a side: -7 (left) or 7 (right)
        float spawnX = Random.Range(0, 2) == 0 ? -6f : 6f;
        float spawnY = -6.5f;
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Instantiate(F_16, spawnPosition, Quaternion.identity);

    }

    IEnumerator SpawnTypeD()
    { //mosqutio 순차적 3대 생성 패턴
        float spawnDelay = 0.7f;
        for (int i = 0; i < 3; i++)
        {
            float spawnX = Random.Range(-3f, 3f);
            float spawnY = Random.Range(5.5f, 7f);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
            GameObject enemy = Instantiate(Mosqutio, spawnPosition, Quaternion.Euler(0f, 0f, 180f));
            // float y = Random.Range(0f, 2.5f);
            // enemy.GetComponent<MosquitoController>().changeminY(y);
            // Destroy(enemy, 7); //임시 제거 코드
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnTypeE()
    { //mosqutio 한번에 2~3대 생성
        int num = Random.Range(2, 4);
        float spawnX = Random.Range(-3.5f, -1f);
        // float y = Random.Range(0f, 2.5f);
        for (int i = 0; i < num; i++)
        {
            Vector3 spawnPosition = new Vector3(spawnX, 6f, 0f);
            GameObject enemy = Instantiate(Mosqutio, spawnPosition, Quaternion.Euler(0f, 0f, 180f));
            // enemy.GetComponent<MosquitoController>().changeminY(y);
            // Destroy(enemy, 7); //임시 제거 코드
            spawnX += 2.3f;
        }

    }

    IEnumerator SpawnTypeM()
    { 
        yield return new WaitForSeconds(4f);
        while (true)
        {
            float spawnX = Random.Range(-2.0f, 2.0f);
            Vector3 spawnPosition = new Vector3(spawnX, 6f, 0.0f);
            GameObject enemy = Instantiate(Lighting, spawnPosition, Quaternion.Euler(0f, 0f, 180f));

            // 랜덤 8~12초 대기 후 다시 생성
            float waitTime = Random.Range(8f, 12f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    public void StopLCoroutine()
    {
        if (LCoroutine != null)
        {
            StopCoroutine(LCoroutine); // 코루틴 정지
            LCoroutine = null;
        }
    }

    public void RestartLCoroutine()
    {
        if (LCoroutine == null)
        {
            LCoroutine = StartCoroutine(SpawnTypeM()); // 코루틴 재시작
        }
    }

    public void PauseInvoke()
    { //생성 중지
        CancelInvoke("SpawnEnemy");
        StopLCoroutine();
    }

    public void ResumeInvoke()
    { //생성 재시작
        InvokeRepeating("SpawnEnemy", 2.0f, spawnInterval);
        RestartLCoroutine();
    }


    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2.5f, spawnInterval);
        LCoroutine = StartCoroutine(SpawnTypeM());
    }

    void Update()
    {

    }







}