using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class buttonController : MonoBehaviour
{
    private Color originalColor; // 원래 색상을 저장
    private Renderer objectRenderer;

    void Start()
    {
        // Renderer를 가져오고 원래 색상을 저장
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        // 3초 뒤에 색상 변경 코루틴 시작
        Invoke("StartColorChangeCoroutine", 7f);
    }

    void OnMouseEnter()
    {
        // 마우스가 버튼 위로 올라가면 색상을 변경
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.red; // 색상 변경 (빨간색)
        }
    }

    void OnMouseExit()
    {
        // 마우스가 버튼을 벗어나면 원래 색상으로 복구
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor;
        }
    }

    void OnMouseDown()
    {
        // 마우스 클릭 시 gamescene 로드
        SceneManager.LoadScene("GameScene");
    }

    void StartColorChangeCoroutine()
    {
        // 코루틴 시작
        StartCoroutine(ChangeColorPeriodically());
    }

    IEnumerator ChangeColorPeriodically()
    {
        while (true)
        {
            if (objectRenderer != null)
            {
                // 현재 색상이 원래 색상이면 빨간색으로 변경
                if (objectRenderer.material.color == originalColor)
                {
                    objectRenderer.material.color = Color.red;
                }
                else // 그렇지 않으면 원래 색상으로 변경
                {
                    objectRenderer.material.color = originalColor;
                }
            }

            // 1초 대기
            yield return new WaitForSeconds(1f);
        }
    }
}
