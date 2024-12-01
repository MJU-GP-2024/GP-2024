using System.Collections;
using UnityEngine;

public class MesserschmittController : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip clip1;
    GameObject SkillGenerator;
    public float rotateSpeed = 5; // 회전 속도
    public float minInterval = 1.0f; // 무기 발사 minimum interval time
    public float maxInterval = 2.0f; // 무기 발사 max interval time
    private float speed = 5f;
    private Vector2 startPoint;
    private Vector2 targetPoint; // player를 endpoint로 잡는 경우
    private GameObject player;

    public GameObject explosionEffectPrefab;
    private Renderer[] renderers;     // 오브젝트의 렌더러
    private Color originalColor;      // 원래 색상

    private EnemyDestructionUtility destructionUtility;
    private bool isDestroyed = false; // 파괴 여부 플래그

    private int Hp = 1;
    public GameObject[] itemPrefabs; // 아이템 프리팹 배열
    public float dropChance = 0.25f;  // 아이템 드롭 확률

    public void change(float rotateSpeed, float minInterval, float maxInterval, float speed)
    {
        this.rotateSpeed = rotateSpeed;
        this.minInterval = minInterval;
        this.maxInterval = maxInterval;
        this.speed = speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return; // 이미 파괴된 경우 실행하지 않음

        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
                audioSource.PlayOneShot(clip1);
                TriggerDestruction(); // 파괴 처리
            }
        }
        else if (other.gameObject.tag == "PlayerMissile")
        {
            this.Hp -= 1;
            StartCoroutine(destructionUtility.FlashRed());

            if (this.Hp <= 0 && !isDestroyed)
            {
                audioSource.PlayOneShot(clip1);
                TriggerDestruction(); // 파괴 처리
            }
        }
        else if (other.gameObject.tag == "SkillMissile")
        {
            if (Random.value < dropChance) // Random.value는 0~1 사이의 값
            {
                DropItem();
            }
            SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
            audioSource.PlayOneShot(clip1);
            TriggerDestruction(); // 파괴 처리
        }
        else if (other.gameObject.tag == "Shield")
        {
            audioSource.PlayOneShot(clip1);
            TriggerDestruction(); // 파괴 처리
        }
    }

    private void TriggerDestruction()
    {
        if (isDestroyed) return; // 이미 파괴된 경우 실행하지 않음

        isDestroyed = true; // 파괴 상태 설정

        if (Random.value < dropChance) // 아이템 드롭 확률
        {
            DropItem();
        }

        GetComponent<Collider2D>().enabled = false;
        SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
        destructionUtility.TriggerDestruction(transform);
    }

    private void DropItem()
    {
        // 랜덤 아이템 선택
        int randomIndex = Random.Range(0, 4);

        // 아이템 생성
        GameObject droppedItem = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
        droppedItem.GetComponent<ItemDropController>().select(randomIndex);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        this.player = GameObject.Find("Player");

        // 렌더러 가져오기 및 색상 초기화
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }

        // 파괴 로직 유틸리티 초기화
        destructionUtility = gameObject.AddComponent<EnemyDestructionUtility>();
        destructionUtility.InitializeDestruction(renderers, originalColor, explosionEffectPrefab);

        // 무작위 무기 발사 시작
        StartCoroutine(ShootRandomly());    // Shoot 메서드 코루틴

        startPoint = transform.position;
        targetPoint = GameObject.Find("Player").transform.position;
        Vector2 direction = startPoint - targetPoint;

        if (transform.position.x > 0)   // 생성 위치에 따라 회전 방향이 바뀝니다
        {
            this.rotateSpeed = -this.rotateSpeed;
        }
    }

    private void Update()
    {
        if (this.Hp <= 0 && !isDestroyed)
        {
            TriggerDestruction(); // 파괴 처리
        }

        // 이동 처리
        transform.Translate(0, speed * Time.deltaTime, 0);
        // 회전 처리
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // 화면 밖으로 벗어났을 경우 삭제
        if (transform.position.y < -6.0f || transform.position.x < -6f || transform.position.x > 6f)
        {
            Destroy(gameObject);
        }
    }

    // 무작위 무기 발사
    private IEnumerator ShootRandomly()
    {
        while (!isDestroyed)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (isDestroyed) yield break; // 파괴된 경우 코루틴 종료

            GetComponent<HostileWeaponProvider>().Shoot("single");
        }
    }

}
