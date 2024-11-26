using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    GameObject SkillGenerator;

    int Master = 0;
    int ready = 0;
    int Hp = 3;
    public bool stun = false;
    private float speed = 2.8f; //이동속도
    private float maxSpeed = 2.8f;

    public GameObject missilePrefab; // 미사일 프리팹 참조
    public Transform missileSpawnPoint; // 미사일 발사 위치
    private float missileCooldown = 1f; // 1초 간격

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
        }
    }

    void Hitted()
    {
        this.stun = true;
        this.maxSpeed=2.8f;
        this.speed=1.5f;
        Invoke("Recover", 1f);
    }
    void Recover()
    {
        this.stun = false;
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
                // 미사일 생성
                Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(missileCooldown); // 미사일 쿨다운
        }
    }
}
