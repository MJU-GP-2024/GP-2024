using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    public GameObject BackgroundPrefab;
    float flipBackground = -1;

    public void Gen_NewBackground()
    {
        GameObject newbackground = Instantiate(BackgroundPrefab) as GameObject;
        newbackground.transform.position = new Vector3(0, 31.6f, 0);
        newbackground.transform.localScale = new Vector3(1.5f, 1.5f * flipBackground, 1);
        flipBackground *= -1;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject newbackground = Instantiate(BackgroundPrefab) as GameObject;
        newbackground.transform.position = new Vector3(0, 12, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
