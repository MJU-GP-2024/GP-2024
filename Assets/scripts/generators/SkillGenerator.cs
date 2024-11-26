using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator : MonoBehaviour
{
    public GameObject missilePrefab;


    IEnumerator Missile() //messersc적기 일직선 4개 패턴
    {
        float spawnDelay = 0.1f;

        for (int i = 0; i < 15; i++)
        {
            float spawnX = Random.Range(-4f, 4f);
            Vector3 spawnPosition = new Vector3(spawnX, -6f, 0f);
            GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.Euler(0f, 0f, 90f));
            yield return new WaitForSeconds(spawnDelay); // 생성간격
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(Missile());
        }
    }
}
