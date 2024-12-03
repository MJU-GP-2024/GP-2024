using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1Controller : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;

    GameObject DeathSound;
    int ready = 0;
    float Hp = 130f;
    float maxHp = 130f; // 최대 체력
    float minSinglePatternInterval = 0.0f; // single 무기 발사 minimum interval time
    float maxSinglePatternInterval = 1.5f; // single 무기 발사 max interval time
    // float minLinearPatternInterval = 3.0f;
    // float maxLinearPatternInterval = 5.0f;
    float minCirclePatternInterval = 5.0f; // circle 무기 발사 minimum interval time
    float maxCirclePatternInterval = 6.5f; // circle 무기 발사 max interval time

    private ScoreManager scoreManager;

    GameObject ScenarioDirector;
    private SpriteRenderer spriteRenderer; // 보스의 SpriteRenderer
    private BossDeathHandler deathHandler;
    private bool isDying = false;

    private void Start()
    {
        scoreManager = GameObject.Find("ScoreText").GetComponent<ScoreManager>();
        this.DeathSound = GameObject.Find("BossDeathSound");

        this.ScenarioDirector = GameObject.Find("ScenarioDirector");
        this.spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기
        deathHandler = GetComponent<BossDeathHandler>();
        audioSource = GetComponent<AudioSource>();

        // 체력 초기화
        this.Hp = maxHp;
        // SpriteRenderer 색상 초기화
        spriteRenderer.color = Color.white;

        // Shoot 메서드 코루틴
        StartCoroutine(SinglePatternShooter());
        // StartCoroutine(LinearPatternShooter());
        StartCoroutine(CirclePatternShooter());
    }

    private void Update()
    {
        if (this.ready == 0 && this.transform.position.y >= 5.2)
        {
            this.transform.Translate(0, -0.4f * Time.deltaTime, 0);
        }

        if (this.ready == 0 && this.transform.position.y <= 5.2)
        {
            this.ready = 1;
        }

        if (Input.GetKeyDown(KeyCode.X))
        { // 임시 파괴 코드
            this.Hp -= maxHp;
        }

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

                if (this.Hp <= 0)
                {
                    scoreManager.AddScore(1000);

                    if (!isDying)
                    {
                        ScenarioDirector.GetComponent<ScenarioDirector>().bossDied();
                        DeathSound.GetComponent<BossDeathSound>().Death();
                    }
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
                if (this.Hp <= 0)
                {
                    if (!isDying)
                    {
                        ScenarioDirector.GetComponent<ScenarioDirector>().bossDied();
                        DeathSound.GetComponent<BossDeathSound>().Death();
                    }
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
            // 무작위 대기 시간
            float waitTime = Random.Range(minSinglePatternInterval, maxSinglePatternInterval);
            yield return new WaitForSeconds(waitTime);

            if (!isDying) // 파괴 상태가 아닐 때만 발사
                GetComponent<HostileWeaponProvider>().Shoot("single");
        }
    }

    // IEnumerator LinearPatternShooter()
    // {
    //     while (true)
    //     {
    //         // 무작위 대기 시간
    //         float waitTime = Random.Range(minLinearPatternInterval, maxLinearPatternInterval);
    //         yield return new WaitForSeconds(waitTime);

    //         if (!isDying && ready == 1) {// 파괴 상태가 아닐 때만 발사
    //             GetComponent<HostileWeaponProvider>().Shoot("linear");
    //             for (int i = 0; i < 4; i++)
    //             {
    //                 audioSource.PlayOneShot(clip1);
    //                 yield return new WaitForSeconds(0.2f);
    //             }
    //         }

    //     }
    // }

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
}
