using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;  // 발사할 총알 프리팹
    [SerializeField] private Transform bulletSpawnPoint; // 발사 위치

    private Transform playerTransform;

    private void Awake()
    {
        // 씬에서 플레이어 찾아서 기억
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    /// <summary>
    /// EnemyController가 호출해서 발사하는 메서드
    /// </summary>
    /// <param name="damage">발사할 데미지</param>
    public void DoAttack(float damage)
    {
        if (playerTransform == null) return;

        // 방향 계산
        Vector2 dir = (playerTransform.position - bulletSpawnPoint.position).normalized;

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // 총알에 Init 호출
        EnemyBulletTile projectile = bullet.GetComponent<EnemyBulletTile>();
        if (projectile != null)
        {
            projectile.Init(dir, damage);
        }
    }
}
