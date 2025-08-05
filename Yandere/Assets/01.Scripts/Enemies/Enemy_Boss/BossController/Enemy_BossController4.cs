using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossController4 : EnemyController
{
    
    
    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 12f;
    [SerializeField] private float pattern2Cooldown = 10f;

    [Header("보스 패턴1 스모크 스크린")]
    [SerializeField] private GameObject pattern1SmokeEffectPrefab;
    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private float pattern1DetectDistance = 8f;
    [SerializeField] private float grenadeThrowHeight = 3f;
    [SerializeField] private float grenadeDuration = 1.5f;

    [Header("보스 패턴2 화염방사기")]
    [SerializeField] private GameObject flameEffectPrefab;
    [SerializeField] private float flameDuration = 3f;
    [SerializeField] private float flameDamagePerTick = 50;
    //[SerializeField] private float flameEffectRadius = 3.5f;      // 🔥 시각 효과 반경
    [SerializeField] private float flameDamageRadius = 5f;        // 🔥 실제 데미지 반경
    [SerializeField] private float flameTickInterval = 0.5f;

    [Header("보스 패턴3 드론 소환")]
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private Transform[] droneSpawnPoints;
   
     
    
    

    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;


    private bool triggered70 = false;
    private bool triggered40 = false;
    private bool triggered10 = false;

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
    #region  #region 보스 몬스터3 : TakeDamage 코드

    public override void TakeDamage(float damage)
    {
        SoundManager.Instance.Play("InGame_Enemy_HitSFX01");
        if (isDead) return;

        damage *= 1 - enemyData.monsterDef / (enemyData.monsterDef + 500);
        _monsterCurrentHealth -= damage;

        Debug.Log($"[보스컨트롤러4] {enemyData.monsterName}가 {damage} 피해를 입었습니다");
        _animator.SetTrigger("Hit");

        if (_monsterCurrentHealth <= 0)
        {
            BossMonsterDie();
        }
    }
    #endregion

    #region 보스 몬스터4 : Die 코드

    private void BossMonsterDie()
    {
        isDead = true;
        _rigidbody2D.velocity = Vector2.zero;
        _animator.SetTrigger("Dead");

        StageManager.Instance.ChangeKillCount(1);
        Destroy(gameObject, 1.0f);
        
        StageManager.Instance.StageClear();
    }

    #endregion

  
    
    

    private IEnumerator BossPatternRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);
            if (isPatterning) continue;

            List<IEnumerator> availablePatterns = new();

            if (pattern1Timer <= 0f && IsPlayerInPattern1Range())
                availablePatterns.Add(ExecutePattern1());
            if (pattern2Timer <= 0f && IsPlayerInPattern2Range())
                availablePatterns.Add(ExecutePattern2());
        
            float hpPercent = _monsterCurrentHealth / enemyData.monsterMaxHp * 100f;

            if (!triggered70 && hpPercent <= 70f)
                availablePatterns.Add(ExecutePattern3_70());
            if (!triggered40 && hpPercent <= 40f)
                availablePatterns.Add(ExecutePattern3_40());
            if (!triggered10 && hpPercent <= 10f)
                availablePatterns.Add(ExecutePattern3_10());

            if (availablePatterns.Count > 0)
            {
                int rand = Random.Range(0, availablePatterns.Count);
                StartCoroutine(availablePatterns[rand]);
            }
        }
    }
#region 패턴1: 스모크 수류탄
    private bool IsPlayerInPattern1Range()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        return distance <= pattern1DetectDistance;
    }

    private IEnumerator ExecutePattern1()
    {
        isPatterning = true;
        pattern1Timer = pattern1Cooldown;

        Debug.Log("[보스 패턴1] 스모크 수류탄 발동");
        _animator.Play("Idle");

        yield return new WaitForSeconds(0.5f);

        ThrowSmokeGrenade();

        yield return new WaitForSeconds(1f);
        isPatterning = false;
    }

    private void ThrowSmokeGrenade()
    {
        if (_playerTransform == null || pattern1SmokeEffectPrefab == null)
        {
            Debug.LogWarning("스모크 수류탄 투척 실패: 대상 또는 프리팹 없음");
            return;
        }

        Vector3 startPos = smokeSpawnPoint.position;
        Vector3 targetPos = _playerTransform.position;

        GameObject grenade = Instantiate(pattern1SmokeEffectPrefab, startPos, Quaternion.identity);
       
        var grenadeScript = grenade.GetComponent<BossPattern4_Projectile>();

        if (grenadeScript != null)
        {
            grenadeScript.Init(targetPos, grenadeThrowHeight, grenadeDuration);
        }
    }
    #endregion
    
#region 패턴2: 화염 방사기

private bool IsPlayerInPattern2Range()
{
    float detectRange = 8f; // 원형 감지 범위
    Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRange, LayerMask.GetMask("Player"));
    return hit != null;
}

