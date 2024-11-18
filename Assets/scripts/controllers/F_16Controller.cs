using UnityEngine;

public class F_16Controller : MonoBehaviour
{
    public float horizontalSpeed = 5f;
    public float upwardSpeed = 6f;
    public float diveSpeed = 10f;
    public float diveAcceleration = 5.0f;
    private Transform player;
    private bool isDiving = false;
    private bool isMovingHorizontally = true;
    private bool isPausedAtDivePoint = false;
    private float pauseTimer = 0f;
    private float divePoint;
    private float upwardPointRight;
    private float upwardPointLeft;
    private Vector2 diveDirection;
    // 화면 하단에서 수평으로 비행하다 위로 올라와서 플레이어를 1초 동안 바라보다가 dive
    void Start()
    {
        player = GameObject.Find("Player").transform;

        // 2와 4.5 사이에서 랜덤하게 다이브 포인트 설정
        divePoint = Random.Range(2f, 4.5f);
        upwardPointRight = Random.Range(2f, 4.8f);
        upwardPointLeft = Random.Range(-2f, -4.8f);

        // 스폰 위치에 따라 바라보는 방향 설정
        if (transform.position.x < 0)
        {
            // 왼쪽에서 스폰되면 오른쪽으로 이동
            horizontalSpeed = Mathf.Abs(horizontalSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, -90f); // 오른쪽을 바라보도록 회전
        }
        else
        {
            // 오른쪽에서 스폰되면 왼쪽으로 이동
            horizontalSpeed = -Mathf.Abs(horizontalSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, 90f); // 왼쪽을 바라보도록 회전
        }
    }

    void Update()
    {

        if (isMovingHorizontally)
        {
            // 수평 이동 (world space)
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime, Space.World);

            // 스폰된 위치의 각 반대편 끝에 다다르면 각각의 upwardPoint 설정
            if ((horizontalSpeed > 0 && transform.position.x >= upwardPointRight) ||
                (horizontalSpeed < 0 && transform.position.x <= upwardPointLeft))
            {
                isMovingHorizontally = false;
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // 위를 바라보도록 회전
            }
        }
        else if (!isDiving)
        {
            // dive point까지 위로 이동 (world space)
            transform.Translate(Vector3.up * upwardSpeed * Time.deltaTime, Space.World);

            if (transform.position.y >= divePoint && player != null)
            {
                isDiving = true;
                isPausedAtDivePoint = true; // dive point에서 pause
            }
        }
        else if (isPausedAtDivePoint)
        {
            // dive point에서 1초 플레이어를 바라보며 pause
            pauseTimer += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, divePoint); // Keep the enemy at the dive point

            // pause 했을 때 플레이어를 바라보도록 회전
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle - 90f); // Adjust image rotation to face player

            if (pauseTimer >= 1f)
            {
                isPausedAtDivePoint = false;
                // 플레이어를 향한 최종 direction 결정
                diveDirection = directionToPlayer;
            }
        }
        else
        {
            // Dive in the direction calculated after the pause (world space)
            diveSpeed += diveAcceleration * Time.deltaTime;
            transform.Translate(diveDirection * diveSpeed * Time.deltaTime, Space.World);
        }

        // 화면에서 벗어나면 객체 삭제
        if (transform.position.y > 5.5f || transform.position.x < -6f || transform.position.x > 6f)
        {
            Destroy(gameObject);
        }
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // Player에게 부딪히면 객체 삭제 (Tag 비교 방식)
    //    if (collision.CompareTag("Player"))
    //    {
    //        Destroy(gameObject); // Destroy the enemy on collision with the player
    //    }
    //}
}