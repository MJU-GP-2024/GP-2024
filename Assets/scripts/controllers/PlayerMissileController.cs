using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
    GameObject SkillGenerator;
    private float speed = 8.5f;

    public void SpeedChange(float a) {
        this.speed = a;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {   
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Boss") {
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Menemy") {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SkillGenerator = GameObject.Find("SkillGenerator");
    }

    // Update is called once per frame
    void Update()
    {

        if(SkillGenerator.GetComponent<SkillGenerator>().TimeSkillActive == 0) {
            transform.Translate(0, speed * Time.deltaTime, 0);
            }
        else {
            transform.Translate(0, speed * Time.unscaledDeltaTime, 0);
        }

        if(transform.position.y >= 5.1f) {
            Destroy(gameObject);
        }
    }
}
