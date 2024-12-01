using System.Collections;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    public float speed = 2.0f;          // 고정 이동 속도
    public float moveRange = 5.0f;      // X축 이동 범위
    public float descendSpeed = 0.6f;   // Y축 하강 속도
    private int Hp = 13;                // 적기 체력
    private float startPositionX;       // 시작 X 위치 저장
    private bool isDescending = true;   // Y축 하강 여부
    private bool isDestroyed = false;   // 파괴 여부 플래그
    public GameObject explosionEffectPrefab;

    private AudioSource audioSource;    // 오디오 소스 컴포넌트
    private Renderer[] renderers;       // 오브젝트의 렌더러
    private Color originalColor;        // 원래 색상

    private EnemyDestructionUtility destructionUtility; // 파괴 유틸리티

    private float currentSpeed;         // 현재 X축 속도
    public float initialSpeed = 1.0f;   // 초기 이동 속도
    public float maxSpeed = 2.0f;       // 최대 속도
    public float acceleration = 0.1f;  // 가속도 (시간이 지날수록 속도 증가)

    public float minYposition = 1f;     // y축 최대하강 위치
    public GameObject[] itemPrefabs;    // 아이템 프리팹 배열
    public float dropChance = 1f;       // 아이템 드롭 확률

    Vector3 left;
    Vector3 right;
    public GameObject Charge;
    public GameObject Missile;
    GameObject lcharge;
    GameObject rcharge;
    private float minInterval = 3f;
    private float maxInterval = 4.5f;

    GameObject SkillGenerator;
    GameObject player;

    public float localTime;             // 개인 시간

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SkillGenerator = GameObject.Find("SkillGenerator");
        player = GameObject.Find("Player");
        startPositionX = transform.position.x;

        // 렌더러 가져오기 및 초기화
        renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            originalColor = renderers[0].material.color;
        }

        // 파괴 로직 유틸리티 초기화
        destructionUtility = gameObject.AddComponent<EnemyDestructionUtility>();
        destructionUtility.InitializeDestruction(renderers, originalColor, explosionEffectPrefab);

        StartCoroutine(ShootRandomly());
    }

    void Update()
    {
        localTime += Time.deltaTime;

        // Y축 하강 처리
        if (isDescending)
        {
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);

            // Y축이 minYposition 이하로 내려가면 멈춤
            if (transform.position.y <= minYposition)
            {
                transform.position = new Vector3(transform.position.x, minYposition, transform.position.z);
                isDescending = false;
            }
        }

        // X축 이동 및 PingPong
        float newX = startPositionX + Mathf.PingPong(Time.time * speed, moveRange) - moveRange / 2;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // 파괴 처리
        if (Hp <= 0 && !isDestroyed)
        {
            TriggerDestruction();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return;

        if (other.CompareTag("PlayerMissile"))
        {
            Hp -= 1;
            StartCoroutine(destructionUtility.FlashRed());
        }
        else if (other.CompareTag("Player"))
        {
            if (!player.GetComponent<PlayerController>().stun)
            {
                SkillGenerator.GetComponent<SkillGenerator>().Cooldown(3);
                TriggerDestruction();
            }
        }
        else if (other.CompareTag("SkillMissile"))
        {
            if (Random.value < dropChance)
            {
                DropItem();
            }
            SkillGenerator.GetComponent<SkillGenerator>().Cooldown(3);
            TriggerDestruction();
        }
        else if (other.CompareTag("Shield"))
        {
            TriggerDestruction();
        }
    }

    private void TriggerDestruction()
    {
        if (isDestroyed) return;

        isDestroyed = true;

        if (Random.value < dropChance)
        {
            DropItem();
        }

        GetComponent<Collider2D>().enabled = false;
        SkillGenerator.GetComponent<SkillGenerator>().Cooldown(3);
        destructionUtility.TriggerDestruction(transform);
    }

    private void DropItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject droppedItem = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
        droppedItem.GetComponent<ItemDropController>().select(randomIndex);
    }

    private IEnumerator ShootRandomly()
    {
        while (!isDestroyed)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (isDestroyed) yield break; // 파괴된 경우 코루틴 종료

            lcharge = Instantiate(Charge, left, Quaternion.identity);
            rcharge = Instantiate(Charge, right, Quaternion.identity);
            Destroy(lcharge, 1.2f);
            Destroy(rcharge, 1.2f);
            yield return new WaitForSeconds(1.2f);

            if (isDestroyed) yield break; // 파괴된 경우 코루틴 종료

            Instantiate(Missile, left, Quaternion.identity);
            Instantiate(Missile, right, Quaternion.identity);
        }
    }

}
