using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class replayButtonController : MonoBehaviour
{
    private Color originalColor; // 원래 색상을 저장
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // SpriteRenderer를 가져오고 원래 색상을 저장
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // 색상 변경 코루틴 바로 시작
        StartCoroutine(StartColorChange());
    }

    void OnMouseEnter()
    {
        // 마우스가 버튼 위로 올라가면 색상을 노란색으로 변경
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.yellow; // 색상 변경 (노란색)
        }
    }

    void OnMouseExit()
    {
        // 마우스가 버튼을 벗어나면 원래 색상으로 복구
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    void OnMouseDown()
    {
        // 마우스 클릭 시 GameScene 로드
        SceneManager.LoadScene("GameScene");
    }

    IEnumerator StartColorChange()
    {
        while (true)
        {
            if (spriteRenderer != null)
            {
                // 현재 색상 변경
                if (spriteRenderer.color == originalColor)
                {
                    spriteRenderer.color = Color.yellow; // 색상 변경 
                }
                else
                {
                    spriteRenderer.color = originalColor;
                }
            }

            // 1초 대기
            yield return new WaitForSeconds(1f);
        }
    }
}
