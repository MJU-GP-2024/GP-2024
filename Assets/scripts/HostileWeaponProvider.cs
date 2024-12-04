using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HostileWeaponProvider : MonoBehaviour
{
    // 싱글 패턴 공격 설정
    private float singePatternVelocity = 4f;        // 투사체 속도
    // 리니어 패턴 공격 설정
    private float linearPatternVelocity = 7.5f;       // 투사체 속도
    private int linearPatternRepeatCount = 4;       // 발사 횟수
    private float linearPatternAttackDelay = 0.25f;  // 발사 간격
    // 원형 패턴 공격 설정
    private float circlePatternVelocity = 4f;       // 투사체 속도
    private int numberOfProjectiles = 26;           // 1회 발사 횟수
    private int circlePatternRepeatCount = 3;       // 총 발사 횟수
    private float circlePatternAttackDelay = 0.13f; // 발사 간격

    private GameObject player;
    private Vector2 playerDirection;
    // private GameObject bulletEnemyAPrefab;
    // private GameObject bulletEnemyBPrefab;
    // private GameObject bulletEnemyCPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletEnemyCPrefab;
    private float time;

    void Start() // initiate weapon, player reference
    {
        player = GameObject.Find("Player");

        if (player == null)
        {
            Debug.Log("Error; Missing Player object");
        }

        // bulletEnemyAPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/src/prefabs/BulletA.prefab", typeof(GameObject));
        // bulletEnemyBPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/src/prefabs/BulletB.prefab", typeof(GameObject));
        // bulletEnemyCPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/src/prefabs/BulletC.prefab", typeof(GameObject));

    }
    void Update() // player를 가리키는 방향 벡터를 update합니다
    {
        time += Time.deltaTime;

        if (player != null)
        {
            playerDirection = (player.transform.position - transform.position).normalized;
        }
    }

    private void SinglePatternVariant()
    {
        // ±10도 랜덤 오차 추가
        float randomAngle = Random.Range(-8f, 8f);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        // 기존 방향 벡터에 회전 적용
        Vector2 randomizedDirection = new Vector2(
            playerDirection.x * Mathf.Cos(angleInRadians) - playerDirection.y * Mathf.Sin(angleInRadians),
            playerDirection.x * Mathf.Sin(angleInRadians) + playerDirection.y * Mathf.Cos(angleInRadians)
        ).normalized;

        // 투사체 생성
        GameObject projectile = Instantiate(bulletEnemyAPrefab, transform.position, Quaternion.identity);

        // 투사체의 Rigidbody2D를 이용해 속도 적용
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = randomizedDirection * singePatternVelocity; // 속도 적용
        }
    }

    private IEnumerator LinearPatternAttackRoutine()
    {
        for (int i = 0; i < linearPatternRepeatCount; i++)
        {
            LinearPatternVariant(); // 투사체 발사
            yield return new WaitForSeconds(linearPatternAttackDelay); // 0.3초 대기
        }
    }

    private void LinearPatternVariant()
    {
        // 투사체 생성 및 초기화
        GameObject projectile = Instantiate(
            bulletEnemyBPrefab,
            transform.position,
            Quaternion.Euler(0, 0, Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg) // Z축 회전만 적용
        );

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = playerDirection.normalized * linearPatternVelocity; // 속도 적용
        }
    }

    private IEnumerator CirclePatternAttackRoutine()
    {
        for (int i = 0; i < circlePatternRepeatCount; i++)
        {
            CirelePatternVariant(); // Circle Pattern 발사
            yield return new WaitForSeconds(circlePatternAttackDelay); // 0.5초 대기
        }
    }

    private void CirelePatternVariant()
    {
        float angleStep = 360f / numberOfProjectiles; // 각 투사체 간의 각도 차이
        float angle = time * 10; // 초기 각도

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            // 각도를 라디안으로 변환
            float angleInRadians = Mathf.Deg2Rad * angle;

            // 각도를 기준으로 방향 벡터 계산
            Vector2 projectileDirection = new Vector2(
                Mathf.Cos(angleInRadians),
                Mathf.Sin(angleInRadians)
            ).normalized;

            // 투사체 생성
            GameObject projectile = Instantiate(bulletEnemyCPrefab, transform.position, Quaternion.identity);

            // 투사체의 Rigidbody2D를 이용해 속도 적용
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = projectileDirection * circlePatternVelocity; // 속도 적용
            }

            // 다음 투사체의 각도 계산
            angle += angleStep;
        }
    }

    public void Shoot(string type)
    {
        if (player == null) return;

        if (type == "single")
        {
            if (bulletEnemyAPrefab != null)
            {
                SinglePatternVariant();
            }
            else
            {
                Debug.Log("Error; Missing Assets/src/prefabs/BulletA.prefab");
            }
        }
        else if (type == "linear")
        {
            if (bulletEnemyBPrefab != null)
            {
                StartCoroutine(LinearPatternAttackRoutine());
            }
            else
            {
                Debug.Log("Error; Missing Assets/src/prefabs/BulletB.prefab");
            }
        }
        else if (type == "circle")
        {
            if (bulletEnemyCPrefab != null)
            {
                StartCoroutine(CirclePatternAttackRoutine());
            }
            else
            {
                Debug.Log("Error; Missing Assets/src/prefabs/BulletC.prefab");
            }
        }
        else
        {
            Debug.Log("Error; weapon " + type + " not found");
        }
    }
}
