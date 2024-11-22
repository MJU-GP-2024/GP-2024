using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    int Master = 0;
    int ready = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireMissilesContinuously()); // 미사일 자동 발사 시작
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Master == 0)
        {
            if (this.ready == 0 && this.transform.position.y <= -2)
            {
                this.transform.Translate(0, 0.04f, 0);
            }

            if (this.ready == 0 && this.transform.position.y >= -2)
            {
                this.ready = 1;
            }

            if (this.ready == 1)
            {
                if (Input.GetKey(KeyCode.W) && this.transform.position.y <= 2.0f)
                {
                    this.transform.Translate(0, 0.065f, 0);
                }
                if (Input.GetKey(KeyCode.A) && this.transform.position.x >= -4.7)
                {
                    this.transform.Translate(-0.065f, 0, 0);
                }
                if (Input.GetKey(KeyCode.S) && this.transform.position.y >= -4.5)
                {
                    this.transform.Translate(0, -0.065f, 0);
                }
                if (Input.GetKey(KeyCode.D) && this.transform.position.x <= 4.7)
                {
                    this.transform.Translate(0.065f, 0, 0);
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
