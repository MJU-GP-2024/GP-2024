using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    int ready = 0;
    float Hp = 1000f;

    GameObject ScenarioDirector;

    // Start is called before the first frame update
    void Start()
    {
        this.ScenarioDirector = GameObject.Find("ScenarioDirector");
    }

    // Update is called once per frame
    void Update()
    {
            if(this.ready==0 && this.transform.position.y >= 3.3) {
                this.transform.Translate(0, -0.03f, 0);
            }

            if(this.ready==0 && this.transform.position.y <= 3.3) {
                this.ready = 1;
            }





            if(Input.GetKeyDown(KeyCode.X)) { //임시 파괴 코드
                this.Hp -= 1001;
            }

            if(this.Hp <= 0) {
                ScenarioDirector.GetComponent<ScenarioDirector>().bossDied();
                Destroy(gameObject);
            }




















    
    }
}
