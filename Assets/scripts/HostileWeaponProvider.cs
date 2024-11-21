using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HostileWeaponProvider : MonoBehaviour
{
    public string weaponVariant;
    public float fireRate;
    private GameObject player;
    private string bossName;
    private Vector2 playerDirection;
    private float singePatternVelocity = 4f;
    private float linearPatternVelocity = 4f;
    private float circlePatternVelocity = 4f;
    public GameObject bulletEnemyAPrefab; // 발사체를 연결
    public GameObject bulletEnemyBPrefab; // 발사체를 연결
    public GameObject bulletEnemyCPrefab; // 발사체를 연결
    float time;


    void Start() // initiate weapon, player reference
    {
        weaponVariant = gameObject.name; // name을 weapon값으로 받아옴
        player = GameObject.Find("Player");

        bulletEnemyAPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/src/prefabs/BulletA.prefab", typeof(GameObject));
        bulletEnemyBPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/src/prefabs/BulletB.prefab", typeof(GameObject));
        bulletEnemyCPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/src/prefabs/BulletA.prefab", typeof(GameObject));
    }

    void Update() // player를 가리키는 방향 벡터를 update합니다
    {
        time += Time.deltaTime;

        if (player != null)
        {
            playerDirection = (player.transform.position - transform.position).normalized;
        }
    }

    private void SinglePatternVariant(Vector2 direction)
    {
        if (bulletEnemyAPrefab != null)
        {
            // ±10도 랜덤 오차 추가
            float randomAngle = Random.Range(-15f, 15f);
            float angleInRadians = randomAngle * Mathf.Deg2Rad;

            // 기존 방향 벡터에 회전 적용
            Vector2 randomizedDirection = new Vector2(
                direction.x * Mathf.Cos(angleInRadians) - direction.y * Mathf.Sin(angleInRadians),
                direction.x * Mathf.Sin(angleInRadians) + direction.y * Mathf.Cos(angleInRadians)
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
    }

    private void LinearPatternVariant(Vector2 direction)
    {
        if (bulletEnemyBPrefab != null)
        {
            // 투사체를 생성하고 초기 속도를 적용
            GameObject projectile = Instantiate(bulletEnemyBPrefab, transform.position, Quaternion.LookRotation(direction));
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = direction * linearPatternVelocity;
            }

            Debug.Log("Projectile fired towards: " + direction);
        }
    }

    private IEnumerator CirclePatternAttackRoutine()
    {
        int repeatCount = 6; // 총 발사 횟수
        float delay = 0.13f;  // 각 발사 사이의 간격 (초 단위)

        for (int i = 0; i < repeatCount; i++)
        {
            CirelePatternVariant(); // Circle Pattern 발사
            yield return new WaitForSeconds(delay); // 0.5초 대기
        }
    }

    private void CirelePatternVariant()
    {
        int numberOfProjectiles = 30; // 한 번에 발사할 투사체의 개수
        float angleStep = 360f / numberOfProjectiles; // 각 투사체 간의 각도 차이
        float angle = time * 10; // 초기 각도

        if (bulletEnemyCPrefab != null)
        {
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
    }

    public void Shoot(string type)
    {
        if (type == "single")
        {
            SinglePatternVariant(playerDirection);
        }
        else if (type == "linear")
        {
            LinearPatternVariant(playerDirection);
        }
        else if (type == "circle")
        {
            StartCoroutine(CirclePatternAttackRoutine());
        }
    }
}
