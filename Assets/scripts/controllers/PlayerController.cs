using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int Master = 0;
    int ready = 0;
    int Hp = 3;
    public bool stun = false;

    public void PlayerStop()
    {
        this.Master = 1;
    }
    public void PlayerStart()
    {
        this.Master = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.stun)
        {
            if (other.gameObject.tag == "bullet")
            {
                this.decreaseHp(1);
                this.Hitted();
            }
            else if (other.gameObject.tag == "enemy")
            {
                this.decreaseHp(1);
                this.Hitted();
            }
        }
    }

    void Hitted()
    {
        this.stun = true;
        Invoke("Recover", 0.3f);
    }
    void Recover()
    {
        this.stun = false;
    }

    public void decreaseHp(int a)
    {
        this.Hp -= a;
        //director 호출 코드 입력
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Hp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Master == 0)
        {
            if (this.ready == 0 && this.transform.position.y <= -2)
            {
                this.transform.Translate(0, 0.04f, 0);
            }

            if (this.ready == 0 && this.transform.position.y >= -2)
            {
                this.ready = 1;
            }

            if (this.ready == 1)
            {
                if (!this.stun)
                {
                    if (Input.GetKey(KeyCode.W) && this.transform.position.y <= 2.0f)
                    {
                        this.transform.Translate(0, 0.065f, 0);
                    }
                    if (Input.GetKey(KeyCode.A) && this.transform.position.x >= -4.7)
                    {
                        this.transform.Translate(-0.065f, 0, 0);
                    }
                    if (Input.GetKey(KeyCode.S) && this.transform.position.y >= -4.5)
                    {
                        this.transform.Translate(0, -0.065f, 0);
                    }
                    if (Input.GetKey(KeyCode.D) && this.transform.position.x <= 4.7)
                    {
                        this.transform.Translate(0.065f, 0, 0);
                    }
                }







            }
























        }
    }
}
