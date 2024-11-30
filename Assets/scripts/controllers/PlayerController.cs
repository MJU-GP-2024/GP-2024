using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    GameObject SkillGenerator;
    public GameObject PlayerMissile;

    int Master = 0;
    int ready = 0;
    int Hp = 4;
    int Firemode = 1;
    public bool stun = false;
    private float speed = 3.5f; //이동속도
    private float maxSpeed = 3.5f;
    public float blinkDuration = 2f; // 총 깜빡임 지속 시간
    public float blinkInterval = 0.2f; // 깜빡이는 간격
    private Renderer[] renderers;

    Transform missileSpawnPoint; // 미사일 발사 위치
    private float missileCooldown = 0.22f; // 1초 간격

    public void PlayerStop()
    {
        this.Master = 1;
    }
    public void PlayerStart()
    {
        this.Master = 0;
    }

    public void PowerUp() {
        if(this.Firemode < 3) {
            Firemode += 1;
        }
    }
    public void HpUp() {
        if(Hp<4) {
            Hp += 1;
        }
    }
    public void Shield() {
        stun = true;
        Invoke("Recover", 3f);
    }
    public void SpeedUp() {
        maxSpeed = 5f;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.stun)
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
                this.decreaseHp(2);
                this.Hitted();
            }
        }
    }

    void Hitted()
    {
        if(!stun){        
            this.stun = true;
            this.maxSpeed=3.5f;
            this.speed=1.5f;
            if(Firemode>1){
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
        //director 호출 코드 입력
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        StartCoroutine(FireMissilesContinuously()); // 미사일 자동 발사 시작
        this.Hp = 3;
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(speed<maxSpeed) {
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
                if(SkillGenerator.GetComponent<SkillGenerator>().TimeSkillActive == 0) {
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
                else{
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
        while (true) // 무한 루프
        {
            if (Master == 0 && ready == 1) // 발사 조건 확인
            {
                
                if(Firemode == 1) {
                // 미사일 생성
                Instantiate(PlayerMissile, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                }
                else if(Firemode == 2) {
                    Instantiate(PlayerMissile, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
                    Instantiate(PlayerMissile, transform.position + new Vector3(0.3f, 0.1f, 0), Quaternion.Euler(0, 0, -17f));
                    Instantiate(PlayerMissile, transform.position + new Vector3(-0.3f, 0.1f, 0), Quaternion.Euler(0, 0, 17f));

                }
                else if(Firemode == 3) {
                    Instantiate(PlayerMissile, transform.position + new Vector3(0.15f, 0.1f, 0), Quaternion.identity);
                    Instantiate(PlayerMissile, transform.position + new Vector3(-0.15f, 0.1f, 0), Quaternion.identity);
                    Instantiate(PlayerMissile, transform.position + new Vector3(0.3f, 0.1f, 0), Quaternion.Euler(0, 0, -17f));
                    Instantiate(PlayerMissile, transform.position + new Vector3(-0.3f, 0.1f, 0), Quaternion.Euler(0, 0, 17f));
                
                }
            }
                yield return new WaitForSecondsRealtime(missileCooldown); // 미사일 쿨다운
            
                
        }
    }
}
