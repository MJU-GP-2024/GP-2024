using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    public GameObject Boss1prefab;
    public GameObject Boss2prefab;
    public GameObject Boss3prefab;

    Vector3 spawnPosition = new Vector3(0.0f, 6.48f, 0.0f);

    public void bossGetStage(int a) {
        if(a == 1) {
            GameObject boss = Instantiate(Boss1prefab, spawnPosition, Quaternion.identity);
        }
        else if(a == 2) { //임시로 동일보스 생성
            GameObject boss = Instantiate(Boss1prefab, spawnPosition, Quaternion.identity);
        }
        else if(a == 3) { //임시로 동일보스 생성
            GameObject boss = Instantiate(Boss1prefab, spawnPosition, Quaternion.identity);
        }
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}