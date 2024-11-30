using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingMissileController : MonoBehaviour
{
    GameObject Player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!this.Player.GetComponent<PlayerController>().stun)
            {
                Destroy(gameObject);
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -6f * Time.deltaTime, 0);

        if(transform.position.y <= -6) {
            Destroy(gameObject);
        }
    }
}
