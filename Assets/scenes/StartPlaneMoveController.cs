using UnityEngine;

public class StartPlaneMoveController : MonoBehaviour
{
    float speed = 0.1f; // 초기 속도
    float maxSpeed = 10f; // 최대 속도
    float acceleration = 0.4f; // 천천히 증가할 때의 가속도
    float boostSpeed = 5f; // 갑자기 빨라질 때의 속도 증가량
    float boostTime = 6f; // 몇 초 뒤에 갑자기 빨라지는지 설정

    private float elapsedTime = 0f; // 경과 시간

    void Update()
    {
        // 경과 시간 업데이트
        elapsedTime += Time.deltaTime;

        // boostTime 이전에는 천천히 가속
        if (elapsedTime < boostTime)
        {
            if (speed < maxSpeed)
            {
                speed += acceleration * Time.deltaTime; // 서서히 속도 증가
            }
        }
        else
        {
            // boostTime이 지난 후 갑자기 속도 증가
            speed += boostSpeed;
            boostSpeed = 0f; // 추가 속도 증가 방지
        }

        // 현재 속도로 y축 이동
        transform.Translate(0, speed * Time.deltaTime, 0);
    }
}
