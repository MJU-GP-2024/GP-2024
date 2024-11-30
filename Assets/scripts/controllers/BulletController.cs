using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!this.player.GetComponent<PlayerController>().stun)
            {
                Destroy(gameObject);
            }
        }
        if (other.gameObject.tag == "Shield") {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -9 || transform.position.x > 9 || transform.position.y < -5.5 || transform.position.y > 5.5)
        {
            Destroy(gameObject);
        }



    }
}
