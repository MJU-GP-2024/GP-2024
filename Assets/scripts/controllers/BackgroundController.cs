using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//test

public class BackgroundController : MonoBehaviour
{
    int Master = 0;
    float speed = 0.8f;
    int child = 0;
    GameObject BackgroundGenerator;

    public void BackgroundStop()
    {
        this.Master = 1;
    }
    public void BackgroundStart()
    {
        this.Master = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.BackgroundGenerator = GameObject.Find("BackgroundGenerator");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Master == 0)
        {
            this.transform.Translate(0, -this.speed * Time.deltaTime, 0);

            if (this.transform.position.y <= -20 && this.child == 0)
            {
                this.BackgroundGenerator.GetComponent<BackgroundGenerator>().Gen_NewBackground();
                this.child = 1;
            }

            if (this.transform.position.y < -32)
            {
                Destroy(gameObject);
            }


        }
    }
}
