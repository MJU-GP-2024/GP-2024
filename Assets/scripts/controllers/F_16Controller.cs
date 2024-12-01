using UnityEngine;

public class F_16Controller : MonoBehaviour
{
    public float horizontalSpeed = 5f;
    public float upwardSpeed = 6f;
    public float diveSpeed = 10f;
    public float diveAcceleration = 5.0f;
    private Transform playerTrans;
    private bool isDiving = false;
    private bool isMovingHorizontally = true;
    private bool isPausedAtDivePoint = false;
    private float pauseTimer = 0f;
    private float divePoint;
    private float upwardPointRight;
    private float upwardPointLeft;
    private Vector2 diveDirection;
    public GameObject explosionEffectPrefab;

    private EnemyDestructionUtility destructionUtility;
    public int maxHits = 3;           // 파괴되기까지 필요한 공격 횟수
    private int currentHits = 0;      // 현재 공격받은 횟수

    void Start()
    {
        // 공통 파괴 로직 초기화
        destructionUtility = gameObject.AddComponent<EnemyDestructionUtility>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color originalColor = renderers[0].material.color;
        destructionUtility.InitializeDestruction(renderers, originalColor, explosionEffectPrefab);

        // 플레이어 위치 참조
        playerTrans = GameObject.Find("Player").transform;

        // 랜덤으로 다이브 포인트 및 이동 범위 설정
        divePoint = Random.Range(2f, 4.5f);
        upwardPointRight = Random.Range(2f, 4.8f);
        upwardPointLeft = Random.Range(-2f, -4.8f);

        // 스폰 위치에 따라 초기 방향 설정
        if (transform.position.x < 0)
        {
            horizontalSpeed = Mathf.Abs(horizontalSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, -90f); // 오른쪽으로 이동
        }
        else
        {
            horizontalSpeed = -Mathf.Abs(horizontalSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, 90f); // 왼쪽으로 이동
        }
    }

    void Update()
    {
        // 수평 이동 단계
        if (isMovingHorizontally)
        {
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime, Space.World);

            // 수평 이동이 끝나면 위로 이동
            if ((horizontalSpeed > 0 && transform.position.x >= upwardPointRight) ||
                (horizontalSpeed < 0 && transform.position.x <= upwardPointLeft))
            {
                isMovingHorizontally = false;
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f); // 위를 바라보도록 회전
            }
        }
        // 위로 상승 단계
        else if (!isDiving)
        {
            transform.Translate(Vector3.up * upwardSpeed * Time.deltaTime, Space.World);

            if (transform.position.y >= divePoint && playerTrans != null)
            {
                isDiving = true;
                isPausedAtDivePoint = true; // 다이브 포인트에서 멈춤
            }
        }
        // 다이브 포인트에서 1초 멈춤
        else if (isPausedAtDivePoint)
        {
            pauseTimer += Time.deltaTime;
            transform.position = new Vector2(transform.position.x, divePoint); // 다이브 포인트 고정

            // 플레이어를 바라보도록 회전
            Vector2 directionToPlayer = (playerTrans.position - transform.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle - 90f);

            if (pauseTimer >= 1f)
            {
                isPausedAtDivePoint = false;
                diveDirection = directionToPlayer; // 다이브 방향 결정
            }
        }
        // 다이브 단계
        else
        {
            diveSpeed += diveAcceleration * Time.deltaTime;
            transform.Translate(diveDirection * diveSpeed * Time.deltaTime, Space.World);
        }

        // 화면에서 벗어나면 객체 삭제
        if (transform.position.y > 5.5f || transform.position.x < -6f || transform.position.x > 6f)
        {
            Destroy(gameObject);
        }
    }

    // 마우스 클릭 시 호출
    void OnMouseDown()
    {
        currentHits++;
        StartCoroutine(destructionUtility.FlashRed());

        if (currentHits >= maxHits)
        {
            destructionUtility.TriggerDestruction(transform);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!GameObject.Find("Player").GetComponent<PlayerController>().stun)
            {
                destructionUtility.TriggerDestruction(transform);
            }
        }
        else if (other.gameObject.tag == "bullet0" || other.gameObject.tag == "SkillMissile")
        {
            destructionUtility.TriggerDestruction(transform);
        }
    }
}
