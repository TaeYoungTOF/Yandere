using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damage;
    protected float speed;
    protected float range;
    protected int pierceCount;
    protected LayerMask enemyLayer;
    protected Vector2 direction;

    protected Vector2 spawnPos;
    protected Vector2 moveDir;

    public virtual void Initialize(int damage, float speed, float range, int pierceCount, LayerMask enemyLayer, Vector2 direction)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.pierceCount = pierceCount;
        this.enemyLayer = enemyLayer;
        this.direction = direction;

        spawnPos = transform.position;
        moveDir = direction.normalized;
    }

    protected virtual void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(spawnPos, transform.position) > range)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            var target = other.GetComponent<IDamagable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            pierceCount--;
            if (pierceCount < 0)
                Destroy(gameObject);
        }
    }
}
