using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private int damage;
    private float speed;
    private float maxDistance;
    private float explosionRadius;
    private LayerMask enemyLayer;
    private Vector2 moveDirection;
    private Vector2 spawnPosition;

    public GameObject damageZonePrefab;

    public void Initialize(SkillStatData data, LayerMask enemyLayer, Vector2 fallbackDirection)
    {
        this.damage = data.damage;
        this.speed = data.speed;
        this.maxDistance = data.range;
        this.explosionRadius = data.explosionRadius;
        this.enemyLayer = enemyLayer;

        spawnPosition = transform.position;

        // 적 자동 조준
        Transform target = FindClosestEnemy();
        if (target != null)
            this.moveDirection = ((Vector2)target.position - (Vector2)transform.position).normalized;
        else
            this.moveDirection = fallbackDirection.normalized; // 적이 없을 경우 기본 방향 사용
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        if (Vector2.Distance(spawnPosition, transform.position) > maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) == 0) return;

        var target = other.GetComponent<IDamagable>();
        target?.TakeDamage(damage);

        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (var e in enemies)
        {
            var target = e.GetComponent<IDamagable>();
            target?.TakeDamage(damage * 0.5f);
        }

        if (damageZonePrefab != null)
            Instantiate(damageZonePrefab, transform.position, Quaternion.identity);
    }

    private Transform FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10f, enemyLayer);
        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }
        return closest;
    }
}
