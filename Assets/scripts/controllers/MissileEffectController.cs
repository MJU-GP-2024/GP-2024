using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEffectController : MonoBehaviour
{
    private float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -3.5f) {
            transform.Translate(0, 11f * Time.deltaTime, 0);
        }
        else if(transform.position.y <= -2.8f) {
            transform.Translate(0, 0.7f * Time.deltaTime, 0);
        }
        else {
            speed *= 1.1f;
            transform.Translate(0, speed * Time.deltaTime, 0);
        }

        if(transform.position.y >= 10f) {
            Destroy(gameObject);
        }
    }
}
