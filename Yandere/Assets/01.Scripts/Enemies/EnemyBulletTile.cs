using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletTile : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 5f; // 최대 수명 (안 맞으면 자동 파괴)

    private Vector2 moveDir;
    private float damage;

    public void Init(Vector2 dir, float damage)
    {
        this.moveDir = dir;
        this.damage = damage;

        Destroy(gameObject, lifetime);

        // (선택) 발사 방향으로 회전하고 싶으면 여기서 Rotation 설정
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 270f, Vector3.forward);
    }

    private void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[EnemyProjectile] 충돌 대상: {other.name}");

        // Player 태그만 감지
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[EnemyProjectile] 플레이어 피격!");
            other.GetComponent<Player>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
