using System.Collections;
using UnityEngine;

public class EnemyDestructionUtility : MonoBehaviour
{
    private Renderer[] renderers;    // 적기의 모든 Renderer
    private Color originalColor;     // 적기의 원래 색상
    private bool isDestroyed = false; // 적기가 파괴되었는지 여부
    private GameObject explosionEffectPrefab; // 폭발 이펙트 프리팹

    // 초기화 메서드
    public void InitializeDestruction(Renderer[] renderers, Color originalColor, GameObject explosionPrefab)
    {
        this.renderers = renderers;
        this.originalColor = originalColor;
        this.explosionEffectPrefab = explosionPrefab;
    }

    // 적기가 붉게 변하는 효과
    public IEnumerator FlashRed()
    {
        foreach (Renderer r in renderers)
        {
            r.material.color = Color.red; // 붉게 변환
        }

        yield return new WaitForSeconds(0.1f); // 0.1초 대기

        if (!isDestroyed)
        {
            foreach (Renderer r in renderers)
            {
                r.material.color = originalColor; // 원래 색상 복원
            }
        }
    }

    // 적기가 파괴 후 추락하는 효과
    public IEnumerator FallAndDestroy(Transform transform)
    {
        float fallSpeed = 0f;            // 초기 추락 속도
        float acceleration = 9.8f;      // 추락 가속도
        float swingDuration = 0.2f;     // 좌우 큰 흔들림 지속 시간
        float elapsedTime = 0f;         // 경과 시간
        float rotationSpeed = 100f;     // 파괴 후 회전 속도
        Vector3 fallDirection = Vector3.down; // 기본 추락 방향 (아래로)

        // 좌우 큰 흔들림 단계
        while (elapsedTime < swingDuration)
        {
            float horizontalSwing = Mathf.Sin(elapsedTime * 50f) * 4f; // 좌우로 크게 흔들림
            transform.position += new Vector3(horizontalSwing, fallSpeed, 0) * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 초기 회전 방향에서 추락 단계
        elapsedTime = 0f; // 경과 시간 초기화
        while (transform.position.y > -10) // 화면 아래로 추락
        {
            fallSpeed += acceleration * Time.deltaTime; // 추락 속도 증가
            transform.position += fallDirection * fallSpeed * Time.deltaTime; // 아래로 이동
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime); // 회전
            yield return null;
        }

        // 추락 후 오브젝트 삭제
        Destroy(transform.gameObject);
    }

    // 파괴 처리
    public void TriggerDestruction(Transform transform)
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        foreach (Renderer r in renderers)
        {
            r.material.color = Color.black;
        }

        isDestroyed = true;
        transform.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(FallAndDestroy(transform));
    }
}
