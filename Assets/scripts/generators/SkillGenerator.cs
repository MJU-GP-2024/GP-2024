using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;

    public GameObject missilePrefab;
    public GameObject TimeEffect;
    public GameObject MissileEffect;

    public float slowMotionScale = 0.35f;
    public int TimeSkillActive = 0;

    public float missileSkillCharged = 30f;
    public float timeSkillCharged = 9f;

    public void Cooldown(float a)
    {
        if ((missileSkillCharged + a) <= 20f)
        {
            missileSkillCharged += a;
        }
        else
        {
            missileSkillCharged = 20f;
        }
    }

    public float GetMissileSkillCharged()
    {
        return this.missileSkillCharged;
    }

    public float GetTimeSkillCharged()
    {
        return this.timeSkillCharged;
    }

    IEnumerator Missile()
    {
        float spawnDelay = 0.1f;
        audioSource.PlayOneShot(clip1);
        Instantiate(MissileEffect, new Vector3(0, -8, 0), Quaternion.identity);
        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < 15; i++)
        {
            float spawnX = Random.Range(-4f, 4f);
            Vector3 spawnPosition = new Vector3(spawnX, -6f, 0f);
            GameObject missile = Instantiate(missilePrefab, spawnPosition, Quaternion.Euler(0f, 0f, 90f));
            yield return new WaitForSecondsRealtime(spawnDelay); // 생성간격
        }
    }

    private void TimeControl()
    {
        if (TimeSkillActive == 0)
        {
            TimeSkillActive = 1;
            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            TimeEffect.GetComponent<Renderer>().enabled = true;
        }
        else if (TimeSkillActive == 1)
        {
            TimeSkillActive = 2;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            TimeEffect.GetComponent<Renderer>().enabled = false;
            StartCoroutine(Cool());
        }
    }

    IEnumerator Cool()
    {
        yield return new WaitForSeconds(1.5f);
        TimeSkillActive = 0;

    }



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        TimeEffect = GameObject.Find("TimeEffect");
        TimeEffect.GetComponent<Renderer>().enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (TimeSkillActive == 1)
        {
            timeSkillCharged -= 3 * Time.unscaledDeltaTime;
            if (timeSkillCharged <= 0f)
            {
                timeSkillCharged = 0f;
                TimeControl();
            }
        }
        else if (TimeSkillActive == 0)
        {
            if (timeSkillCharged < 20f)
            {
                timeSkillCharged += Time.deltaTime;
            }
            else
            {
                timeSkillCharged = 20f;
            }
        }

        if (missileSkillCharged < 20f)
        {
            missileSkillCharged += Time.deltaTime;
        }
        else if (missileSkillCharged >= 20f)
        {
            missileSkillCharged = 20f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (missileSkillCharged >= 20f)
            {
                StartCoroutine(Missile());
                missileSkillCharged = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Missile());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TimeControl();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TimeControl();
        }
    }
}
