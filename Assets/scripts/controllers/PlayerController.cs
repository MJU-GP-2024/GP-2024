using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject SkillGenerator;

    int Master = 0;
    int ready = 0;
    int Hp = 3;
    public bool stun = false;
    private float speed = 2.8f; // 이동속도
    private float maxSpeed = 2.8f;

    public GameObject missilePrefab; // 미사일 프리팹 참조
    public Transform missileSpawnPoint; // 미사일 발사 위치
    private float missileCooldown = 1f; // 1초 간격

    private bool isDestroyed = false; // 파괴 상태 여부
    public GameObject destructionEffectPrefab; // 일반 파괴 이펙트 프리팹
    public GameObject finalDestructionEffectPrefab; // 최종 파괴 이펙트 프리팹

    private Collider2D playerCollider; // Player의 Collider 참조

    public void PlayerStop()
    {
        this.Master = 1;
    }

    public void PlayerStart()
    {
        this.Master = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.stun && !this.isDestroyed)
        {
            if (other.gameObject.tag == "EnemyMissile" || other.gameObject.tag == "enemy")
            {
                this.decreaseHp(1);
                this.Hitted();
            }
        }
    }

    void Hitted()
    {
        this.stun = true;
        this.maxSpeed = 2.8f;
        this.speed = 1.5f;
        Invoke("Recover", 1f);
    }

    void Recover()
    {
        this.stun = false;
    }

    public void decreaseHp(int a)
    {
        this.Hp -= a;

        if (this.Hp <= 0 && !this.isDestroyed)
        {
            StartCoroutine(DestroyPlayer()); // 파괴 루틴 시작
        }
    }

    void Start()
    {
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        playerCollider = GetComponent<Collider2D>(); // Player의 Collider 참조
        StartCoroutine(FireMissilesContinuously()); // 미사일 자동 발사 시작
        this.Hp = 3;
    }

    void Update()
    {
        if (isDestroyed) return; // 파괴 상태에서는 업데이트 중지

        if (speed < maxSpeed)
        {
            speed *= 1.015f;
        }

        if (this.ready == 0 && this.transform.position.y <= -2)
        {
            this.transform.Translate(0, 0.04f, 0);
        }

        if (this.ready == 0 && this.transform.position.y >= -2)
        {
            this.ready = 1;
        }

        if (this.Master == 0)
        {
            if (this.ready == 1)
            {
                if (SkillGenerator.GetComponent<SkillGenerator>().TimeSkillActive == 0)
                {
                    if (Input.GetKey(KeyCode.W) && this.transform.position.y <= 2.0f)
                    {
                        this.transform.Translate(0, speed * Time.deltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.A) && this.transform.position.x >= -4.7)
                    {
                        this.transform.Translate(-speed * Time.deltaTime, 0, 0);
                    }
                    if (Input.GetKey(KeyCode.S) && this.transform.position.y >= -4.5)
                    {
                        this.transform.Translate(0, -speed * Time.deltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.D) && this.transform.position.x <= 4.7)
                    {
                        this.transform.Translate(speed * Time.deltaTime, 0, 0);
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.W) && this.transform.position.y <= 2.0f)
                    {
                        this.transform.Translate(0, speed * Time.unscaledDeltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.A) && this.transform.position.x >= -4.7)
                    {
                        this.transform.Translate(-speed * Time.unscaledDeltaTime, 0, 0);
                    }
                    if (Input.GetKey(KeyCode.S) && this.transform.position.y >= -4.5)
                    {
                        this.transform.Translate(0, -speed * Time.unscaledDeltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.D) && this.transform.position.x <= 4.7)
                    {
                        this.transform.Translate(speed * Time.unscaledDeltaTime, 0, 0);
                    }
                }
            }
        }
    }

    IEnumerator FireMissilesContinuously()
    {
        while (true)
        {
            if (Master == 0 && ready == 1 && !isDestroyed)
            {
                Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(missileCooldown);
        }
    }

    IEnumerator DestroyPlayer()
    {
        isDestroyed = true; // 파괴 상태로 설정
        speed = 0; // 움직임 멈춤

        // **Collider 비활성화**
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        // 1단계: 첫 번째 떨림과 이펙트
        yield return StartCoroutine(ShakeAndEffect(0.2f, destructionEffectPrefab));

        // 2단계: 1.5초 후 두 번째 떨림과 이펙트
        yield return new WaitForSeconds(1.5f);
        yield return StartCoroutine(ShakeAndEffect(0.2f, destructionEffectPrefab));

        // 3단계: 1.3초 후부터 끊김 없는 떨림과 반복 이펙트 시작
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(ContinuousShakeAndEffect(0.1f, destructionEffectPrefab));

        // 2초 후 최종 이펙트와 함께 오브젝트 삭제
        yield return new WaitForSeconds(2f);

        if (finalDestructionEffectPrefab != null)
        {
            Instantiate(finalDestructionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Player 오브젝트 삭제
    }

    IEnumerator ShakeAndEffect(float duration, GameObject effectPrefab)
    {
        float elapsedTime = 0f;

        // 떨림
        while (elapsedTime < duration)
        {
            float shakeAmount = 0.2f;
            transform.position += (Vector3)Random.insideUnitCircle * shakeAmount;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이펙트 생성
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator ContinuousShakeAndEffect(float effectInterval, GameObject effectPrefab)
    {
        while (true)
        {
            // 떨림 (끊김 없이 지속)
            float shakeAmount = 0.2f;
            transform.position += (Vector3)Random.insideUnitCircle * shakeAmount;

            // 이펙트 생성 (일정 시간마다)
            if (effectPrefab != null)
            {
                Instantiate(effectPrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(effectInterval);
        }
    }
}
