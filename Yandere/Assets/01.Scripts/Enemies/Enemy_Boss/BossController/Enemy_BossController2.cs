using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class Enemy_BossController2 : EnemyController
{

    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 10f;
    [SerializeField] private float pattern2Cooldown = 15f;
    [SerializeField] private float pattern3Cooldown = 7f;
    
    [Header("보스패턴1 연속 사격")]
    [SerializeField] private GameObject pattern1BulletPrefab; 
    [SerializeField] private Transform pattern1BulletSpawnPoint; // 발사 위치 (보스 총구 위치)
    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shootDelay = 0.5f;  
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float pattern1AttackRange = 10f;
    [SerializeField] private float pattern1Interval = 1f;

    [Header("보스패턴2 섬광 수류탄 설정")] 
    [SerializeField] private GameObject pattern2GrenadeProjectilePrefab;
    [SerializeField] private Transform pattern2GrenadeSpawnPoint;
    [SerializeField] private float pattern2Damage = 10f;
    [SerializeField] private float pattern2DebuffBlindDuration = 5f;
    [SerializeField] private float pattern2Interval;
    [SerializeField] private float pattern2GrenadeRange = 7f;
    [SerializeField] private float grenadeThrowHeight  = 3f;
    [SerializeField] private float grenadeDuration = 1.5f;

    [Header("보스패턴3 단검 휘두르기 설정")] 
    [SerializeField] private GameObject pattern3SlashEffectPrefab;
    [SerializeField] private int slashCount = 3;
    [SerializeField] private float slashInterval = 0.4f;
    [SerializeField] private float pattern3SlashRange = 3f;
    [SerializeField] private int pattern3Damage = 100;
    [SerializeField] private float knockbackForce  = 10f;
   
    
    
    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;
    private float pattern3Timer = 0f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(BossPatternRoutine());
    }

    void Update()
    {
        if (isDead) return;

        pattern1Timer -= Time.deltaTime;
        pattern2Timer -= Time.deltaTime;
        pattern3Timer -= Time.deltaTime;
    }
    
    #region 보스 몬스터2 : TakeDamage 코드
    public override void TakeDamage(float damage)
    {
        SoundManager.Instance.Play("InGame_Enemy_HitSFX01");
        if (isDead) return;

        damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
        _monsterCurrentHealth -= damage;

        Debug.Log($"[보스컨트롤러2] {enemyData.monsterName}가 {damage} 피해를 입었습니다");

        _animator.SetTrigger("Hit");

 
        if (_monsterCurrentHealth <= 0)
        {
            BossMonsterDie();
        }
    }
    
    #endregion

    #region 보스 몬스터2 : Die 코드
    void BossMonsterDie()
    {
        isDead = true;                                                      // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead");                                  // 애니메이터의 파라미터(트리거) "Dead"를 실행
        
        StageManager.Instance.ChangeKillCount(1);
        StartCoroutine(DelayedReturnToPool(1));

        StageManager.Instance.StageClear();
    }
    #endregion

    #region 보스 패턴 루틴

    private IEnumerator BossPatternRoutine()
    {
        Debug.Log("보스 패턴 루프 진입");
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);
            if (isPatterning) continue;

            List<int> availablePatterns = new List<int>();

            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            if (pattern1Timer <= 0f && distanceToPlayer <= pattern1AttackRange)
                availablePatterns.Add(1);

            if (pattern2Timer <= 0f && distanceToPlayer <= pattern2GrenadeRange)
                availablePatterns.Add(2);
            
            if (pattern3Timer <= 0f && distanceToPlayer <= pattern3SlashRange)
                availablePatterns.Add(3);

            if (availablePatterns.Count > 0)
            {
                int selected = availablePatterns[Random.Range(0, availablePatterns.Count)];

                switch (selected)
                {
                    case 1:
                        StartCoroutine(ExcutePattern1());
                        break;
                    case 2:
                        StartCoroutine(ExecutePattern2());
                        break;
                    case 3:
                        StartCoroutine(ExecutePattern3()); 
                        break;
                }
            }
        }
    }

    #endregion
    
    #region 패턴1: 연속 사격
    
    private IEnumerator ExcutePattern1()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("보스 패턴1: 연속 사격 시작");

        //animator.SetTrigger("GunAttack"); // 애니메이션 트리거
        yield return new WaitForSeconds(pattern1Interval);

        for (int i = 0; i < bulletCount; i++)            // bulletCount만큼 연사 발사
        {
            Vector2 dir = (_playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            _spriteRenderer.flipX = dir.x < 0;

            
            FireBullet();                                // 총알 발사 함수
            
            yield return new WaitForSeconds(shootDelay); // 연사 간격
        }
        
        
        yield return new WaitForSeconds(1f);            // 후딜
        isPatterning = false;
    }

    private void FireBullet()
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning("FireBullet 실패: _playerTransform이 null입니다.");
            return;
        }
        
        Vector3 dir = (_playerTransform.position - pattern1BulletSpawnPoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        // 총알 생성
        //GameObject bullet = Instantiate(pattern1BulletPrefab, pattern1BulletSpawnPoint.position, Quaternion.identity);
        GameObject bullet = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage2BossSkillPattern1Proj01, pattern1BulletSpawnPoint.position, Quaternion.identity);
        
        StartCoroutine(ReturnToPoolAfterDelay(bullet, 3.5f, PoolType.Stage2BossSkillPattern1Proj01));
        //Destroy(bullet, 3.5f);
  
        // 사운드 이펙트
        SoundManager.Instance.Play("InGame_EnemyBoss2Pattern1_GunSFX");
       
        // 보스2용 Projectile 컴포넌트 가져오기
        Enemy_Boss2_Pattern1_Projectile01 proj = bullet.GetComponent<Enemy_Boss2_Pattern1_Projectile01>();

        if (proj == null)
        {
            Debug.LogWarning("Projectile 스크립트가 프리팹에 없음!");
            return;
        }
        
        proj.Init(bulletDamage, bulletSpeed, angle);
        
        bool facingLeft = _spriteRenderer.flipX;
        proj.SetFacingDirection(facingLeft);
    }
    #endregion

    #region 패턴2: 섬광 수류탄
    
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
        if (_playerTransform == null || pattern2GrenadeProjectilePrefab == null)
        {
            Debug.LogWarning("섬광 수류탄 투척 실패: 대상 또는 프리팹이 없음");
            return;
        }
        Vector3 startPos = pattern2GrenadeSpawnPoint.position;
        Vector3 targetPos = _playerTransform.position;
        
       
        //GameObject grenade = Instantiate(pattern2GrenadeProjectilePrefab, startPos, Quaternion.identity);
        GameObject grenade = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage2BossSkillPattern2Proj01, startPos, Quaternion.identity);
        SoundManager.Instance.Play("InGame_EnemyBoss_ThrowingSFX");
        Enemy_Boss2_Pattern2_GrenadeProjectile02 grenadeScript = grenade.GetComponent<Enemy_Boss2_Pattern2_GrenadeProjectile02>();

        if (grenadeScript != null)
        {
            grenadeScript.Init(targetPos, grenadeThrowHeight, grenadeDuration, pattern2Damage, pattern2DebuffBlindDuration);;
        }
        
    }
    #endregion
    
    #region 패턴3: 단검 휘두르기
    
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
            GameObject slashEffect = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage2BossSkillPattern3Proj01, transform.position, Quaternion.identity);
           // GameObject slashEffect = Instantiate(pattern3SlashEffectPrefab, transform.position, Quaternion.identity);
           
           StartCoroutine(ReturnToPoolAfterDelay(slashEffect, 0.8f, PoolType.Stage2BossSkillPattern3Proj01));
           //Destroy(slashEffect, 0.8f);
           
           SoundManager.Instance.Play("InGame_EnemyBoss2Pattern3_SlashSFX");

            // 데미지 및 넉백 적용
            DealSlashDamage();

            yield return new WaitForSeconds(slashInterval);
        }

        yield return new WaitForSeconds(0.5f);
        isPatterning = false;
    }
    
    private void DealSlashDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pattern3SlashRange, LayerMask.GetMask("Player"));

        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(pattern3Damage);

                Vector2 knockDir = (hit.transform.position - transform.position).normalized;
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
    
    #endregion
    
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType poolType)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null && obj.activeInHierarchy)
        {
            ObjectPoolManager.Instance.ReturnToPool(poolType, obj);
        }
    }

    #region Scene창 사거리 기즈모 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pattern1AttackRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pattern2GrenadeRange);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pattern3SlashRange);
    }
    #endregion
}
