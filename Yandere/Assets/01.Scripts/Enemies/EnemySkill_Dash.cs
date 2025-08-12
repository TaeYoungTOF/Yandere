using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill_Dash : MonoBehaviour
{
    [Header("돌진 설정")]
    [SerializeField] private float cooldown = 5f;
    [SerializeField] private float dashSpeed = 10f;
    public float radius;
    [SerializeField] private float duration = 0.3f;
    //[SerializeField] private float warningTime = 0.5f;
    [SerializeField] private float afterDelay = 1f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float dashDamage = 20f;

    
    [Header("이펙트")]
    [SerializeField] private GameObject chargeEffect;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private LineRenderer dashWarningEffect;
    [SerializeField] private float patternWarningTime = 1f;
    [SerializeField] private LayerMask playerLayer;


    private Rigidbody2D _rb;
    private EnemyController _controller;
    private Transform _player;
    private Vector2 dashDir;
    private bool _canUseDash = false;
    private bool _isDashing = false;
    private float timer;
    public bool IsDashing() => _isDashing;
    
    private void Start()
    {
        timer = cooldown; // 시작하자마자 돌진 발동 못하게 타이머 채워줌
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<EnemyController>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_canUseDash && timer <= 0f && !_isDashing)
        {
            StartCoroutine(DoDash());
        }

        if (timer > 0f)
            timer -= Time.deltaTime;
    }
    public void SetCanUseDash(bool value)
    {
        _canUseDash = value;
    }

    private IEnumerator DoDash()
    {
          _isDashing = true;
    timer = cooldown;

    // ✅ 패턴 시작: 이동/런 애니 끄기
    _controller.SetPatterning(true);

    // 대시 방향 고정
    dashDir = (_player.position - transform.position).normalized;

    // ▷ 차지 이펙트 + 예열 동안 정지
    GameObject charge = ObjectPoolManager.Instance.GetFromPool(
        PoolType.EnemyChargeSkill, transform.position, Quaternion.identity);

    // 경고 라인 키기
    if (dashWarningEffect != null)
    {
        dashWarningEffect.enabled = true;
        ShowDashPreview(dashDir);
    }

    // 예열 대기 (이 동안 EnemyController는 isPatterning으로 이동 안 함)
    yield return new WaitForSeconds(patternWarningTime);

    if (dashWarningEffect != null) dashWarningEffect.enabled = false;
    StartCoroutine(ReturnToPoolAfterDelay(charge, 0.3f, PoolType.EnemyChargeSkill));

    // ▷ 실제 대시 구간
    float elapsed = 0f;
    bool hitOnce = false;
    SoundManager.Instance.Play("InGame_Enemy_DashRunSFX01");

    while (elapsed < duration)
    {
        // 이펙트
        GameObject dash = ObjectPoolManager.Instance.GetFromPool(
            PoolType.EnemyDashSkill, transform.position, Quaternion.identity);
        StartCoroutine(ReturnToPoolAfterDelay(dash, 0.3f, PoolType.EnemyDashSkill));

        // 이동 (Rigidbody2D를 쓰면 velocity로 해도 좋음)
        transform.position += (Vector3)(dashDir * dashSpeed * Time.deltaTime);
        elapsed += Time.deltaTime;

        // 충돌 1회 판정
        if (!hitOnce)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.6f, Vector2.zero, 0f, playerLayer);
            if (hit.collider != null)
            {
                Player player = hit.collider.GetComponent<Player>();
                if (player != null)
                {
                    Vector2 knockDir = (player.transform.position - transform.position).normalized;
                    player.TakeDamage(dashDamage);
                    player.GetComponent<Rigidbody2D>()?.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
                    hitOnce = true;
                }
            }
        }

        yield return null;
    }

    // 후딜
    _rb.velocity = Vector2.zero; // 혹시 모를 잔여 속도 제거
    yield return new WaitForSeconds(afterDelay);

    // ✅ 패턴 종료: 다시 움직이게
    _controller.SetPatterning(false);
    _isDashing = false;

    // 부모 공격 쿨 좀 밀기
    _controller.DelayAttack(0.5f);
    }
    public float GetDetectRadius()
    {
        return radius; // detectRadius는 돌진 발동 거리
    }
    private void ShowDashPreview(Vector2 dir)
    {
        if (dashWarningEffect == null) return;

        Vector3 start = transform.position;
        float dashDistance = dashSpeed * duration; // ✅ 실제 돌진 거리 계산
        Vector3 end = start + (Vector3)(dir.normalized * dashDistance);

        dashWarningEffect.SetPosition(0, start);
        dashWarningEffect.SetPosition(1, end);
    }
    
    
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType type)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null && obj.activeInHierarchy)
        {
            ObjectPoolManager.Instance.ReturnToPool(type, obj);
        }
    }
    
}
