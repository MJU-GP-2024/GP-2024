using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesserschmittController : MonoBehaviour
{
    public float rotateSpeed = 5; // 회전 속도
    public float minInterval = 1.0f; // 무기 발사 minimum interval time
    public float maxInterval = 2.0f; // 무기 발사 max interval time
    private float speed = 5f;
    private Vector2 startPoint;
    private Vector2 targetPoint; // player를 endpoint로 잡는 경우
    private GameObject player;
    // private ItemDropController itemDropController;
    
    private int Hp = 1;

    public void change(float rotateSpeed, float minInterval, float maxInterval, float speed)
    {
        this.rotateSpeed = rotateSpeed;
        this.minInterval = minInterval;
        this.maxInterval = maxInterval;
        this.speed = speed;
    }

    void DropItem()
    {
        // 적기의 현재 위치에서 아이템 드랍 요청
        //itemDropController.RequestItemDrop(transform.position);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                //DropItem();
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.tag == "PlayerMissile")
        {
            this.Hp -= 1;
        }
        else if (other.gameObject.tag == "SkillMissile")
        {
            //DropItem();
            Destroy(gameObject);
        }
    }

    void Start()
    {
        this.player = GameObject.Find("Player");
        StartCoroutine(ShootRandomly());    // Shoot 메서드 코루틴

        startPoint = transform.position;
        targetPoint = GameObject.Find("Player").transform.position;
        Vector2 direction = startPoint - targetPoint;

        //float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //transform.Rotate(0, 0, -angle); // 생성시 플레이어를 바라봅니다

        if (transform.position.x > 0)   // 생성 위치에 따라 회전 방향이 바뀝니다
        {
            this.rotateSpeed = -this.rotateSpeed;
        }
        //itemDropController = FindObjectOfType<ItemDropController>();
    }

    void Update()
    {
        if(this.Hp <= 0) {
            Destroy(gameObject);
        }

        transform.Translate(0, this.speed * Time.deltaTime, 0);
        transform.Rotate(0, 0, this.rotateSpeed * Time.deltaTime);

        // 화면에서 벗어나면 객체 삭제
        if (transform.position.y < -6.0f || transform.position.x < -6f || transform.position.x > 6f)
        {
            Destroy(gameObject);
        }
    }

    void Flip() //현재 미구현
    {
        for (int i = 0; i < 180; i++)
        {
            transform.Rotate(1, 0, 0);
        }
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
}