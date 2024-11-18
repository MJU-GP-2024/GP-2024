using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesserschmittController : MonoBehaviour
{
    public int rotateSpeed = 5;       // 회전 속도
    public float minInterval = 1.0f;    // 무기 발사 minimum interval time
    public float maxInterval = 2.0f;    // 무기 발사 max interval time
    private float speed = 5f;
    //private float randomSeed = 2f;      // 곡선의 무작위성 정도
    private Vector2 startPoint;
    private Vector2 targetPoint;        // player를 endpoint로 잡는 경우
    //private float edgePoint = 10f;      // 화면의 가장자리 좌표
    private float time = 0;

    public void change(int a, float b, float c, float d) {
        this.rotateSpeed = a;
        this.minInterval = b;
        this.maxInterval = c;
        this.speed = d;
    }

    void Start()
    {
        
        StartCoroutine(ShootRandomly());    // Shoot 메서드 코루틴

        startPoint = transform.position;
        targetPoint = GameObject.Find("Player").transform.position;
        Vector2 direction = startPoint - targetPoint;

        //float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //transform.Rotate(0, 0, -angle); // 생성시 플레이어를 바라봅니다

    
        if (transform.position.x > 0)   // 생성 위치에 따라 회전 방향이 바뀝니다
        {
            this.rotateSpeed = -this.rotateSpeed;
        }

    }

    void Update()
    {
        //time += Time.deltaTime;

        transform.Translate(0, this.speed*Time.deltaTime, 0);
        transform.Rotate(0, 0, this.rotateSpeed * Time.deltaTime);

        // 화면에서 벗어나면 객체 삭제
        if (transform.position.y < -6.0f || transform.position.x < -6f || transform.position.x > 6f)
        {
            Destroy(gameObject);
        }
    }

    void Flip() //현재 미구현
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

