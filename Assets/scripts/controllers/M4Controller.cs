using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class M4Controller : MonoBehaviour
{
    public float speed = 10f; // 미사일 속도

    // Update is called once per frame
    void Update()
    {
        // 위로 이동
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 화면을 벗어나면 삭제
        if (transform.position.y > 10f)
        {
            // Destroy(gameObject);
        }
    }
}
