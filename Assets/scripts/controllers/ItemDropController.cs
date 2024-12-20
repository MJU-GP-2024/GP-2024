using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    GameObject Player;
    public GameObject[] itemPrefabs; // 아이템 프리팹 배열 (Enemy.cs에서 복사)
    public float fallSpeed = 2.3f;   // 떨어지는 속도
    public float itemChangeInterval = 1.0f; // 아이템 변경 간격
    private float changeTimer = 0f;  // 아이템 변경 타이머
    private float scaleFactor = 1f;
    private ScoreManager scoreManager;

    private int currentIndex;        // 현재 아이템의 인덱스

    public void select(int a)
    {
        this.currentIndex = a;
    }

    void Start()
    {
        this.Player = GameObject.Find("Player");
        scoreManager = GameObject.Find("ScoreText").GetComponent<ScoreManager>();
        // // 초기 아이템 설정
        // currentIndex = Random.Range(0, itemPrefabs.Length);
        // UpdateItemAppearance();
    }

    void Update()
    {
        if (transform.position.y <= -5.5)
        {
            Destroy(gameObject);
        }
        // 아래로 이동
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 아이템 변경 간격 타이머
        changeTimer += Time.deltaTime;
        if (changeTimer >= itemChangeInterval)
        {
            changeTimer = 0f;
            ChangeItem();
        }
    }

    private void ChangeItem()
    {
        // itemPrefabs[currentIndex].SetActive(false);
        // 다음 아이템으로 변경
        currentIndex = (currentIndex + 1) % itemPrefabs.Length; // 순환 변경
        Debug.Log($"Item changed to index: {currentIndex}");
        UpdateItemAppearance();
    }

    // private void UpdateItemAppearance()
    // {
    //     // itemPrefabs[currentIndex].SetActive(true);
    //     // 아이템의 프리팹교체
    //     for (int i = 0; i < itemPrefabs.Length; i++)
    //     {
    //         itemPrefabs[i].SetActive(i == currentIndex); // 현재 아이템만 활성화
    //     }
    // }



    private void UpdateItemAppearance()
    {
        // 현재 오브젝트의 Sprite Renderer 가져오기
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && itemPrefabs[currentIndex] != null)
        {

            // itemPrefabs의 Sprite를 가져와 설정
            spriteRenderer.sprite = itemPrefabs[currentIndex].GetComponent<SpriteRenderer>().sprite;
            spriteRenderer.color = itemPrefabs[currentIndex].GetComponent<SpriteRenderer>().color;
            scaleFactor = 1f;
            switch (currentIndex)
            {
                case 0:
                    scaleFactor = 0.08f;
                    break;
                case 1:
                    scaleFactor = 0.5f;
                    break;
                case 2:
                    scaleFactor = 0.47f;
                    break;
                case 3:
                    scaleFactor = 0.4f;
                    break;
            }
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            scaleFactor = 1f;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 아이템 획득
        if (other.gameObject.tag == "Player")
        {
            ApplyItemEffect();
            scoreManager.AddScore(50);
            Destroy(gameObject);
        }
    }

    private void ApplyItemEffect()
    {
        // 아이템 효과 적용 (현재 선택된 아이템 기준)
        switch (currentIndex)
        {
            case 0:
                Debug.Log("체력 회복!");
                Player.GetComponent<PlayerController>().HpUp();
                break;
            case 1:
                Debug.Log("파워 증가!");
                Player.GetComponent<PlayerController>().PowerUp();
                break;
            case 2:
                Debug.Log("무적 상태!");
                Player.GetComponent<PlayerController>().Shield();
                break;
            case 3:
                Debug.Log("속도 증가!");
                Player.GetComponent<PlayerController>().SpeedUp();
                break;
        }
    }

}
