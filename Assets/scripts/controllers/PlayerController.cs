using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;
    public AudioClip clip2;

    GameObject SkillGenerator;
    public GameObject PlayerMissile;
    public GameObject ShieldPrefab;

    int Master = 0;
    int ready = 0;
    int Hp = 3;
    int Firemode = 1;
    public bool stun = false;
    private float speed = 3.5f; // 이동속도
    private float maxSpeed = 3.5f;
    public float blinkDuration = 2f; // 총 깜빡임 지속 시간
    public float blinkInterval = 0.2f; // 깜빡이는 간격
    private Renderer[] renderers;

    Transform missileSpawnPoint; // 미사일 발사 위치
    private float missileCooldown = 0.22f; // 1초 간격

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

    public void PowerUp()
    {
        audioSource.PlayOneShot(clip1);
        if (this.Firemode < 3)
        {
            Firemode += 1;
        }
    }
    public void HpUp()
    {
        audioSource.PlayOneShot(clip1);
        if (Hp < 4)
        {
            Hp += 1;
        }
    }
    public void Shield()
    {
        audioSource.PlayOneShot(clip1);
        stun = true;
        GameObject shield = Instantiate(ShieldPrefab);
        Destroy(shield, 3f);
        Invoke("Recover", 3f);
    }
    public void SpeedUp()
    {
        audioSource.PlayOneShot(clip1);
        maxSpeed = 5f;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.stun && !this.isDestroyed)
        {
            if (other.gameObject.tag == "EnemyMissile")
            {
                this.decreaseHp(1);
                this.Hitted();
            }
            else if (other.gameObject.tag == "enemy")
            {
                this.decreaseHp(1);
                this.Hitted();
            }
            else if (other.gameObject.tag == "Menemy")
            {
                this.decreaseHp(1);
                this.Hitted();
            }
        }
    }

    void Hitted()
    {
        if (!stun)
        {
            this.stun = true;
            this.maxSpeed = 3.5f;
            this.speed = 1.5f;
            if (Firemode > 1)
            {
                this.Firemode -= 1;
            }
            StartCoroutine(BlinkCoroutine());
            Invoke("Recover", 2f);
        }
    }

    void Recover()
    {
        this.stun = false;
    }

    private IEnumerator BlinkCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < blinkDuration)
        {
            foreach (Renderer rend in renderers)
            {
                rend.enabled = !rend.enabled; // 렌더러 토글
            }

            yield return new WaitForSecondsRealtime(blinkInterval); // 깜빡임 간격
            elapsedTime += blinkInterval;
        }

        // 깜빡임 종료 후 모든 렌더러 활성화
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }
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
        audioSource = GetComponent<AudioSource>();
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        playerCollider = GetComponent<Collider2D>(); // Player의 Collider 참조
        StartCoroutine(FireMissilesContinuously()); // 미사일 자동 발사 시작
        this.Hp = 3;
        renderers = GetComponentsInChildren<Renderer>();
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
            this.transform.Translate(0, 3f * Time.deltaTime, 0);
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
                    if (Input.GetKey(KeyCode.UpArrow) && this.transform.position.y <= 2.0f)
                    {
                        this.transform.Translate(0, speed * Time.deltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x >= -4.7)
                    {
                        this.transform.Translate(-speed * Time.deltaTime, 0, 0);
                    }
                    if (Input.GetKey(KeyCode.DownArrow) && this.transform.position.y >= -4.5)
                    {
                        this.transform.Translate(0, -speed * Time.deltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x <= 4.7)
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
                    if (Input.GetKey(KeyCode.UpArrow) && this.transform.position.y <= 2.0f)
                    {
                        this.transform.Translate(0, speed * Time.unscaledDeltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x >= -4.7)
                    {
                        this.transform.Translate(-speed * Time.unscaledDeltaTime, 0, 0);
                    }
                    if (Input.GetKey(KeyCode.DownArrow) && this.transform.position.y >= -4.5)
                    {
                        this.transform.Translate(0, -speed * Time.unscaledDeltaTime, 0);
                    }
                    if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x <= 4.7)
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

                if (Firemode == 1)
                {
                    Instantiate(PlayerMissile, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                }
                else if (Firemode == 2)
                {
                    Instantiate(PlayerMissile, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                    Instantiate(PlayerMissile, transform.position + new Vector3(0.3f, 0.1f, 0), Quaternion.Euler(0, 0, -17f));
                    Instantiate(PlayerMissile, transform.position + new Vector3(-0.3f, 0.1f, 0), Quaternion.Euler(0, 0, 17f));

                }
                else if (Firemode == 3)
                {
                    Instantiate(PlayerMissile, transform.position + new Vector3(0.15f, 0.1f, 0), Quaternion.identity);
                    Instantiate(PlayerMissile, transform.position + new Vector3(-0.15f, 0.1f, 0), Quaternion.identity);
                    Instantiate(PlayerMissile, transform.position + new Vector3(0.3f, 0.1f, 0), Quaternion.Euler(0, 0, -17f));
                    Instantiate(PlayerMissile, transform.position + new Vector3(-0.3f, 0.1f, 0), Quaternion.Euler(0, 0, 17f));

                }
                audioSource.PlayOneShot(clip2);
                         }
            yield return new WaitForSecondsRealtime(missileCooldown);
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

    public int getHp()
    {
        return this.Hp;
    }
}
