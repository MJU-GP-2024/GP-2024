using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    public float initialSpeed = 3.0f;   // 초기 이동 속도
    public float maxSpeed = 10.0f;      // 최대 속도
    public float acceleration = 0.1f;   // 가속도 (시간이 지날수록 속도가 증가)

    public float moveRange = 5.0f;      // 이동 범위

    private float currentSpeed;         // 현재 속도
    private float startPositionX;       // 시작 위치 저장

    // Start is called before the first frame update
    void Start()
    {
        startPositionX = transform.position.x;
        currentSpeed = initialSpeed;    // 초기 속도로 설정
    }

    // Update is called once per frame
    void Update()
    {
        // 시간에 따라 속도 증가 (최대 속도 제한 적용)
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed); // 최대 속도 제한
        }

        // PingPong 함수와 증가된 속도를 사용하여 이동
        float newX = startPositionX + Mathf.PingPong(Time.time * currentSpeed, moveRange) - moveRange / 2;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
