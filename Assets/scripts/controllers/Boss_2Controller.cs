using System.Collections;
using UnityEngine;

public class Boss_2Controller : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;

    GameObject DeathSound;

    int ready = 0;
    float Hp = 160f;
    float maxHp = 160f; // 최대 체력
    float minSinglePatternInterval = 0.0f; // single 무기 발사 minimum interval time
    float maxSinglePatternInterval = 1.5f; // single 무기 발사 max interval time
    float minCirclePatternInterval = 7.0f; // circle 무기 발사 minimum interval time
    float maxCirclePatternInterval = 8.5f; // circle 무기 발사 max interval time
    float minLinearPatternInterval = 3.0f;
    float maxLinearPatternInterval = 5.0f;

    private BossDeathHandler deathHandler;
    private bool isDying = false; // 보스 파괴 상태를 나타내는 플래그
    private SpriteRenderer spriteRenderer; // 보스의 SpriteRenderer
    private ScoreManager scoreManager;

    GameObject ScenarioDirector;

    private void Start()
    {
        scoreManager = GameObject.Find("ScoreText").GetComponent<ScoreManager>();
        this.DeathSound = GameObject.Find("BossDeathSound");

        deathHandler = GetComponent<BossDeathHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기
        ScenarioDirector = GameObject.Find("ScenarioDirector");

        audioSource = GetComponent<AudioSource>();

        // 체력 초기화
        this.Hp = maxHp;
        // SpriteRenderer 색상 초기화
        spriteRenderer.color = Color.white;


        StartCoroutine(SinglePatternShooter());
        StartCoroutine(CirclePatternShooter());
        StartCoroutine(LinearPatternShooter());
    }

    private void Update()
    {
        if (ready == 0 && transform.position.y >= 4.7)
        {
            transform.Translate(0, -0.6f * Time.deltaTime, 0);
        }

        if (ready == 0 && transform.position.y <= 4.7)
        {
            ready = 1;
        }

        if (Input.GetKeyDown(KeyCode.X)) // 임시 파괴 코드
            Hp -= maxHp;

        UpdateColorByHealth(); // 체력에 따라 색상 업데이트

    }

    private void UpdateColorByHealth()
    {
        if (spriteRenderer != null && !deathHandler.IsHit) // deathHandler의 isHit 상태 확인
        {
            float healthRatio = Mathf.Clamp01(Hp / maxHp);
            float greenBlueIntensity = Mathf.Lerp(0.15f, 1.0f, healthRatio);
            spriteRenderer.color = new Color(1.0f, greenBlueIntensity, greenBlueIntensity, 1.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 보스가 자리(ready == 1)에 도달했을 때만 공격을 받음
        if (ready == 1)
        {
            if (other.CompareTag("PlayerMissile")) // 플레이어 미사일과 충돌했을 경우
            {
                Hp -= 1; // 체력 감소
                deathHandler.ApplyHitEffect(); // 피격 효과 호출
                if (Hp <= 0 && !isDying)
                {
                    scoreManager.AddScore(1000);
                    ScenarioDirector.GetComponent<ScenarioDirector>().bossDied();
                    DeathSound.GetComponent<BossDeathSound>().Death();
                    isDying = true; // 파괴 상태로 설정
                    deathHandler.TriggerDeathSequence();
                }
                else
                {
                    UpdateColorByHealth(); // 체력에 따라 색상 업데이트
                }
            }
            else if (other.CompareTag("SkillMissile"))
            {
                Hp -= 2.5f; // 체력 감소
                deathHandler.ApplyHitEffect(); // 피격 효과 호출
                if (Hp <= 0 && !isDying)
                {
                    ScenarioDirector.GetComponent<ScenarioDirector>().bossDied();
                    DeathSound.GetComponent<BossDeathSound>().Death();
                    isDying = true; // 파괴 상태로 설정
                    deathHandler.TriggerDeathSequence();
                }
                else
                {
                    UpdateColorByHealth(); // 체력에 따라 색상 업데이트
                }
            }
        }
    }


    IEnumerator SinglePatternShooter()
    {
        while (true)
        {
            if (isDying) yield break; // 보스가 파괴 중일 때는 발사 중단

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
            // 무작위 대기 시간
            float waitTime = 4 + Random.Range(minCirclePatternInterval, maxCirclePatternInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isDying && ready == 1)
            {// 파괴 상태가 아닐 때만 발사
                GetComponent<HostileWeaponProvider>().Shoot("circle");
                yield return new WaitForSeconds(0.2f);
                for (int i = 0; i < 2; i++)
                {
                    audioSource.PlayOneShot(clip2);
                    yield return new WaitForSeconds(0.45f);
                }
            }

        }
    }

    IEnumerator LinearPatternShooter()
    {
        while (true)
        {
            // 무작위 대기 시간
            float waitTime = Random.Range(minLinearPatternInterval, maxLinearPatternInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isDying && ready == 1)
            {// 파괴 상태가 아닐 때만 발사
                GetComponent<HostileWeaponProvider>().Shoot("linear");
                for (int i = 0; i < 4; i++)
                {
                    audioSource.PlayOneShot(clip1);
                    yield return new WaitForSeconds(0.2f);
                }
            }

        }
    }
}
