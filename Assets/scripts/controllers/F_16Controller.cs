using UnityEngine;

public class F_16Controller : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;


    GameObject SkillGenerator;
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

    public GameObject explosionEffectPrefab;
    private EnemyDestructionUtility destructionUtility;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
                audioSource.PlayOneShot(clip1);
                destructionUtility.TriggerDestruction(transform);
            }
        }
        else if (other.gameObject.tag == "PlayerMissile")
        {
            this.Hp -= 1;
            StartCoroutine(destructionUtility.FlashRed());
        }
        else if (other.gameObject.tag == "SkillMissile")
        {
            if (Random.value < dropChance) // Random.value는 0~1 사이의 값
            {
                DropItem();
            }
            SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
            destructionUtility.TriggerDestruction(transform);
        }
        else if (other.gameObject.tag == "Shield")
        {
            destructionUtility.TriggerDestruction(transform);
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
        audioSource = GetComponent<AudioSource>();
        
        // 공통 파괴 로직 초기화
        destructionUtility = gameObject.AddComponent<EnemyDestructionUtility>();
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Color originalColor = renderers[0].material.color;
        destructionUtility.InitializeDestruction(renderers, originalColor, explosionEffectPrefab);

        // 플레이어 위치 참조
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        this.player = GameObject.Find("Player");
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
        if (this.Hp <= 0)
        {
            if (Random.value < dropChance) // Random.value는 0~1 사이의 값
            {
                DropItem();
            }

            GetComponent<Collider2D>().enabled = false;
            SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
            destructionUtility.TriggerDestruction(transform);
        }

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
}
