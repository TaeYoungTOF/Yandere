using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCutter : MonoBehaviour
{
    // public float speed = 15f;
    // public float damage = 15f;
    // public float maxDistance = 10f;
    // public int pierceCount = 5;
    // public LayerMask enemyLayer;

    // private Vector2 moveDirection;
    // private Vector2 spawnPosition;

    // void Start()
    // {
    //     spawnPosition = transform.position;
    //     moveDirection = transform.up;  // 발사 방향 설정
    // }

    // void Update()
    // {
    //     // 이동
    //     transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

    //     // 최대 사거리 초과 시 제거
    //     if (Vector2.Distance(spawnPosition, transform.position) > maxDistance)
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // 적 레이어에 해당하는지 확인
    //     if (((1 << other.gameObject.layer) & enemyLayer) != 0)
    //     {
    //         var target = other.GetComponent<IDamagable>();
    //         if (target != null)
    //         {
    //             target.TakeDamage(damage);
    //         }

    //         pierceCount--;
    //         if (pierceCount < 0)
    //         {
    //             Destroy(gameObject);
    //         }
    //     }
    // }
}
