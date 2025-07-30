using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossController4 : EnemyController
{
    
    
    [Header("보스 공격 범위 (패턴 사용 시 Scence 사정거리 표시)")]
    
    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 12f;
    [SerializeField] private float pattern2Cooldown = 10f;
    [SerializeField] private float pattern3Cooldown = 5f;

    [Header("보스 패턴1 스모크 스크린")]
    [SerializeField] private GameObject pattern1SmokeEffectPrefab;
    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private float pattern1DetectDistance = 8f;
    [SerializeField] private float grenadeThrowHeight = 3f;
    [SerializeField] private float grenadeDuration = 1.5f;

    [Header("보스 패턴2 화염방사기")]
    [SerializeField] private GameObject flameEffectPrefab;
    [SerializeField] private Transform pattern2Box;
    [SerializeField] private Vector2 pattern2Size;
    [SerializeField] private float flameThrowHeight = 3f;
    [SerializeField] private float flameDuration = 3f;
    [SerializeField] private float flameDamagePerTick = 50;
    [SerializeField] private float flameTickInterval = 0.5f;

    [Header("보스 패턴3 드론 소환")]
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private Transform[] droneSpawnPoints;
   
     
    
    

    private float pattern1Timer = 0f;
    private float pattern2Timer = 0f;
    private float pattern3Timer = 0f;

    private bool triggered70 = false;
    private bool triggered40 = false;
    private bool triggered10 = false;

    void Start()
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
     Collider2D hit = Physics2D.OverlapBox(pattern2Box.position, pattern2Size, 0f, LayerMask.GetMask("Player"));
     return hit != null;
    }

    private IEnumerator ExecutePattern2()
    {
        isPatterning = true;
        pattern2Timer = pattern2Cooldown;

        Debug.Log("보스 패턴2: 화염방사기 발동");

        yield return new WaitForSeconds(0.5f);

        // 🔥 화염 이펙트 생성 및 방향 회전
        GameObject effect = Instantiate(flameEffectPrefab, transform.position, Quaternion.identity);
        effect.transform.parent = transform;

        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // ✅ 위쪽 기준이면 -90도 회전 필요

        Destroy(effect, flameDuration);

        float timer = 0f;
        while (timer < flameDuration)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(pattern2Box.position, pattern2Size, 0f, LayerMask.GetMask("Player"));
            foreach (var hit in hits)
            {
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(flameDamagePerTick);
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
    
    #region 피격 및 사망 처리

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

    private void BossMonsterDie()
    {
        isDead = true;
        _rigidbody2D.velocity = Vector2.zero;
        _animator.SetTrigger("Dead");

        StageManager.Instance.ChangeKillCount(1);
        Destroy(gameObject, 1.0f);
    }

    #endregion

    #region 디버그용 Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        if (smokeSpawnPoint != null)
            Gizmos.DrawWireSphere(smokeSpawnPoint.position, 1.5f);
        
        Gizmos.color = Color.red;
        if (pattern2Box != null)
            Gizmos.DrawWireCube(pattern2Box.position, pattern2Size);
    }
  

    
    #endregion
}
