using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_BossController2 : EnemyController
{
    [Header("보스 패턴 트리거 범위")]
    [SerializeField] private Transform pattern1Box;
    [SerializeField] private Vector2 pattern1Size;

    [SerializeField] private Transform pattern2Box;
    [SerializeField] private Vector2 pattern2Size;

    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 10f;
    [SerializeField] private float pattern2Cooldown = 15f;
    
    [Header("패턴1 불렛 설정")]
    [SerializeField] private GameObject projectilePrefab; // Demo_Project.Projectile 붙은 프리팹
    [SerializeField] private Transform firePoint; // 발사 위치 (보스 총구 위치)
    [SerializeField] private float bulletSpeed = 10f;

    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;

    private bool isPatterning = false;

    protected override void Start()
    {
        base.Start();
        Debug.Log("보스 Start 실행됨");
        StartCoroutine(BossPatternRoutine());
    }

    protected override void Update()
    {
        base.Update(); // 혹시라도 이동/사망은 그대로 유지
        if (isDead) return;

        pattern1Timer -= Time.deltaTime;
        pattern2Timer -= Time.deltaTime;
    }

    private IEnumerator BossPatternRoutine()
    {
        Debug.Log("보스 패턴 루프 진입");
        while (!isDead)
        {
            yield return new WaitForSeconds(3f); // Idle 대기 시간
            Debug.Log("패턴 체크 시작");

            if (isPatterning)
            {
                Debug.Log("이미 패턴 중");
                continue;
            }

            if (IsPlayerInPattern1Range() && pattern1Timer <= 0f)
            {
                StartCoroutine(ExecutePattern1());
                continue;
            }

            if (IsPlayerInPattern2Range() && pattern2Timer <= 0f)
            {
                StartCoroutine(ExecutePattern2());
                continue;
            }

            // 기본 동작 or 대기
            yield return null;
        }
    }

    #region 패턴1: 연속 사격
    private bool IsPlayerInPattern1Range()
    {
        Collider2D hit = Physics2D.OverlapBox(pattern1Box.position, pattern1Size, 0f, LayerMask.GetMask("Player"));
        return hit != null;
    }

    private IEnumerator ExecutePattern1()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("보스 패턴1: 연속 사격 시작");

        //animator.SetTrigger("GunAttack"); // 애니메이션 트리거
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 5; i++)
        {
            FireBullet(); // 총알 발사 함수
            yield return new WaitForSeconds(0.2f); // 연사 간격
        }

        isPatterning = false;
    }

    private void FireBullet()
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning("FireBullet 실패: _playerTransform이 null입니다.");
            return;
        }

        Debug.Log("총알 발사 실행됨");
        Vector3 dir = (_playerTransform.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var proj = bullet.GetComponent<BossPattern2_Projectile>();

        if (proj == null)
        {
            Debug.LogWarning("Projectile 스크립트가 프리팹에 없음!");
            return;
        }

        proj.moveAngle = angle;
        proj.spriteAngle = angle;
        proj.moveSpeed = bulletSpeed;
        
        bool facingLeft = _spriteRenderer.flipX;
        proj.SetFacingDirection(facingLeft);
    }
    #endregion

    #region 패턴2: 섬광 수류탄
    private bool IsPlayerInPattern2Range()
    {
        Collider2D hit = Physics2D.OverlapBox(pattern2Box.position, pattern2Size, 0f, LayerMask.GetMask("Player"));
        return hit != null;
    }

    private IEnumerator ExecutePattern2()
    {
        isPatterning = true;
        pattern2Timer = pattern2Cooldown;

        Debug.Log("보스 패턴2: 섬광 수류탄");

        //animator.SetTrigger("ThrowGrenade"); // 애니메이션 트리거
        yield return new WaitForSeconds(0.5f);

        ThrowFlashGrenade(); // 수류탄 던지기

        yield return new WaitForSeconds(1f);
        isPatterning = false;
    }

    private void ThrowFlashGrenade()
    {
        Debug.Log("수류탄 투척 (여기에 Grenade 프리팹 생성)");
        // Instantiate(grenadePrefab, transform.position, Quaternion.identity);
    }
    #endregion

    #region 디버그용 Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (pattern1Box != null)
            Gizmos.DrawWireCube(pattern1Box.position, pattern1Size);

        Gizmos.color = Color.blue;
        if (pattern2Box != null)
            Gizmos.DrawWireCube(pattern2Box.position, pattern2Size);
    }
    #endregion
}
