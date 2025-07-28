using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_BossController3 : EnemyController
{

    [Header("보스 공격 범위 (패턴 사용 시 사정거리)")]
    [SerializeField] private Transform pattern1Box;
    [SerializeField] private Vector2 pattern1Size;

    
    [Header("보스 패턴1 설정")] 
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private float laserSpeed = 15f;
    [SerializeField] private float laserChargeTime = 2f;
    [SerializeField] private float laserInterval = 2f;
    [SerializeField] private int laserCount = 3;
    [SerializeField] private int laserDamage = 100;
    

    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown  = 12f;

    private float pattern1Timer = 0f;
    private bool isPatterning = false;
    private float enemyCurrentHp;
    
    
    void Start()
    {
        base.Start();
        enemyCurrentHp = enemyData.monsterMaxHp;
        StartCoroutine(BossPatternRoutine());
    }

 
    void Update()
    {
        base.Update();
        if (isDead) return;

        pattern1Timer -= Time.deltaTime;
    }

    private IEnumerator BossPatternRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);

            if (isPatterning || pattern1Timer > 0f) continue;

            if (enemyCurrentHp / enemyData.monsterMaxHp >= 0.5f && IsPlayerInPattern1Range())
            {
                StartCoroutine(ExecutePattern1());
            }
        }
    }

    private IEnumerator ExecutePattern1()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("보스 패턴1: 레이저 3회 발사");

        // 조준 및 충전
        yield return new WaitForSeconds(2f); // 2초 충전

        for (int i = 0; i < 3; i++)
        {
            FireLaser(); // 레이저 발사
            yield return new WaitForSeconds(laserInterval);
        }

        isPatterning = false;
    }

    private void FireLaser()
    {
        if (_playerTransform == null || laserPrefab == null) return;

        Vector3 startPos = laserSpawnPoint.position;
        Vector3 direction = (_playerTransform.position - laserSpawnPoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject laser = Instantiate(laserPrefab, startPos, rotation);
        var proj = laser.GetComponent<Enemy_Boss3pattern1_Projectile1>();
        if (proj != null)
        {
            proj.Init(direction, laserSpeed, laserDamage);
        }
    }
    
    private bool IsPlayerInPattern1Range()
    {
        Collider2D hit = Physics2D.OverlapBox(
            pattern1Box.position,
            pattern1Size,
            0f,
            LayerMask.GetMask("Player")
        );

        return hit != null;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (pattern1Box != null)
            Gizmos.DrawWireCube(pattern1Box.position, pattern1Size);
    }
}
