using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    GameObject SkillGenerator;

    public float speed = 4.0f;          // 고정 이동 속도
    public float moveRange = 5.0f;      // X축 이동 범위
    public float descendSpeed = 3.0f;   // Y축 하강 속도
    private int Hp = 3;             // 적기 체력 (3번 맞으면 파괴)
    public float minInterval = 2.0f; // 무기 발사 minimum interval time
    public float maxInterval = 3.0f; // 무기 발사 max interval time

    private float startPositionX;       // 시작 X 위치 저장
    private bool isDescending = true;   // Y축 하강 여부

    private AudioSource audioSource;     // 오디오 소스 컴포넌트

    public float initialSpeed = 3.0f;   // 초기 이동 속도
    public float maxSpeed = 6.0f;      // 최대 속도
    public float acceleration = 0.1f;   // 가속도 (시간이 지날수록 속도가 증가)

    private float currentSpeed;         // 현재 X축 속도
    public float minYposition = 2.5f; // y축 최대하강 위치
    public GameObject[] itemPrefabs; // 아이템 프리팹 배열
    public float dropChance = 0.25f;  // 아이템 드롭 확률

    public float localTime; // 개인 시간

    GameObject player;

    private void DropItem()
    {
        // 랜덤 아이템 선택
        int randomIndex = Random.Range(0, 4);

        // 아이템 생성
        GameObject droppedItem = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);
        droppedItem.GetComponent<ItemDropController>().select(randomIndex);
    }

    public void changeminY(float a)
    {
        this.minYposition = a;
    }

    void Start()
    {
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        audioSource = GetComponent<AudioSource>();

        this.player = GameObject.Find("Player");
        startPositionX = transform.position.x;
        StartCoroutine(ShootRandomly()); 
    }

    IEnumerator ShootRandomly()
    {
        while (true)
        {
            // 무작위 대기 시간
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // shoot() 메서드 실행
            GetComponent<HostileWeaponProvider>().Shoot("single");
        }
    }

    void Update()
    {
        if (Hp <= 0)
            {
                if (Random.value < dropChance) // Random.value는 0~1 사이의 값
                {
                    DropItem();
                }
                SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
                Destroy(gameObject); // 체력이 0 이하가 되면 적기 삭제
            }
        this.localTime += Time.deltaTime;

        if(transform.position.y < -6f) {
            Destroy(gameObject);
        }

        // Y축 하강 처리
        if (isDescending)
        {
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);

            // // Y축이 minYposition 이하로 내려가면 멈춤
            // if (transform.position.y <= minYposition)
            // {
            //     transform.position = new Vector3(transform.position.x, minYposition, transform.position.z);
            //     isDescending = false; // 더 이상 내려가지 않도록 설정
            // }
        }

        // 시간에 따라 X축 속도 증가 (최대 속도 제한 적용)
        if (currentSpeed < maxSpeed)
        {
            transform.position -= new Vector3(currentSpeed * this.localTime, 0, 0);

        }

        // PingPong 함수와 고정된 속도를 사용하여 X축 이동
        float newX = startPositionX + Mathf.PingPong(Time.time * speed, moveRange) - moveRange / 2;
        // PingPong 함수와 증가된 속도를 사용하여 X축 이동
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    // IEnumerator FireMissile()
    // {
    //     while (true)
    //     {
    //         Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
    //         yield return new WaitForSeconds(missileCooldown);
    //     }
    // }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMissile")) // 플레이어 미사일과 충돌했을 경우
        {
            this.Hp -= 1; // 체력 감소

            // 충돌 이펙트 생성
            // 충돌 사운드 재생
            audioSource.Play();
        }

        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == "SkillMissile")
        {
            if (Random.value < dropChance) // Random.value는 0~1 사이의 값
            {
            DropItem();
        }
            SkillGenerator.GetComponent<SkillGenerator>().Cooldown(1);
            Destroy(gameObject);
        }
    }
}
