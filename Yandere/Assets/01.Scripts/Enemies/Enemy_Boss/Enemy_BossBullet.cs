using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;

    public void Init(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("[BossBullet] Rigidbody2D가 없습니다.");
            return;
        }

        rb.velocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifeTime);
    }
}
