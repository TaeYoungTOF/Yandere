using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float damage, speed, range;
    int pierceCount;
    LayerMask enemyLayer;

    Vector2 moveDir;
    Vector2 spawnPos;

    public void Initialize(float damage, float speed, float range, int pierceCount, LayerMask enemyLayer)
    {
        this.damage = damage;
        this.speed = speed;
        this.range = range;
        this.pierceCount = pierceCount;
        this.enemyLayer = enemyLayer;
    }

    void Start()
    {
        moveDir = transform.up;
        spawnPos = transform.position;
    }

    void Update()
    {
        transform.Translate(moveDir * speed * Time.deltaTime);
        if (Vector2.Distance(spawnPos, transform.position) > range)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & enemyLayer) != 0)
        {
            //other.GetComponent<Enemy>()?.TakeDamage(damage);
            pierceCount--;
            if (pierceCount < 0)
                Destroy(gameObject);
        }
    }
}
