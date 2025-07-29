using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossController4 : EnemyController
{
    
    
    [Header("보스 공격 범위 (패턴 사용 시 Scence 사정거리 표시)")]
    
    [Header("패턴 쿨타임")]
    [SerializeField] private float pattern1Cooldown = 12f;

    [Header("보스 패턴1 스모크 스크린")]
    [SerializeField] private GameObject pattern1SmokeEffectPrefab;
    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private float pattern1DetectDistance = 8f;
    [SerializeField] private float grenadeThrowHeight = 3f;
    [SerializeField] private float grenadeDuration = 1.5f;

    private float pattern1Timer = 0f;

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

    private IEnumerator BossPatternRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1f);
            if (isPatterning) continue;

            bool canUsePattern1 = pattern1Timer <= 0f && IsPlayerInPattern1Range();

            List<IEnumerator> availablePatterns = new();

            if (canUsePattern1) availablePatterns.Add(ExecutePattern1());

            if (availablePatterns.Count > 0)
            {
                int rand = Random.Range(0, availablePatterns.Count);
                StartCoroutine(availablePatterns[rand]);
            }
        }
    }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        if (smokeSpawnPoint != null)
            Gizmos.DrawWireSphere(smokeSpawnPoint.position, 1.5f);
    }
}
