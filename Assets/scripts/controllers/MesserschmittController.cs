using System.Collections;
using UnityEngine;

public class MesserschmittController : MonoBehaviour
{
    public int rotateSpeed = 5;        // 회전 속도
    public float minInterval = 1.0f;  // 무기 발사 최소 간격
    public float maxInterval = 2.0f;  // 무기 발사 최대 간격
    private float speed = 5f;         // 이동 속도
    public int maxHits = 3;           // 파괴되기까지 필요한 공격 횟수
    private int currentHits = 0;      // 현재 공격받은 횟수

    public GameObject explosionEffectPrefab; 
    private Renderer[] renderers;     // 오브젝트의 렌더러
    private Color originalColor;      // 원래 색상

    private EnemyDestructionUtility destructionUtility;
    private GameObject player;        // 플레이어 참조

    // **동적 파라미터 변경 메서드**
    public void change(int newRotateSpeed, float newMinInterval, float newMaxInterval, float newSpeed)
    {
        this.rotateSpeed = newRotateSpeed; // 회전 속도 설정
        this.minInterval = newMinInterval; // 무기 발사 최소 간격 설정
        this.maxInterval = newMaxInterval; // 무기 발사 최대 간격 설정
        this.speed = newSpeed;             // 이동 속도 설정
    }

    void Start()
    {
        // 플레이어 참조
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
        StartCoroutine(ShootRandomly());
    }

    void Update()
    {
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

    // 무작위 무기 발사
    IEnumerator ShootRandomly()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // 무기 발사 호출 (HostileWeaponProvider 사용)
            GetComponent<HostileWeaponProvider>().Shoot("single");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 시 처리
        if (other.gameObject.tag == "Player")
        {
            if (!GameObject.Find("Player").GetComponent<PlayerController>().stun)
            {
                destructionUtility.TriggerDestruction(transform);
            }
        }
        // 총알이나 스킬과 충돌 시 처리
        else if (other.gameObject.tag == "bullet0" || other.gameObject.tag == "SkillMissile")
        {
            destructionUtility.TriggerDestruction(transform);
        }
    }
}
