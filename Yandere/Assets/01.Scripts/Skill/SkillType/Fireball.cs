using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 8f;
    public float maxDistance = 10f;
    public float damage = 20f;
    public GameObject damageZonePrefab;
    public LayerMask enemyLayer;

    private Vector2 moveDirection;
    private Vector2 spawnPosition;
    private Transform target;

    void Start()
    {
        spawnPosition = transform.position;
        moveDirection = transform.up;
        target = FindClosestEnemy();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
            moveDirection = dir;
        }

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(spawnPosition, transform.position) > maxDistance)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            var target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            Explode();
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 2f, enemyLayer);
        foreach (var e in enemies)
        {
            var target = e.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage * 0.5f);
            }
        }

        if (damageZonePrefab != null)
        {
            Instantiate(damageZonePrefab, transform.position, Quaternion.identity);
        }
    }

    Transform FindClosestEnemy()
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