private IEnumerator ExecutePattern2()
{
    isPatterning = true;
    pattern2Timer = pattern2Cooldown;

    Debug.Log("보스 패턴2: 화염방사기 발동");
    _animator.Play("Idle");

    yield return new WaitForSeconds(0.5f);

    // 플레이어 방향 기준 회전 각도 계산
    Vector2 forward = (_playerTransform.position - transform.position).normalized;
    float angleDeg = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;

    // 🔥 화염 이펙트 생성
    GameObject effect = Instantiate(flameEffectPrefab, transform.position, Quaternion.Euler(0, 0, angleDeg - 90f));
    SoundManager.Instance.Play("InGame_EnemyBoss4Pattern2_FlameSFX");
    effect.transform.parent = transform;
    Destroy(effect, flameDuration);

    // 이펙트 크기 변경 (선택사항)
    // effect.transform.localScale = Vector3.one * flameEffectRadius;

    float fanAngle = 45f; // 부채꼴 각도
    float timer = 0f;

    while (timer < flameDuration)
    {
        Collider2D[] candidates = Physics2D.OverlapCircleAll(transform.position, flameDamageRadius, LayerMask.GetMask("Player"));

        foreach (var target in candidates)
        {
            if (target == null) continue;

            Vector2 toTarget = (target.transform.position - transform.position);
            float distance = toTarget.magnitude;

            // ✅ 거리 체크 (flameDamageRadius 사용)
            if (distance > flameDamageRadius)
                continue;

            // ✅ 부채꼴 방향 체크
            float angleToTarget = Vector2.Angle(forward, toTarget.normalized);
            if (angleToTarget <= fanAngle / 2f)
            {
                Player player = target.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(flameDamagePerTick);
                }
            }
        }

        timer += flameTickInterval;
        yield return new WaitForSeconds(flameTickInterval);
    }

    yield return new WaitForSeconds(1f);
    isPatterning = false;
}


#endregion

#region 패턴3: 군사 드론 소환

private bool ShouldTriggerPattern3()
{
    float hpPercent = _monsterCurrentHealth / enemyData.monsterMaxHp * 100f;
    return (!triggered70 && hpPercent <= 70f) ||
           (!triggered40 && hpPercent <= 40f) ||
           (!triggered10 && hpPercent <= 10f);
}

private IEnumerator ExecutePattern3_70()
{
    isPatterning = true;
    triggered70 = true;
    Debug.Log("[보스 패턴3] 드론 5개 소환 (70%)");
    _animator.Play("Idle");

    yield return new WaitForSeconds(0.5f);
    SpawnDrones(5);
    yield return new WaitForSeconds(1f);
    isPatterning = false;
}

private IEnumerator ExecutePattern3_40()
{
    isPatterning = true;
    triggered40 = true;
    Debug.Log("[보스 패턴3] 드론 6개 소환 (40%)");
    _animator.Play("Idle");

    yield return new WaitForSeconds(0.5f);
    SpawnDrones(6);
    yield return new WaitForSeconds(1f);
    isPatterning = false;
}

private IEnumerator ExecutePattern3_10()
{
    isPatterning = true;
    triggered10 = true;
    Debug.Log("[보스 패턴3] 드론 7개 소환 (10%)");
    _animator.Play("Idle");

    yield return new WaitForSeconds(0.5f);
    SpawnDrones(7);
    yield return new WaitForSeconds(1f);
    isPatterning = false;
}

private void SpawnDrones(int count)
{
    List<Transform> shuffledSpawnPoints = new List<Transform>(droneSpawnPoints);
    ShuffleList(shuffledSpawnPoints);

    for (int i = 0; i < count; i++)
    {
        Transform spawnPoint;
        if (i < shuffledSpawnPoints.Count)
        {
            spawnPoint = shuffledSpawnPoints[i];
        }
        else
        {
            // 스폰포인트보다 드론이 많으면 중복 허용
            spawnPoint = droneSpawnPoints[Random.Range(0, droneSpawnPoints.Length)];
        }

        Instantiate(dronePrefab, spawnPoint.position, Quaternion.identity);
    }
}

private void ShuffleList<T>(List<T> list)
{
    for (int i = 0; i < list.Count; i++)
    {
        int rand = Random.Range(i, list.Count);
        (list[i], list[rand]) = (list[rand], list[i]);
    }
}

#endregion


    #region 디버그용 Gizmos

    private void OnDrawGizmosSelected()
    {
        // 기존 코드 유지
        Gizmos.color = Color.gray;
        if (smokeSpawnPoint != null)
            Gizmos.DrawWireSphere(smokeSpawnPoint.position, 1.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 8f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, flameDamageRadius );

        // 🔥 부채꼴 방향 시각화 추가
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
#endif

        if (_playerTransform != null)
        {
            Vector3 origin = transform.position;
            Vector2 forward = (_playerTransform.position - transform.position).normalized;
            float fanAngle = 45f;
            int segments = 30;
            float step = fanAngle / segments;

            Gizmos.color = Color.yellow;

            for (int i = 0; i <= segments; i++)
            {
                float angle = -fanAngle / 2f + step * i;
                float rad = Mathf.Deg2Rad * angle;
                Vector2 dir = new Vector2(
                    forward.x * Mathf.Cos(rad) - forward.y * Mathf.Sin(rad),
                    forward.x * Mathf.Sin(rad) + forward.y * Mathf.Cos(rad)
                );

                Gizmos.DrawLine(origin, origin + (Vector3)(dir.normalized * flameDamageRadius ));
            }

            // 정면 방향선
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin, origin + (Vector3)(forward * flameDamageRadius ));
        }
    }
    
    #endregion
}
