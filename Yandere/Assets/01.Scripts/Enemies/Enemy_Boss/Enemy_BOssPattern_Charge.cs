using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BOssPattern_Charge : MonoBehaviour, IBossPattern
{
  
    [Header("돌진 속성")]
    [SerializeField] private float chargeSpeedMultiplier = 3f;
    [SerializeField] private float chargeDuration = 1.2f;
    [SerializeField] private float windUpTime = 2.5f;
    [SerializeField] private float cooldown = 30f;

    [Header("충돌 피해")]
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float damagePercent = 0.15f;

    [Header("이펙트")]
    [SerializeField] private GameObject chargeEffectPrefab; // 바닥 궤적 이펙트

    private Transform _player;
    private Rigidbody2D _rigid;
    private EnemyController _enemyController;

    private float _timer;
    private bool _isDone;
    private bool _isCharging;
    private Vector2 _chargeDir;

    public bool IsDone => _isDone;
    public bool CanExecute() => _timer <= 0f;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player")?.transform;
        _rigid = GetComponent<Rigidbody2D>();
        _enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        if (_timer > 0f) _timer -= Time.deltaTime;
    }

    public void Execute()
    {
        StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        _isDone = false;
        _isCharging = false;

        // 1. 준비 단계 (방향 고정)
        _chargeDir = (_player.position - transform.position).normalized;

        // 1-1. 궤적 이펙트 생성
        SpawnChargeEffect();

        Debug.Log("[Charge] 준비 중...");
        yield return new WaitForSeconds(windUpTime);

        // 2. 돌진 시작
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

        yield return new WaitForSeconds(1f); // 후딜레이

        _timer = cooldown;
        _isDone = true;
    }

    private void SpawnChargeEffect()
    {
        if (chargeEffectPrefab == null) return;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + (Vector3)(_chargeDir * 5f); // 이펙트 길이

        GameObject effect = Instantiate(chargeEffectPrefab, startPos, Quaternion.identity);
        Vector3 dir = endPos - startPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        effect.transform.rotation = Quaternion.Euler(0, 0, angle);
        effect.transform.localScale = new Vector3(dir.magnitude, 1f, 1f);
        Destroy(effect, windUpTime + chargeDuration + 1f);
    }

    // 충돌 처리용 함수
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isCharging) return;
        if (!collision.CompareTag("Player")) return;

        // 넉백
        Vector2 pushDir = (collision.transform.position - transform.position).normalized;
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
        if (playerRb != null)
            playerRb.AddForce(pushDir * knockbackForce, ForceMode2D.Impulse);

        // // 데미지
        // float damage = PlayerManager.Instance.MaxHP * damagePercent;
        // StageManager.Instance.Player.TakeDamage(damage);
        //
        // Debug.Log($"[Charge] 플레이어 넉백 + 데미지 {damage}");
    }
}
