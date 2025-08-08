using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EType_Attack : EnemyController
{
    [Header("패턴 쿨타임")]
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private float cooldownTimer = 0f;             // 현재 쿨다운 진행상황

    [Header("수류탄 설정")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float damagePerTick = 10f;
    [SerializeField] private float damagePerSecond = 1f;
    [SerializeField] private float damageDuration = 5f;
    [SerializeField] private float damagePerRadius = 5f;

    [Header("투척 설정")]
    [SerializeField] private float grenadeThrowHeight = 3f;
    [SerializeField] private float grenadeDuration = 1.5f;
    [SerializeField] private float grenadeRange = 3;


    protected override void Start()
    {
        base.Start();
        cooldownTimer = cooldown;
    }
    void Update()
    {
        if (isDead || _playerTransform == null) return;

        if(cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
        
        float distanceToPlayer  = Vector2.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= grenadeRange && cooldownTimer <= 0f)
        {
            _animator.SetTrigger("Attack");
            ThrowGrenade();
            cooldownTimer = cooldown; // 쿨타임 초기화
        }

    }

    public override  void MonsterAttack()
    {
        _animator.SetTrigger("Attack");

        if (isDead) return;

        ThrowGrenade();

        // 공격 쿨타임 초기화
        DelayAttack(cooldown);
    }

    #region 수류탄 투척 로직

    void ThrowGrenade()
    {
        if (_playerTransform == null || projectilePrefab == null)
        {
            Debug.Log("몬스터 E타입 수류탄 투척 실패: 대상 또는 프리팹이 없음");
            return;
        }

        Vector3 startPos = projectileSpawnPoint.position;
        Vector3 targetPos = _playerTransform.position;
        
        SoundManager.Instance.Play("InGame_EnemyBoss_ThrowingSFX");
        
        // 🎯 1. 수류탄 생성
        GameObject grenade = ObjectPoolManager.Instance.GetFromPool(PoolType.EnemyEAttackGrenadeProj01, startPos, Quaternion.identity);
        float scale = 1f * 1f * 1f;
        grenade.transform.localScale = new Vector3(scale, scale, 0.35f);
        
        // 🎯 2. 컴포넌트 찾고 Init
        Enemy_EType_Attack_Projectile01 proj = grenade.GetComponent<Enemy_EType_Attack_Projectile01>();

        if (proj != null)
        {
            proj.Init(targetPos, grenadeThrowHeight, damagePerTick, damagePerSecond, damagePerRadius, grenadeDuration,projectileSpawnPoint.position, damageDuration);
        }
        else
        {
            Debug.LogError("💥 Init 실패! GetComponentEnemy_EType_Attack_Projectile01>() 결과가 null");
        }
        
    }

    #endregion
    
    #region Scene창 사거리 기즈모 표시

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, grenadeRange);
        #endif
    }

    #endregion
}
