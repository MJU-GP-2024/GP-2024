using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoController : MonoBehaviour
{
    public float speed = 4.0f;          // 고정 이동 속도
    public float moveRange = 5.0f;      // X축 이동 범위
    public float descendSpeed = 1.0f;   // Y축 하강 속도
    public int health = 5;             // 적기 체력 (3번 맞으면 파괴)

    private float startPositionX;       // 시작 X 위치 저장
    private bool isDescending = true;   // Y축 하강 여부

    public GameObject missilePrefab;    // 적 미사일 프리팹
    public Transform missileSpawnPoint; // 미사일 발사 위치
    private float missileCooldown = 1f; // 1초 간격

    private AudioSource audioSource;     // 오디오 소스 컴포넌트

    public float initialSpeed = 3.0f;   // 초기 이동 속도
    public float maxSpeed = 6.0f;      // 최대 속도
    public float acceleration = 0.1f;   // 가속도 (시간이 지날수록 속도가 증가)

    private float currentSpeed;         // 현재 X축 속도
    public float minYposition = 2.5f; // y축 최대하강 위치

    public float localTime; // 개인 시간

    GameObject player;

    public void changeminY(float a)
    {
        this.minYposition = a;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        this.player = GameObject.Find("Player");
        startPositionX = transform.position.x;
        StartCoroutine(FireMissile()); // 미사일 자동 발사 시작
    }

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
            transform.position -= new Vector3(0, descendSpeed * Time.deltaTime, 0);

            // Y축이 4 이하로 내려가면 멈춤
            if (transform.position.y <= 4.0f)
            {
                transform.position = new Vector3(transform.position.x, 4.0f, transform.position.z);
                isDescending = false; // 더 이상 내려가지 않도록 설정
            }
        }

        // PingPong 함수와 고정된 속도를 사용하여 X축 이동
        // float newX = startPositionX + Mathf.PingPong(Time.time * speed, moveRange) - moveRange / 2;
        // PingPong 함수와 증가된 속도를 사용하여 X축 이동
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    IEnumerator FireMissile()
    {
        while (true)
        {
            Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(missileCooldown);
        }
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMissile")) // 플레이어 미사일과 충돌했을 경우
        {

            health--; // 체력 감소
            Debug.Log("미사일 맞음.");

            Destroy(other.gameObject); // 미사일 삭제

            // 충돌 이펙트 생성
            // 충돌 사운드 재생
            audioSource.Play();

            // GetComponent<ParticleSystem>().Play();


            if (health <= 0)
            {
                Destroy(gameObject); // 체력이 0 이하가 되면 적기 삭제
            }
        }

        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == "bullet0")
        {
            Destroy(gameObject);
        }
    }
}
