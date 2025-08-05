using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss4_Pattern3_DroneChaser : MonoBehaviour
{
   [Header("드론 이동/추격")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float chaseDuration = 5f;

    [Header("폭발 관련")]
    [SerializeField] private float explodeRange = 0.5f;
    [SerializeField] private int damage = 100;
    [SerializeField] private float explosionDelay = 0.5f;
    [SerializeField] private float explodeRadius = 1.5f; // 실제 폭발 범위

    [Header("이펙트 프리팹")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject explosionRangeEffectPrefab;
    [SerializeField] private GameObject explosionWarningPrefab;
    [SerializeField] private float explosionWarningDuration = 0.5f;

    private Transform player;
    private bool isExploding = false;
    private GameObject rangeEffectInstance;

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        StartCoroutine(SelfDestructAfterTime());

        // 드론 몸에 붙는 범위 시각화 이펙트
        if (explosionRangeEffectPrefab != null)
        {
            
            //GameObject rangeEffectInstance = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage4BossSkillPattern3Proj02, transform.position, Quaternion.identity);
            rangeEffectInstance = Instantiate(
                explosionRangeEffectPrefab,
                transform.position,
                Quaternion.identity,
                transform // 드론에 따라다니도록 부모 설정
            );
            rangeEffectInstance.transform.localScale = Vector3.one * explodeRange * 2f;
        }
    }

    private void Update()
    {
        if (player == null || isExploding) return;

        // 플레이어 쫓기
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // 폭발 조건
        if (Vector2.Distance(transform.position, player.position) < explodeRange)
        {
            StartCoroutine(ExplodeDelayed(explosionDelay));
        }
    }
    
    private IEnumerator SelfDestructAfterTime()
    {
        yield return new WaitForSeconds(chaseDuration);
        StartCoroutine(ExplodeDelayed(explosionDelay));
    }

    private IEnumerator ExplodeDelayed(float delay)
    {
        if (isExploding) yield break;
        isExploding = true;

        // 폭발 경고
        if (explosionWarningPrefab != null)
        {
            GameObject warning = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage4BossSkillPattern3Proj03, transform.position, Quaternion.identity);
           // GameObject warning = Instantiate(explosionWarningPrefab, transform.position, Quaternion.identity);
            warning.transform.localScale = Vector3.one * explodeRadius * 2f;
            StartCoroutine(DelayedReturnToPool(explosionWarningDuration));
            //Destroy(warning, explosionWarningDuration);
            ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage4BossSkillPattern3Proj03, warning);
        }

        yield return new WaitForSeconds(delay);

        // 데미지 판정은 범위 내 모든 플레이어 대상으로
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRadius, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            Player p = hit.GetComponent<Player>();
            if (p != null)
                p.TakeDamage(damage);
        }

        if (explosionEffectPrefab != null)
        {
            GameObject fx = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage4BossSkillPattern3Proj04, transform.position, Quaternion.identity);
            // fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            StartCoroutine(DelayedReturnToPool(1f));
            //Destroy(fx, 1f);
            ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage4BossSkillPattern3Proj04, fx);
        }

        SoundManager.Instance.Play("HIT");
        ObjectPoolManager.Instance.ReturnToPool(PoolType.Stage4BossSkillPattern3Proj03, gameObject);
        //(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isExploding) return;

        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExplodeDelayed(explosionDelay));
        }
    }
    IEnumerator DelayedReturnToPool(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
#endif
}
