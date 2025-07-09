using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBoomProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // 초당 속도
    [SerializeField] private float distance = 10f; // 총 이동 거리
    [SerializeField] private float lifetime = 3f; // 최대 유지 시간

    private Vector2 moveDir;

    public void Init(Vector2 dir)
    {
        moveDir = dir.normalized;

        Vector3 targetPos = transform.position + (Vector3)(moveDir * distance);

        // ✅ DOTween으로 직선 이동
        transform.DOMove(targetPos, distance / speed)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));

        Destroy(gameObject, lifetime); // 혹시 모르게 안전망
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
