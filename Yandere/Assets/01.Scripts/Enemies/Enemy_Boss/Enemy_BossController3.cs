using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_BossController3 : EnemyController
{

    [Header("보스 공격 범위 (패턴 사용 시 사정거리)")]
    [SerializeField] private Transform pattern1Box;
    [SerializeField] private Vector2 pattern1Size;
    [SerializeField] private Transform pattern2Center;   // 범위 체크 중심 (예: 보스 위치 기준)
    [SerializeField] private float pattern2DetectRadius = 6f; // 발동 조건: 일정 거리 이상
    [SerializeField] private float pattern2SlashRadius = 3f;  // 실제 공격 범위

    
    [Header("보스 패턴1 설정")] 
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private float laserSpeed = 15f;
    [SerializeField] private float laserChargeTime = 2f;
    [SerializeField] private float laserInterval = 2f;
    [SerializeField] private int laserCount = 3;
    [SerializeField] private int laserDamage = 100;

    [Header("보스 패턴2 설정")]
    [SerializeField] private float teleportDistanceBehindPlayer = 1.5f;
    [SerializeField] private float preAttackDelay = 2f;
    [SerializeField] private float slashRadius = 3f;
    [SerializeField] private int pattern2Damage = 100;
    [SerializeField] private GameObject slashEffectPrefab;

    
    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown  = 12f;
    [SerializeField] private float pattern2Cooldown  = 15f;

    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;
    private bool isPatterning = false;
    
    
    void Start()
    {
        base.Start();
        StartCoroutine(BossPatternRoutine());
    }

 
    void Update()
    {
        if (isDead) return;

        pattern1Timer -= Time.deltaTime;
    }
    
    
    #region 보스 몬스터3 : TakeDamage 코드
    public override void TakeDamage(float damage)
    {
        SoundManagerTest.Instance.Play("InGame_Enemy_HitSFX01");
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
    
    #region 보스 몬스터3 : Die 코드
    
    void BossMonsterDie()
    {
        isDead = true;                                                      // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero;                               // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead");                                  // 애니메이터의 파라미터(트리거) "Dead"를 실행
        
        StageManager.Instance.ChangeKillCount(1);
        Destroy(gameObject, 1.0f);
        
        //TODO : 클리어 UI창 연결 해야함
    }
    
    #endregion
    

    private IEnumerator BossPatternRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);

            if (isPatterning || pattern1Timer > 0f) continue;

            if (_monsterCurrentHealth / enemyData.monsterMaxHp >= 0.5f && IsPlayerInPattern1Range())
            {
                StartCoroutine(ExecutePattern1());
            }
            if (pattern2Timer <= 0f && IsPlayerInPattern2Range())
                StartCoroutine(ExecutePattern2());
        }
    }

    
    #region 패턴1: 1자 레이저 3번 발사
    private IEnumerator ExecutePattern1()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("보스 패턴1: 레이저 3회 발사");

        // 조준 및 충전
        yield return new WaitForSeconds(laserChargeTime); // 2초 충전

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
    #endregion
    
    #region 패턴2: 순간이동 후 360도 근접 공격

    private IEnumerator ExecutePattern2()
    {
        isPatterning = true;
        pattern2Timer = pattern2Cooldown;
        
        // 순간이동 위치 계산(플레이어 뒤쪽)
        Vector2 toPlayer = (_playerTransform.position - transform.position).normalized;
        Vector3 teleportPos = _playerTransform.position - (Vector3)(toPlayer * teleportDistanceBehindPlayer);
        
        // 순간이동
        transform.position = teleportPos;
        _rigidbody2D.velocity = Vector2.zero;
        
        Debug.Log("보스 패턴2: 순간이동 후 360도 공격");

        yield return new WaitForSeconds(preAttackDelay);
        
        // 360도 슬래시 공격
        Collider2D[] hits = Physics2D.OverlapCircleAll(pattern2Center.position, pattern2SlashRadius, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(pattern2Damage);
            }
        }
        
        if (slashEffectPrefab)
            Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        
        isPatterning = false;
    }
    
    private bool IsPlayerInPattern2Range()
    {
        float dist = Vector2.Distance(pattern2Center.position, _playerTransform.position);
        return dist > pattern2DetectRadius;
    }
    
    #endregion
    
    
    #region 디버그용 Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (pattern1Box != null)
            Gizmos.DrawWireCube(pattern1Box.position, pattern1Size);
        
        // 패턴2 감지 범위
        Gizmos.color = Color.cyan;
        if (pattern2Center != null)
            Gizmos.DrawWireSphere(pattern2Center.position, pattern2DetectRadius);

        // 패턴2 슬래시 범위
        Gizmos.color = Color.magenta;
        if (pattern2Center != null)
            Gizmos.DrawWireSphere(pattern2Center.position, pattern2SlashRadius);
    }
    
    #endregion
}
