using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlaneController : MonoBehaviour
{
    public float speed = 4.0f;          // 고정 이동 속도
    public float moveRange = 5.0f;      // X축 이동 범위
    public float descendSpeed = 1.0f;   // Y축 하강 속도

    private float startPositionX;       // 시작 X 위치 저장
    private bool isDescending = true;   // Y축 하강 여부

    public GameObject missilePrefab;    // 적 미사일 프리팹
    public Transform missileSpawnPoint; // 미사일 발사 위치
    private float missileCooldown = 1f; // 1초 간격



    // Start is called before the first frame update
    void Start()
    {
        startPositionX = transform.position.x;
        StartCoroutine(FireMissile()); // 미사일 자동 발사 시작
    }

    // Update is called once per frame
    void Update()
    {
        // Y축 하강 처리
        if (isDescending)
        {
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);

            // Y축이 4 이하로 내려가면 멈춤
            if (transform.position.y <= 4.0f)
            {
                transform.position = new Vector3(transform.position.x, 4.0f, transform.position.z);
                isDescending = false; // 더 이상 내려가지 않도록 설정
            }
        }

        // PingPong 함수와 고정된 속도를 사용하여 X축 이동
        float newX = startPositionX + Mathf.PingPong(Time.time * speed, moveRange) - moveRange / 2;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);


    }

    // 미사일 발사
    IEnumerator FireMissile()
    {
        while(true)
        {
            Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(missileCooldown);

        }
    }    

}
