using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour
{
    int Master = 0;
    int ready = 0;
    float Hp = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.Master==0) {
            if(this.ready==0 && this.transform.position.y >= 3.3) {
                this.transform.Translate(0, -0.03f, 0);
            }

            if(this.ready==0 && this.transform.position.y <= 3.3) {
                this.ready = 1;
            }






            if(this.Hp <= 0) {
                
            }




















        }
    }
}
