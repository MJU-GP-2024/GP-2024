using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
    public GameObject Hit1;
    public GameObject Hit2;
    public GameObject Hit3;
    private int Effect;
    GameObject SkillGenerator;
    private float speed = 8.5f;

    public void SpeedChange(float a) {
        this.speed = a;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {   
            if(Effect <=4 ) {
                Instantiate(Hit1, transform.position, Quaternion.identity);
            }
            else if(Effect <= 6) {
                Instantiate(Hit2, transform.position, Quaternion.identity);
            }
            else {
                Instantiate(Hit3, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Boss") {
            if(Effect <=4 ) {
                Instantiate(Hit1, transform.position, Quaternion.identity);
            }
            else if(Effect <= 6) {
                Instantiate(Hit2, transform.position, Quaternion.identity);
            }
            else {
                Instantiate(Hit3, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Menemy") {
            if(Effect <=4 ) {
                Instantiate(Hit1, transform.position, Quaternion.identity);
            }
            else if(Effect <= 6) {
                Instantiate(Hit2, transform.position, Quaternion.identity);
            }
            else {
                Instantiate(Hit3, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SkillGenerator = GameObject.Find("SkillGenerator");
        Effect = Random.Range(0, 8);
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
