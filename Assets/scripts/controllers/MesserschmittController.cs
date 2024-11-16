using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messerschmitt : MonoBehaviour
{
    public float rotateSpeed = 0.14f;   // 회전 속도
    public float minInterval = 1.0f;    // 무기 발사 minimum interval time
    public float maxInterval = 2.0f;    // 무기 발사 max interval time
    private float speed = 0.03f;
    private float randomSeed = 2f;      // 곡선의 무작위성 정도
    private Vector2 startPoint;
    private Vector2 targetPoint;        // player를 endpoint로 잡는 경우
    private float edgePoint = 10f;      // 화면의 가장자리 좌표
    private float time = 0;

    void Start()
    {
        StartCoroutine(ShootRandomly());    // Shoot 메서드 코루틴

        startPoint = transform.position;
        targetPoint = GameObject.Find("Player").transform.position;
        Vector2 direction = startPoint - targetPoint;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, -angle); // 생성시 플레이어를 바라봅니다

        if (transform.position.x > 0)   // 생성 위치에 따라 회전 방향이 바뀝니다
        {
            rotateSpeed = -rotateSpeed;
        }
    }

    void Update()
    {
        time += Time.deltaTime;

        transform.Translate(0, speed, 0);
        transform.Rotate(0, 0, rotateSpeed);
    }

    void Flip()
    {
        for (int i = 0; i < 180; i++)
        {
            transform.Rotate(1, 0, 0);
        }
    }

    IEnumerator ShootRandomly()
    {
        while (true)
        {
            // 무작위 대기 시간
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // shoot() 메서드 실행
            Shoot();
        }
    }

    void Shoot()
    {
        Debug.Log("shoot by messerschmitt. elapsed time since creation:" + time);
    }
}
