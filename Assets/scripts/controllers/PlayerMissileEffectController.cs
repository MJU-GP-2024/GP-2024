using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileEffectController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.3f);
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
