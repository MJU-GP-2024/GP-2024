using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//적기 mosquito 미사일
public class M8Controller : MonoBehaviour
{
    public float speed3 = 9f; // 미사일 속도

    // Start is called before the first frame update
    void Start()
    {
        // 초기 설정은 필요하지 않음
    }

    // Update is called once per frame
    void Update()
    {
        // 아래 방향으로 이동
        transform.Translate(Vector3.down * speed3 * Time.deltaTime);

        
    }
}
