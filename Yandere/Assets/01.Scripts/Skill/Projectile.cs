using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float maxDistance = 20f;
    public float damage = 10f;

    private Vector2 moveDirection;
    private Vector2 spawnPosition;

    void Start()
    {
        moveDirection = transform.up;
        spawnPosition = transform.position;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        float distance = Vector2.Distance(spawnPosition, transform.position);
        if (distance > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        */
    }
}
