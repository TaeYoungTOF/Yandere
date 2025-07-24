using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float bulletDelay = 0.2f;
    
    [SerializeField] private EnemySkill_Dash dashSkill;
    [SerializeField] private EnemySkill_Boom boomSkill;
    
    
    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void Attack(float damage)
    {
        if (_playerTransform == null) return;
        
        dashSkill?.TryDash();                                               // 대쉬 스킬이 할당 되어 있다면 실행
        boomSkill?.TryBoom();                                               // 연막탄 스킬이 할당 되어 있다면 실행
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        StartCoroutine(FireBulletsSequentially(direction, damage));
        
        
    }
    IEnumerator FireBulletsSequentially(Vector2 direction, float damage)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyRangeProjectile>().Init(direction, damage);
            yield return new WaitForSeconds(bulletDelay);
        }
    }
}
