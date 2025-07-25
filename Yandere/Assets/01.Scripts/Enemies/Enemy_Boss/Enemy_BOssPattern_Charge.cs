using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossPattern_Charge : MonoBehaviour, IBossPattern
{
    
    [SerializeField, Tooltip("남은 쿨타임 (읽기 전용)")]
    private float debugCooldownLeft;
  
    [Header("돌진 속성")]
    [SerializeField] private float chargeSpeedMultiplier = 3f;
    [SerializeField] private float chargeDuration = 1.2f;
    [SerializeField] private float windUpTime = 2.5f;
    [SerializeField] private float cooldown = 30f;

    [Header("충돌 피해")]
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float damagePercent = 0.15f;

    [Header("이펙트")]
    [SerializeField] private Enemy_BossDashPreview dashPreview;  // 인스펙터에서 연결 (대쉬 거리 표시)
    [SerializeField] private GameObject dashParticlePrefab;      // 인스펙터에서 연결 (대쉬 이펙트 표시)
    [SerializeField] private float dashParticlePrefabDestroyTime = 0.5f;

    private Transform _player;
    private Rigidbody2D _rigid;
    private EnemyController _enemyController;

    private float _timer;
    private bool _isDone;
    private bool _isCharging;
    private bool _canUseDash = false;
    private Vector2 _chargeDir;

    public bool IsDone => _isDone;
    public bool IsDashing => _isCharging;
    public float DashForce => _enemyController.enemyData.monsterMoveSpeed * chargeSpeedMultiplier;
    public bool CanExecute()
    {
        return _timer <= 0f && _canUseDash;
    }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
        _rigid = GetComponent<Rigidbody2D>();
        _enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            debugCooldownLeft = _timer; // 인스펙터 확인용
        }
        else
        {
            debugCooldownLeft = 0f;
        }
    }

    public void Execute()
    {
        StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        _isDone = false;
        _isCharging = false;
        
        _enemyController.isDashing = true;

        // 1. 준비 단계 (방향 고정)
        _chargeDir = (_player.position - transform.position).normalized;

        // 1-1. 궤적 이펙트 생성
        SpawnChargeEffect();

        Debug.Log("[Charge] 준비 중...");
        yield return new WaitForSeconds(windUpTime);

        if (dashParticlePrefab != null)
        {
            GameObject effect = Instantiate(dashParticlePrefab, transform.position, Quaternion.identity);
            Destroy(effect, dashParticlePrefabDestroyTime); // 2초 뒤 제거 (원하는 시간 조절 가능)
        }
        
        // 2. 돌진 시작
        SoundManagerTest.Instance.Play("InGame_EnemyBoss_DashSkillSFX");
        _isCharging = true;
        float speed = _enemyController.enemyData.monsterMoveSpeed * chargeSpeedMultiplier;
        float elapsed = 0f;

        while (elapsed < chargeDuration)
        {
            _rigid.velocity = _chargeDir * speed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        _rigid.velocity = Vector2.zero;
        _isCharging = false;
        
        _enemyController.isDashing = false;

        yield return new WaitForSeconds(1f); // 후딜레이

        _timer = cooldown;
        _isDone = true;
    }

    private void SpawnChargeEffect()
    {
        if (dashPreview != null)
        {
            dashPreview.direction = _chargeDir;
            dashPreview.ShowDashDirection();
        }
    }
    
    public void SetCanUseDash(bool value)
    {
        _canUseDash = value;
    }
    
    
}
