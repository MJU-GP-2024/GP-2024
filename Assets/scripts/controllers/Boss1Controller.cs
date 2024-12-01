using System.Collections;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    int ready = 0;
    float Hp = 1000f;
    float minSinglePatternInterval = 0.0f;
    float maxSinglePatternInterval = 1.5f;
    float minCirclePatternInterval = 7.0f;
    float maxCirclePatternInterval = 10.0f;

    private BossDeathHandler deathHandler;
    private bool isDying = false; // 보스 파괴 상태를 나타내는 플래그

    void Start()
    {
        deathHandler = GetComponent<BossDeathHandler>();
        StartCoroutine(SinglePatternShooter());
        StartCoroutine(CirclePatternShooter());
    }

    void Update()
    {
        if (ready == 0 && transform.position.y >= 5.3f)
            transform.Translate(0, -0.03f, 0);

        if (ready == 0 && transform.position.y <= 5.3f)
            ready = 1;

        if (Input.GetKeyDown(KeyCode.X)) // 임시 파괴 코드
            Hp -= 1001;

        if (Hp <= 0 && !isDying)
        {
            isDying = true; // 파괴 상태로 설정
            deathHandler.TriggerDeathSequence();
        }
    }

    IEnumerator SinglePatternShooter()
    {
        while (true)
        {
            // 보스가 파괴 중일 때는 탄환 발사 중단
            if (isDying) yield break;

            float waitTime = Random.Range(minSinglePatternInterval, maxSinglePatternInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isDying) // 파괴 상태가 아닐 때만 발사
                GetComponent<HostileWeaponProvider>().Shoot("single");
        }
    }

    IEnumerator CirclePatternShooter()
    {
        while (true)
        {
            // 보스가 파괴 중일 때는 탄환 발사 중단
            if (isDying) yield break;

            float waitTime = 4 + Random.Range(minCirclePatternInterval, maxCirclePatternInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isDying) // 파괴 상태가 아닐 때만 발사
                GetComponent<HostileWeaponProvider>().Shoot("circle");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMissile"))
        {
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            deathHandler.ApplyHitEffect(); // 피격 효과 호출
            Destroy(other.gameObject); // 총알 제거
        }
    }

    void OnMouseDown()
    {
        // 마우스 클릭 위치 가져오기
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; // Z축 값을 0으로 고정 (2D 환경)

        deathHandler.ApplyHitEffect(); // 피격 효과 호출
    }
}
