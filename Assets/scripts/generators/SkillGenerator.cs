using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator : MonoBehaviour
{
    public GameObject missilePrefab;
    public GameObject TimeEffect;

    public float slowMotionScale = 0.35f;
    public int TimeSkillActive = 0;


    IEnumerator Missile()
    {
        float spawnDelay = 0.1f;

        for (int i = 0; i < 15; i++)
        {
            float spawnX = Random.Range(-4f, 4f);
            Vector3 spawnPosition = new Vector3(spawnX, -6f, 0f);
            GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.Euler(0f, 0f, 90f));
            yield return new WaitForSeconds(spawnDelay); // 생성간격
        }
    }

    private void TimeControl() {
        if(TimeSkillActive == 0) {
            TimeSkillActive = 1;
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            TimeEffect.GetComponent<Renderer>().enabled = true;
        }
        else if(TimeSkillActive == 1) {
            TimeSkillActive = 0;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            TimeEffect.GetComponent<Renderer>().enabled = false;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        TimeEffect = GameObject.Find("TimeEffect");
        TimeEffect.GetComponent<Renderer>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(Missile());
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            TimeControl();
        }
    }
}
