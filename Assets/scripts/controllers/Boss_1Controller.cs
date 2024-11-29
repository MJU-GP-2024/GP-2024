using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1Controller : MonoBehaviour
{
    int ready = 0;
    float Hp = 100f;
    float minSinglePatternInterval = 0.0f; // single 무기 발사 minimum interval time
    float maxSinglePatternInterval = 1.5f; // single 무기 발사 max interval time
    float minCirclePatternInterval = 7.0f; // circle 무기 발사 minimum interval time
    float maxCirclePatternInterval = 10.0f; // circle무기 발사 max interval time

    GameObject ScenarioDirector;

    private void OnTriggerEnter2D(Collider2D other)
    {if(this.ready == 1){
        if (other.CompareTag("PlayerMissile")) // 플레이어 미사일과 충돌했을 경우
        {   this.Hp -= 1; // 체력 감소

        }
        else if(other.gameObject.tag == "SkillMissile") {
            this.Hp -= 4;
        }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.ScenarioDirector = GameObject.Find("ScenarioDirector");

        // Shoot 메서드 코루틴
        StartCoroutine(SinglePatternShooter());
        StartCoroutine(CirclePatternShooter());
    }

    // Update is called once per frame
    void Update()
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
        { //임시 파괴 코드
            this.Hp -= 1001;
        }

        if (this.Hp <= 0)
        {
            ScenarioDirector.GetComponent<ScenarioDirector>().bossDied();
            Destroy(gameObject);
        }
    }

    IEnumerator SinglePatternShooter()
    {
        while (true)
        {
            // 무작위 대기 시간
            float waitTime = Random.Range(minSinglePatternInterval, maxSinglePatternInterval);
            yield return new WaitForSeconds(waitTime);

            // shoot() 메서드 실행
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

            // shoot() 메서드 실행
            GetComponent<HostileWeaponProvider>().Shoot("circle");
        }
    }
}
