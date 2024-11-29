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
    private int Hp = 2;
    GameObject player;
    public GameObject[] itemPrefabs; // 아이템 프리팹 배열
    public float dropChance = 0.25f;  // 아이템 드롭 확률

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == "PlayerMissile")
        {
            this.Hp -= 1;
        }
        else if (other.gameObject.tag == "SkillMissile")
        {
            if (Random.value < dropChance) // Random.value는 0~1 사이의 값
            {
            DropItem();
            }
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        // 랜덤 아이템 선택
        int randomIndex = Random.Range(0, 4);

        // 아이템 생성
        GameObject droppedItem = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
        droppedItem.GetComponent<ItemDropController>().select(randomIndex);
    }


    // 화면 하단에서 수평으로 비행하다 위로 올라와서 플레이어를 1초 동안 바라보다가 dive
    void Start()
    {
        this.player = GameObject.Find("Player");
        playerTrans = GameObject.Find("Player").transform;

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
        if(this.Hp <= 0) {
            if (Random.value < dropChance) // Random.value는 0~1 사이의 값
            {
            DropItem();
            }
            Destroy(gameObject);
        }

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

            if (transform.position.y >= divePoint && playerTrans != null)
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
            Vector2 directionToPlayer = (playerTrans.position - transform.position).normalized;
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


}