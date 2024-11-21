using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    int ready = 0;
    float Hp = 1000f;
    public float minCirclePatternInterval = 1.0f; // circle 무기 발사 minimum interval time
    public float maxCirclePatternInterval = 2.0f; // circle무기 발사 max interval time

    GameObject ScenarioDirector;

    // Start is called before the first frame update
    void Start()
    {
        this.ScenarioDirector = GameObject.Find("ScenarioDirector");

        // Shoot 메서드 코루틴
        StartCoroutine(CirclePatternShooter());
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ready == 0 && this.transform.position.y >= 3.3)
        {
            this.transform.Translate(0, -0.03f, 0);
        }

        if (this.ready == 0 && this.transform.position.y <= 3.3)
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

    IEnumerator CirclePatternShooter()
    {
        while (true)
        {
            // 무작위 대기 시간
            float waitTime = Random.Range(minCirclePatternInterval, maxCirclePatternInterval);
            yield return new WaitForSeconds(waitTime);

            // shoot() 메서드 실행
            GetComponent<HostileWeaponProvider>().Shoot("circle");
        }
    }
}
