using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    public GameObject BackgroundPrefab;

    public void Gen_NewBackground() {
        GameObject newbackground = Instantiate(BackgroundPrefab) as GameObject;
        newbackground.transform.position = new Vector3 (-0.5f, 20.6f, 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject newbackground = Instantiate(BackgroundPrefab) as GameObject;
        newbackground.transform.position = new Vector3 (-0.5f, 10.6f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
