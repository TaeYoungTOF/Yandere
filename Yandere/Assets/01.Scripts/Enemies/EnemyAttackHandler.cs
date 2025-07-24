using System;
using System.Collections;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    private Transform _playerTransform;
    private EnemyController _enemyController;
    
    [Header("에너미 원거리 공격 관련")]
    [SerializeField] private GameObject rangeBulletPrefab;                          // 에너미 원거리 불렛 프리팹
    [SerializeField] private int bulletCount = 3;              // 몇 발 발사할지
    [SerializeField] private float bulletDelay = 0.2f;         // 발사 간격 (초)
    
    [Header("에너미 대쉬 스킬 관련")]
    [SerializeField] private float enemyDashCooldown = 5f;                          // 에너미 대쉬 스킬 쿨다운
    [SerializeField] private float enemyDashTimer = 0f;                             // 에너미 대쉬 스킬 타이머
    
    [Header("에너미 연막 스킬 관련")]
    [SerializeField] private float enemyBoomCooldown = 10f;                         // 에너미 연막탄 스킬 쿨다운
    [SerializeField] private float enemyBoomTimer = 0f;                             // 에너미 연막탄 스킬 타이머
    [SerializeField] private GameObject smokeBulletPrefab;                          // 에너미 연막탄 불렛 프리팹
    [SerializeField] private float boomSpreadAngle = 15f;                           // 연막탄 갈래 각도

    [Header("에너미 불렛 스폰 위치")]
    [SerializeField] private Transform bulletSpawnPoint;                            // 불렛이 소환되는 위치
    
    void Awake()
    {
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null)
        {
            Debug.Log("EnemyAttackHandler : EnemyController를 못찾음.");
        }
        
        // 플레이어 찾아서 기억해두기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("플레이어가 씬에 없음!");
        }
    }

    private void Update()
    {
        // Handler에서 돌진 쿨타임 직접 관리
        if (enemyDashTimer > 0f)
            enemyDashTimer -= Time.deltaTime;

        // 만약 연막탄 쿨타임도 있으면 같이 돌리기
        if (enemyBoomTimer > 0f)
            enemyBoomTimer -= Time.deltaTime;
    }

    public void UpdateDashTimer()
    {
        if (enemyDashTimer > 0f)
        {
            enemyDashTimer -= Time.deltaTime;
        }
    }

    public bool CanDash()
    {
        return enemyDashTimer <= 0f;
    }

    public void ResetDashTimer()
    {
        enemyDashTimer = enemyDashCooldown;
    }

    public bool CanBoom()
    {
        return enemyBoomTimer <= 0f;
    }

    public void ResetBoomTimer()
    {
        enemyBoomTimer = enemyBoomCooldown;
    }
    
    
    public void MonsterAttack(EnemyAttackType attackType, float damage)
    {
        switch (attackType)
        {
            case EnemyAttackType.AttackType_A:
                DoTypeA_Attack(damage);
                break;
            case EnemyAttackType.AttackType_B:
                DoTypeB_Attack(damage);
                break;
            case EnemyAttackType.AttackType_C:
                DoTypeC_Attack(damage);
                break;
            case EnemyAttackType.AttackType_D:
                DoTypeD_Attack(damage);
                break;
            default:
                Debug.LogWarning("Unknown attack type!");
                break;
        }
    }

    private void DoTypeA_Attack(float damage)
    {
        if (_playerTransform == null)
        {
            Debug.Log("EnemyAttackHandler A타입 : 플레이어가 없음!");
            return;
        }

        // 플레이어한테 데미지 주기
        StageManager.Instance.Player.TakeDamage(damage);
        Debug.Log($"공격타입A 기본 공격! {damage} 데미지를 플레이어에게 줌");
        // 예: 근접 타격
    }

    private void DoTypeB_Attack(float damage)
    {
        if (_playerTransform == null) return;

        Vector2 dirToPlayer = (_playerTransform.position - transform.position).normalized;
    
        Debug.Log("[Check] 연발 시작"); // 추가
        StartCoroutine(FireBulletsSequentially(dirToPlayer, damage));
    }

    private void DoTypeC_Attack(float damage)
    {
        if (_playerTransform == null)
        {
            Debug.Log("EnemyAttackHandler C타입 : 플레이어가 없음!");
            return;
        }
        
        StageManager.Instance.Player.TakeDamage(damage);
        Debug.Log($"공격타입C 기본 공격! {damage} 데미지를 플레이어에게 줌");
        if (CanDash())
        {
            Debug.Log("공격타입C 돌진 스킬 발동!");
            EnemyDashSkill();
            ResetDashTimer();
        }
        
        // 예: 대쉬 공격
    }
    
    private void DoTypeD_Attack(float damage)
    {
        if (_playerTransform == null)
        {
            Debug.Log("EnemyAttackHandler D타입 : 플레이어가 없음!");
            return;
        }
        StageManager.Instance.Player.TakeDamage(damage);
        Debug.Log($"공격타입D 기본 공격! {damage} 데미지를 플레이어에게 줌");
        
        if (CanBoom())
        {
            Debug.Log("공격타입D 연막 스킬 발동!");
            EnemyBoomSkill();
            ResetBoomTimer();
        }
        
        // 예: 연막 공격
    }

    public void EnemyDashSkill()
    {
        if (_playerTransform == null) return;
        
        Vector2 dashDir = (_playerTransform.position - transform.position).normalized;
        Rigidbody2D _rigidbody2D = GetComponent<Rigidbody2D>();

        float dashForce = 10f;                                                              // 돌진 속도
        
        _enemyController.isDashing = true;
        _rigidbody2D.velocity = dashDir * dashForce;
        Debug.Log("[Dash] 돌진 시작!");

        StartCoroutine(StopDashAfter(0.3f));
    }

    IEnumerator StopDashAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        _enemyController.isDashing = false;
        Debug.Log("[Dash] 돌진 끝!");
    }

    public void EnemyBoomSkill()
    {
        if (_playerTransform == null) return;

        Vector2 dirToPlayer = (_playerTransform.position - transform.position).normalized;

        for (int i = -1; i <= 1; i++)
        {
            float angleOffset = boomSpreadAngle * i;
            Vector2 spreadDir = RotateVector(dirToPlayer, angleOffset);

            GameObject bullet = Instantiate(smokeBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyBoomProjectile>().Init(spreadDir);
        }

        Debug.Log("[Boom] 3갈래 연막탄 DOTween 발사!");
    }
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        float newX = v.x * cos - v.y * sin;
        float newY = v.x * sin + v.y * cos;

        return new Vector2(newX, newY).normalized;
    }
    
    private IEnumerator FireBulletsSequentially(Vector2 direction, float damage)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(rangeBulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<EnemyRangeProjectile>().Init(direction, damage); // ✅ direction 고정되어야 함

            yield return new WaitForSeconds(bulletDelay);
        }

        Debug.Log($"[Range] 연발 완료: {bulletCount}발, delay: {bulletDelay}");
    }
}
