using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int Master = 0;
    int ready = 0;

    public void PlayerStop() {
        this.Master = 1;
    }
    public void PlayerStart() {
        this.Master = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.Master == 0) {
            if(this.ready==0 && this.transform.position.y<=-2) {
                this.transform.Translate(0, 0.04f, 0);
            }

            if(this.ready==0 && this.transform.position.y>=-2) {
                this.ready = 1;
            }

            if(this.ready==1) {
                if(Input.GetKey(KeyCode.W) && this.transform.position.y <= 2.0f) {
                    this.transform.Translate(0, 0.065f, 0);
                }
                if(Input.GetKey(KeyCode.A) && this.transform.position.x >= -4.7) {
                    this.transform.Translate(-0.065f, 0, 0);
                }
                if(Input.GetKey(KeyCode.S) && this.transform.position.y >= -4.5) {
                    this.transform.Translate(0, -0.065f, 0);
                }
                if(Input.GetKey(KeyCode.D) && this.transform.position.x <= 4.7) {
                    this.transform.Translate(0.065f, 0, 0);
                }







            }
























        }
    }
}
