using System.Collections;
using UnityEngine;

public class BossDeathHandler : MonoBehaviour
{
    public GameObject[] explosionEffects; // 폭발 이펙트 프리팹 배열
    public float deathDuration = 5.0f;    // 보스 파괴 지속 시간
    public float initialExplosionFrequency = 0.05f; // 초기 폭발 빈도
    public float shakeIntensity = 0.5f;  // 흔들림 강도
    public Color targetColor = new Color(0f, 0f, 0f, 1f); // 검은색
    public Color hitColor = new Color(1f, 0.5f, 0f, 1f);  // 주황빛
    public float hitEffectDuration = 0.1f;                // 피격 효과 지속 시간

    private bool isDying = false;
    private bool isHit = false; // 피격 상태 플래그
    private float currentExplosionFrequency;
    private PolygonCollider2D polygonCollider;
    private SpriteRenderer spriteRenderer;
    private Coroutine explosionCoroutine;
    private Coroutine shakeCoroutine;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        currentExplosionFrequency = initialExplosionFrequency;
    }

    public bool IsHit
    {
        get { return isHit; }
    }

    public void ApplyHitEffect()
    {
        if (!isHit)
        {
            StartCoroutine(FlashHitEffect());
        }
    }

    private IEnumerator FlashHitEffect()
    {
        isHit = true;
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = hitColor; // 주황빛으로 변경
        yield return new WaitForSeconds(hitEffectDuration);
        isHit = false;
        spriteRenderer.color = originalColor; // 원래 색상 복구
    }


    /// 보스가 죽었을 때 실행
    public void TriggerDeathSequence()
    {
        if (isDying) return;
        isDying = true;

        // 파괴 효과 시작
        explosionCoroutine = StartCoroutine(SpawnExplosions());
        shakeCoroutine = StartCoroutine(ShakeAndDarkenBoss());

        // 파괴 완료 후 제거
        StartCoroutine(CompleteDeath());
    }

    private IEnumerator SpawnExplosions()
    {
        float elapsedTime = 0f;

        while (true)
        {
            Vector3 randomPosition = GetRandomPointInPolygon();
            int randomIndex = Random.Range(0, explosionEffects.Length);
            GameObject selectedEffect = explosionEffects[randomIndex];
            Instantiate(selectedEffect, randomPosition, Quaternion.identity);

            elapsedTime += currentExplosionFrequency;
            currentExplosionFrequency = Mathf.Lerp(initialExplosionFrequency, initialExplosionFrequency * 1.6f, elapsedTime / deathDuration);

            yield return new WaitForSeconds(currentExplosionFrequency);
        }
    }

    private IEnumerator ShakeAndDarkenBoss()
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < deathDuration)
        {
            float shakeElapsedTime = 0f;

            while (shakeElapsedTime < 0.5f)
            {
                float shakeAmountX = Random.Range(-shakeIntensity, shakeIntensity);
                transform.position = originalPosition + new Vector3(shakeAmountX, 0, 0);

                if (spriteRenderer != null)
                {
                    float t = elapsedTime / deathDuration;
                    spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, t);
                }

                shakeElapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
            elapsedTime += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator CompleteDeath()
    {
        yield return new WaitForSeconds(deathDuration);

        if (explosionCoroutine != null) StopCoroutine(explosionCoroutine);
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);

        StartCoroutine(FadeAndShrink());
    }

    private IEnumerator FadeAndShrink()
    {
        float fadeDuration = 2.0f;
        float elapsedTime = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 0.9f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);

            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
            }

            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private Vector3 GetRandomPointInPolygon()
    {
        if (polygonCollider == null) return transform.position;

        Vector2[] points = polygonCollider.points;
        int randomTriangleIndex = Random.Range(0, points.Length - 2);
        Vector2 a = transform.TransformPoint(points[0]);
        Vector2 b = transform.TransformPoint(points[randomTriangleIndex + 1]);
        Vector2 c = transform.TransformPoint(points[randomTriangleIndex + 2]);

        float r1 = Mathf.Sqrt(Random.Range(0f, 1f));
        float r2 = Random.Range(0f, 1f);
        float x = (1 - r1) * a.x + r1 * (1 - r2) * b.x + r1 * r2 * c.x;
        float y = (1 - r1) * a.y + r1 * (1 - r2) * b.y + r1 * r2 * c.y;

        return new Vector3(x, y, transform.position.z);
    }
}
