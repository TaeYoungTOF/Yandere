using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBoomProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // 초당 속도
   // [SerializeField] private float distance = 10f; // 총 이동 거리
   // [SerializeField] private float lifetime = 3f; // 최대 유지 시간

    private Vector2 moveDir;

    public void Init(Vector2 dir)
    {
        // 1. 이동 방향 지정
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;

        // 2. 회전 방향 설정 (총알이 보는 방향 맞추기)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 총알 스프라이트가 "오른쪽"을 기본 방향으로 그려졌을 경우
        //transform.rotation = Quaternion.Euler(0, 0, angle);

        // 만약 스프라이트가 "위쪽"이 기본 방향이라면?
        transform.rotation = Quaternion.Euler(0, 0, angle - 45f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[BoomProjectile] 플레이어 연막탄 적중!");
            StageManager.Instance.Player.TakeDamage(10f); // 데미지 예시
            Destroy(gameObject);
        }
    }
}
