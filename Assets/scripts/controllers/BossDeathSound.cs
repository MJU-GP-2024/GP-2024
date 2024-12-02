using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ex1;
    public AudioClip ex2;
    public AudioClip ex3;
    public AudioClip ex4;
    public AudioClip ex5;

    void StopAllProcesses() {
        StopAllCoroutines();
    }

    IEnumerator BossDeath() {
        for(int i=0; i < 90; i++) {
            int t = Random.Range(0,5);
            if(t==0) {
                audioSource.PlayOneShot(ex1);
            }
            else if(t==1) {
                audioSource.PlayOneShot(ex2);
            }
            else if(t==2) {
                audioSource.PlayOneShot(ex3);
            }
            else if(t==3) {
                audioSource.PlayOneShot(ex4);
            }
            else if(t==4) {
                audioSource.PlayOneShot(ex5);
            }
            yield return new WaitForSeconds(0.07f);
        }
        // StopAllProcesses();
    }

    public void Death() {
        StartCoroutine(BossDeath());
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
