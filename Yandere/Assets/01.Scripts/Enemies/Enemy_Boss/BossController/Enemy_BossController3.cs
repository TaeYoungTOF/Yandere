using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy_BossController3 : EnemyController
{
    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 12f;
    [SerializeField] private float pattern2Cooldown = 15f;
    
    [Header("보스패턴1 레이저 설정")]
    [SerializeField] private GameObject pattern1LaserPrefab;
    [SerializeField] private GameObject pattern1LaserChargeEffectPrefab;
    [SerializeField] private Transform pattern1LaserSpawnPoint;
    [SerializeField] private float laserDamage = 100;
    [SerializeField] private float laserChargeTime = 2f;
    [SerializeField] private int laserCount = 3;
    [SerializeField] private float pattern1LaserRange = 10f;
    [SerializeField] private float laserInterval = 2f;
    [SerializeField] private float patternWarningTime = 0.5f;
    [SerializeField] private LineRenderer LaserLinePreview;


    [Header("보스패턴2 360도 베기 설정")]
    [SerializeField] private GameObject slashEffectPrefab;
    [SerializeField] private GameObject slashWarningPrefab;
    [SerializeField] private float warningDuration = 0.6f;
    [SerializeField] private float teleportDistanceBehindPlayer = 1.5f;
    [SerializeField] private float preAttackDelay = 1f;
    [SerializeField] private float pattern2SlashRadius = 3f;
    [SerializeField] private float pattern2AttackRange = 7f;
    [SerializeField] private int pattern2Damage = 100;

    [Header("보스 패턴3 레이저 설정")]
    [SerializeField] private GameObject laserPrefab_Pattern3;
    [SerializeField] private GameObject pattern3LaserChargeEffectPrefab;
    [SerializeField] private int laserCount_Pattern3 = 5;
    [SerializeField] private float laserChargeTime_Patter3 = 1;
    [SerializeField] private float laserInterval_Pattern3 = 0.6f;


    private Vector2 cachedDashDir;
    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;


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
    }


    #region 보스 몬스터3 : TakeDamage 코드

    public override void TakeDamage(float damage)
    {
        SoundManager.Instance.Play("InGame_Enemy_HitSFX01");
        if (isDead) return;

        damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
        _monsterCurrentHealth -= damage;

        Debug.Log($"[보스컨트롤러3] {enemyData.monsterName}가 {damage} 피해를 입었습니다");

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
        isDead = true; // 죽은 상태체크
        _rigidbody2D.velocity = Vector2.zero; // Vector2.zero(0,0)을 _rigidbody2D.velocity에 넣어줌 (안 움직이게 하는 코드)
        _animator.SetTrigger("Dead"); // 애니메이터의 파라미터(트리거) "Dead"를 실행

        StageManager.Instance.ChangeKillCount(1);
        StartCoroutine(DelayedReturnToPool(1));

        StageManager.Instance.StageClear();
    }

    #endregion

    #region 보스 패턴 루틴

    private IEnumerator BossPatternRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);
            if (isPatterning) continue;

            List<int> availablePatterns = new List<int>();

            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            if (pattern1Timer <= 0f && _monsterCurrentHealth / enemyData.monsterMaxHp >= 0.5f &&
                distanceToPlayer <= pattern1LaserRange)
                availablePatterns.Add(1);

            if (pattern2Timer <= 0f && distanceToPlayer <= pattern2AttackRange)
                availablePatterns.Add(2);

            if (pattern1Timer <= 0f && _monsterCurrentHealth / enemyData.monsterMaxHp < 0.5f &&
                distanceToPlayer <= pattern1LaserRange)
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

    #region 패턴1: 1자 레이저 3번 발사

    private IEnumerator ExcutePattern1()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("보스 패턴1: 레이저 3회 발사");
        
        // 조준 및 충전
        GameObject chargeEffect = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage3BossSkillPattern1Proj01, pattern1LaserSpawnPoint.position, Quaternion.identity);
        //GameObject chargeEffect = Instantiate(pattern1LaserChargeEffectPrefab, pattern1LaserSpawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(laserChargeTime); // 충전 시간
        
        for (int i = 0; i < laserCount; i++)
        {
            
            Vector2 dir = (_playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            _spriteRenderer.flipX = dir.x < 0;
            
            // ✅ 플레이어 조준 방향 고정
            cachedDashDir = (_playerTransform.position - transform.position).normalized;
            
            // ✅ 라인 렌더러 켜고 방향 미리보기 표시
            if (LaserLinePreview != null)
            {
                LaserLinePreview.enabled = true;
                ShowDashPreview(cachedDashDir); // 방향 고정
            }
        
            yield return new WaitForSeconds(patternWarningTime);  // 경고 시간
            
            if (LaserLinePreview != null)
                LaserLinePreview.enabled = false;
            
            ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage3BossSkillPattern1Proj01, chargeEffect);
            //Destroy(chargeEffect);
            
            FireLaser();                                            // 레이저 발사 함수
            
          
            
            yield return new WaitForSeconds(laserInterval);         // 레이저 발사 간격
        }
        yield return new WaitForSeconds(2f);
        isPatterning = false;
    }

    private void FireLaser()
    {
        if (pattern1LaserPrefab == null)
        {
            Debug.LogWarning("LaserBullet 실패: LaserBullet null입니다.");
        }

        Vector3 startPos = pattern1LaserSpawnPoint.position;
        Vector3 direction = cachedDashDir;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // 레이저 생성
        //GameObject laser = Instantiate(pattern1LaserPrefab, startPos, rotation);
        GameObject laser = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage3BossSkillPattern1Proj02, startPos, rotation);
        
        // 사운드 이펙트
        SoundManager.Instance.Play("InGame_EnemyBoss3Pattern1_LaserSFX");
        
        // 보스3용 Projectile 컴포넌트 가져오기
        Enemy_Boss3_pattern1_Projectile01 proj = laser.GetComponent<Enemy_Boss3_pattern1_Projectile01>();
        
        if (proj != null)
        {
            proj.Init(direction, laserDamage);
        }
    }

    private void ShowDashPreview(Vector2 dir)
    {
        if (LaserLinePreview == null) return;

        Vector3 start = transform.position;
        float LaserDistance = 13f; // 
        Vector3 end = start + (Vector3)(dir.normalized * LaserDistance);

        LaserLinePreview.SetPosition(0, start);
        LaserLinePreview.SetPosition(1, end);
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
        _animator.SetBool("Run", false);
        Debug.Log("보스 패턴2: 순간이동 후 360도 공격");

        yield return new WaitForSeconds(preAttackDelay);

        GameObject warning = Instantiate(slashWarningPrefab, transform.position, Quaternion.identity);
        warning.transform.localScale = Vector3.one * pattern2SlashRadius * 2f; // 직경으로 조절
        Destroy(warning, warningDuration + 0.1f); // 경고 표시 제거

        yield return new WaitForSeconds(warningDuration); // ⚠️ 경고 시간 대기

        //var effect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        GameObject effect = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage3BossSkillPattern2Proj01, transform.position, Quaternion.identity);
        StartCoroutine(DelayedReturnToPool(0.3f));
        ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage3BossSkillPattern2Proj01, effect);
        //Destroy(effect, 0.3f); // ✅ 생성된 인스턴스만 파괴
        SoundManager.Instance.Play("InGame_EnemyBoss3Pattern2_SlashSFX");

        // 360도 슬래시 공격
        float dist = Vector2.Distance(transform.position, _playerTransform.position);
        if (dist <= pattern2SlashRadius)
        {
            Player player = _playerTransform.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(pattern2Damage);
            }
        }

        UpdateSpriteDirection(_playerTransform.position - transform.position); // 방향을 마지막에 고정

        yield return new WaitForSeconds(0.5f); // 후딜 시간 추가 (안정성)

        isPatterning = false;
    }

    #endregion

    #region 패턴3: 1자 레이저 5번 발사 (HP 50 이하 일 때)

    private IEnumerator ExecutePattern3()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("보스 패턴3: 빠른 레이저 5회 발사");

        // 충전 대기
        //GameObject chargeEffect = Instantiate(pattern3LaserChargeEffectPrefab, pattern1LaserSpawnPoint.position, Quaternion.identity);
        GameObject chargeEffect = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage3BossSkillPattern3Proj01, pattern1LaserSpawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(laserChargeTime_Patter3);

        // 연속 레이저 발사
        for (int i = 0; i < laserCount_Pattern3; i++)
        {
            Vector2 dir = (_playerTransform.position - transform.position).normalized;
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            
            _spriteRenderer.flipX = dir.x < 0;
            
            // ✅ 플레이어 조준 방향 고정
            cachedDashDir = (_playerTransform.position - transform.position).normalized;
            
            // ✅ 라인 렌더러 켜고 방향 미리보기 표시
            if (LaserLinePreview != null)
            {
                LaserLinePreview.enabled = true;
                ShowDashPreview(cachedDashDir); // 방향 고정
            }
        
            yield return new WaitForSeconds(patternWarningTime);  // 경고 시간
            //Destroy(chargeEffect);
            ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage3BossSkillPattern3Proj01, chargeEffect);
            // laser = Instantiate(laserPrefab_Pattern3, pattern1LaserSpawnPoint.position, Quaternion.identity);
            GameObject laser = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage3BossSkillPattern3Proj02, pattern1LaserSpawnPoint.position, Quaternion.identity);
            
            SoundManager.Instance.Play("InGame_EnemyBoss3Pattern1_LaserSFX");
            var proj = laser.GetComponent<Enemy_Boss3_pattern1_Projectile01>();
            if (proj != null)
            {
                proj.Init(cachedDashDir, laserDamage);
            }

            if (LaserLinePreview != null)
                LaserLinePreview.enabled = false;
            
            yield return new WaitForSeconds(laserInterval_Pattern3);
        }

        isPatterning = false;
    }

    #endregion
    
    #region Scene창 사거리 기즈모 표시

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pattern1LaserRange);

        // 패턴2 감지 범위
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, pattern2AttackRange);
    }

    #endregion


    
}
