using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    private int Choose;
    public GameObject explode1;
    public GameObject explode2;
    private Vector3 location;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            if(Choose==0) {
                GameObject effect = Instantiate(explode1, location, Quaternion.identity);
                Destroy(effect, 0.4f);
            }
            else{
                GameObject effect = Instantiate(explode2, location, Quaternion.identity);
                Destroy(effect, 0.4f);
            }
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Boss") {
            if(Choose==0) {
                GameObject effect = Instantiate(explode1, location, Quaternion.identity);
                Destroy(effect, 0.4f);
            }
            else{
                GameObject effect = Instantiate(explode2, location, Quaternion.identity);
                Destroy(effect, 0.4f);
            }
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Menemy") {
            if(Choose==0) {
                GameObject effect = Instantiate(explode1, location, Quaternion.identity);
                Destroy(effect, 0.4f);
            }
            else{
                GameObject effect = Instantiate(explode2, location, Quaternion.identity);
                Destroy(effect, 0.4f);
            }
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Choose = Random.Range(0,2);
    }

    // Update is called once per frame
    void Update()
    {
        location = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);

        transform.Translate(0.5f, 0, 0);

        if(this.transform.position.y > 5.3f) {
            Destroy(gameObject);
        }
    }
}
