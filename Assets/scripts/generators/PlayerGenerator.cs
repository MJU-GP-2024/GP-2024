using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public GameObject Player;
    public int chosen = 1; //선택 비행기. 임시 1
    // Start is called before the first frame update
    void Start()
    {
        if(chosen==1) {
            Vector3 spawnPosition = new Vector3(0, -6.5f, 0.0f);
            Instantiate(Player, spawnPosition, Quaternion.Euler(0f, 0f, 0f));
        }
        else if(chosen==2) {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
