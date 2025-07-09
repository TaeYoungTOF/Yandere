using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRangeProjectile  : MonoBehaviour
{
    [SerializeField] private float speed = 6f; // 초당 속도
    [SerializeField] private float distance = 15f; // 총 비행 거리
    [SerializeField] private float lifetime = 5f;

    private Vector2 moveDir;
    private float damage;

    public void Init(Vector2 dir, float damage)
    {
        this.moveDir = dir.normalized;
        this.damage = damage;

        // ⏫ 회전 보정!
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 270f, Vector3.forward);

        Vector3 targetPos = transform.position + (Vector3)(moveDir * distance);

        transform.DOMove(targetPos, distance / speed)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[RangeProjectile] 플레이어 피격!");
            StageManager.Instance.Player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
