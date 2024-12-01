using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioDirector : MonoBehaviour
{
    GameObject EnemyGen;
    GameObject BossGen;

    public AudioSource audioSource;
    public AudioClip bgm;


    private int chapter = 1; //스테이지 카운터
    float time; //스테이지 진행시간
    int bossOnStage = 0; //보스 출현상태



    public void bossDied() { //스테이지 카운터 증가
        this.chapter += 1;
        bossOnStage = 0;
        this.changePattern();
    }

    void changePattern() {
        if(chapter==2) {
            EnemyGen.GetComponent<EnemyGenerator>().patternchange(3);
        }
        else if(chapter==3) {
            EnemyGen.GetComponent<EnemyGenerator>().patternchange(5);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        this.chapter = 1;
        this.EnemyGen = GameObject.Find("EnemyGenerator");
        this.BossGen = GameObject.Find("BossGenerator");
        audioSource.clip = bgm;
        audioSource.Play();
    }


    // Update is called once per frame
    void Update()
    {
        if(bossOnStage == 0){
            this.time += Time.deltaTime;
        }
        
        if(this.time >= 18.0f){
            EnemyGen.GetComponent<EnemyGenerator>().PauseInvoke();
        }

        if(this.time >= 20.0f) {
            bossOnStage = 1; //시간 측정 중단
            this.time = 0;
            BossGen.GetComponent<BossGenerator>().bossGetStage(this.chapter);
            EnemyGen.GetComponent<EnemyGenerator>().ResumeInvoke();
        }

    }
}
