using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_BossController2 : EnemyController
{
    [Header("보스 공격 범위 (패턴 사용 시 사정거리)")]
    [SerializeField] private Transform pattern1Box;
    [SerializeField] private Vector2 pattern1Size;
    [SerializeField] private Transform pattern2Box;
    [SerializeField] private Vector2 pattern2Size;
    [SerializeField] private Transform pattern3Box;
    [SerializeField] private Vector2 pattern3Size;

    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 10f;
    [SerializeField] private float pattern2Cooldown = 15f;
    [SerializeField] private float pattern3Cooldown = 7f;
    
    [Header("패턴1 불렛 설정")]
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private Transform firePoint; // 발사 위치 (보스 총구 위치)
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private int bulletCount = 5;

    [Header("패턴2 섬광 수류탄 설정")] 
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float grenadeThrowHeight  = 3f;
    [SerializeField] private float grenadeDuration = 1.5f;

    [Header("패턴3 단검 휘두르기 설정")] 
    [SerializeField] private int slashCount = 3;
    [SerializeField] private float slashInterval = 0.4f;
    [SerializeField] private int pattern3Damage = 100;
    [SerializeField] private float knockbackForce  = 10f;
    [SerializeField] private GameObject slashEffectPrefab;
    
    

    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;
    private float pattern3Timer = 0f;
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
        pattern3Timer -= Time.deltaTime;
        
    }

    private IEnumerator BossPatternRoutine()
    {
        Debug.Log("보스 패턴 루프 진입");
        while (!isDead)
        {
            yield return new WaitForSeconds(3f);
            if (isPatterning) continue;

            List<Func<IEnumerator>> availablePatterns = new();

            if (IsPlayerInPattern1Range() && pattern1Timer <= 0f)
                availablePatterns.Add(ExecutePattern1);
            if (IsPlayerInPattern2Range() && pattern2Timer <= 0f)
                availablePatterns.Add(ExecutePattern2);
            if (IsPlayerInPattern3Range() && pattern3Timer <= 0f)
                availablePatterns.Add(ExecutePattern3);

            if (availablePatterns.Count > 0)
            {
                int randIndex = Random.Range(0, availablePatterns.Count);
                StartCoroutine(availablePatterns[randIndex]());
            }

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

        for (int i = 0; i < bulletCount; i++)
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
        if (_playerTransform == null || grenadePrefab == null)
        {
            Debug.LogWarning("섬광 수류탄 투척 실패: 대상 또는 프리팹이 없음");
            return;
        }
        Vector3 startPos = firePoint.position;
        Vector3 targetPos = _playerTransform.position;
        
        GameObject grenade = Instantiate(grenadePrefab, startPos, Quaternion.identity);
        Enemy_BossGrenadeProjectile02 grenadeScript = grenade.GetComponent<Enemy_BossGrenadeProjectile02>();

        if (grenadeScript != null)
        {
            grenadeScript.Init(targetPos, grenadeThrowHeight, grenadeDuration);
        }
    }
    

    private bool IsPlayerInPattern3Range()
    {
        Collider2D hit = Physics2D.OverlapBox(pattern3Box.position, pattern3Size, 0f, LayerMask.GetMask("Player"));
        return hit != null;
    }

    private IEnumerator ExecutePattern3()
    {
        isPatterning = true;
        pattern3Timer = pattern3Cooldown;

        Debug.Log("보스 패턴3: 단검 휘두르기");
        for (int i = 0; i < slashCount; i++)
        {
            // 애니메이션 트리거 (필요 시)
            // animator.SetTrigger("Slash");

            // 이펙트 생성
            if (slashEffectPrefab)
                Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
            
            SoundManagerTest.Instance.Play("InGame_EnemyBoss2Pattern3_SlashSFX");

            // 데미지 및 넉백 적용
            DealSlashDamage();

            yield return new WaitForSeconds(slashInterval);
        }

        yield return new WaitForSeconds(0.5f);
        isPatterning = false;
    }
    
    private void DealSlashDamage()
    {
        Collider2D hit = Physics2D.OverlapBox(pattern3Box.position, pattern3Size, 0f, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            var player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(pattern3Damage);

                Vector2 knockDir = (hit.transform.position - transform.position).normalized;
                var rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
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
        
        Gizmos.color = Color.yellow;
        if (pattern3Box != null)
            Gizmos.DrawWireCube(pattern3Box.position, pattern3Size);
    }
    #endregion
}
