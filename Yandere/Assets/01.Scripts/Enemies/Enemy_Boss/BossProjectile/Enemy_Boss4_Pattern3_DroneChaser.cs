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

    private Transform targetPlayer;
    private bool isExploding = false;
    private GameObject rangeEffectInstance;

    // private void Start()
    // {
    //     player = GameObject.FindWithTag("Player")?.transform;
    //     StartCoroutine(SelfDestructAfterTime());
    //
    //     // 드론 몸에 붙는 범위 시각화 이펙트
    //     if (explosionRangeEffectPrefab != null)
    //     {
    //         
    //         //GameObject rangeEffectInstance = ObjectPoolManager.Instance.GetFromPool(PoolType.Stage4BossSkillPattern3Proj02, transform.position, Quaternion.identity);
    //         rangeEffectInstance = Instantiate(
    //             explosionRangeEffectPrefab,
    //             transform.position,
    //             Quaternion.identity,
    //             transform // 드론에 따라다니도록 부모 설정
    //         );
    //         rangeEffectInstance.transform.localScale = Vector3.one * explodeRange * 2f;
    //     }
    // }
    
    public void Init(Vector3 spawnPosition, Transform player)
    {
        transform.position = spawnPosition;
        targetPlayer = player;
        isExploding = false;

        StopAllCoroutines();
        StartCoroutine(SelfDestructAfterTime());  // ⬅️ 이거 추가
       
        if (explosionRangeEffectPrefab != null)
        {
            rangeEffectInstance = Instantiate(
                explosionRangeEffectPrefab,
                transform.position,
                Quaternion.identity,
                transform
            );
            rangeEffectInstance.transform.localScale = Vector3.one * explodeRange * 2f;
        }
    }

    

    private void Update()
    {
        if (targetPlayer == null || isExploding) return;

        // 플레이어 쫓기
        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // 폭발 조건
        if (Vector2.Distance(transform.position, targetPlayer.position) < explodeRange)
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
            //Destroy(warning, explosionWarningDuration);
            StartCoroutine(ReturnToPoolAfterDelay(warning, explosionWarningDuration, PoolType.Stage4BossSkillPattern3Proj03));
            
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
            
            //Destroy(fx, 1f);
            StartCoroutine(ReturnToPoolAfterDelay(fx, 1f, PoolType.Stage4BossSkillPattern3Proj04));
        }

        SoundManager.Instance.Play("InGame_EnemyBoss4_Pattern3_DroneExplosion");
        StartCoroutine(ReturnToPoolAfterDelay(gameObject, 0.1f, PoolType.Stage4BossSkillPattern3Proj01));
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
    
    private IEnumerator ReturnToPoolAfterDelay(GameObject obj, float delay, PoolType poolType)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null && obj.activeInHierarchy)
        {
            ObjectPoolManager.Instance.ReturnToPool(poolType, obj);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
#endif
}
