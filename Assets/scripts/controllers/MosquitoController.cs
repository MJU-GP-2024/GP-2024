using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    public float initialSpeed = 3.0f;   // 초기 이동 속도
    public float maxSpeed = 6.0f;      // 최대 속도
    public float acceleration = 0.1f;   // 가속도 (시간이 지날수록 속도가 증가)

    public float moveRange = 5.0f;      // X축 이동 범위
    public float descendSpeed = 1.0f;   // Y축 하강 속도

    private float currentSpeed;         // 현재 X축 속도
    private float startPositionX;       // 시작 X 위치 저장
    private bool isDescending = true;   // Y축 하강 여부
    public float minYposition = 2.5f; //y축 최대하강 위치

    public float localTime; //개인 시간

    public void changeminY(float a) {
        this.minYposition = a;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPositionX = transform.position.x;
        currentSpeed = initialSpeed;    // 초기 속도로 설정
    }

    // Update is called once per frame
    void Update()
    {
        this.localTime += Time.deltaTime;
        // Y축 하강 처리
        if (isDescending)
        {
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);

            // Y축이 minYposition 이하로 내려가면 멈춤
            if (transform.position.y <= minYposition)
            {
                transform.position = new Vector3(transform.position.x, minYposition, transform.position.z);
                isDescending = false; // 더 이상 내려가지 않도록 설정
            }
        }

        // 시간에 따라 X축 속도 증가 (최대 속도 제한 적용)
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed); // 최대 속도 제한
        }

        // PingPong 함수와 증가된 속도를 사용하여 X축 이동
        float newX = startPositionX + Mathf.PingPong(this.localTime * currentSpeed, moveRange) - moveRange / 2;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
