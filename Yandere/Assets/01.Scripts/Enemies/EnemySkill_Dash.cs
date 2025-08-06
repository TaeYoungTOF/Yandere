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
        
        Debug.Log("[에너미] 돌진 시작");

        dashDir = (_player.position - transform.position).normalized;
        
        //  // ✅ 시전 준비시간
        GameObject chargeEffect = ObjectPoolManager.Instance.GetFromPool(PoolType.EnemyChargeSkill, transform.position, Quaternion.identity);
        StartCoroutine(ReturnToPoolAfterDelay(chargeEffect, 0.3f, PoolType.EnemyChargeSkill));

        // ✅ 워닝 이펙트 표시
        if (dashWarningEffect != null)
        {
            dashWarningEffect.enabled = true;
            ShowDashPreview(dashDir); // 방향 고정
        }

        yield return new WaitForSeconds(patternWarningTime); // 경고 시간
        
        if (dashWarningEffect != null)
            dashWarningEffect.enabled = false;
        

        // ▼ 아래는 기존 대시 로직 유지
        float elapsed = 0f;
        bool hitOnce = false;
        
        SoundManager.Instance.Play("InGame_Enemy_DashRunSFX01");
        while (elapsed < duration)
        {
          
            GameObject dash = ObjectPoolManager.Instance.GetFromPool(PoolType.EnemyDashSkill, transform.position, Quaternion.identity);
            StartCoroutine(ReturnToPoolAfterDelay(dash, 0.3f, PoolType.EnemyDashSkill));

            transform.position += (Vector3)(dashDir * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;

            // 충돌 감지
            if (!hitOnce)
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.8f, Vector2.zero, 0f, playerLayer);
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

        yield return new WaitForSeconds(afterDelay);
        _isDashing = false;
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
