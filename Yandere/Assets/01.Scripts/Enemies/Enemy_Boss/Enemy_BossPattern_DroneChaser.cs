using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossPattern_DroneChaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float chaseDuration = 5f;
    [SerializeField] private float explodeRange = 0.5f;
    [SerializeField] private int damage = 100;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject explosionRangeEffectPrefab;

    private Transform player;
    private bool isExploding = false;
    private GameObject rangeEffectInstance;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SelfDestructAfterTime());

        // 범위 워닝 이펙트 생성
        if (explosionRangeEffectPrefab != null)
        {
            rangeEffectInstance = Instantiate(
                explosionRangeEffectPrefab, 
                transform.position, 
                Quaternion.identity,
                transform // 드론에 붙임
            );
            rangeEffectInstance.transform.localScale = Vector3.one * explodeRange * 2f;
        }
    }

    private void Update()
    {
        if (player == null || isExploding) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        if (Vector2.Distance(transform.position, player.position) < explodeRange)
        {
            StartCoroutine(ExplodeDelayed(1f)); // 0.5초 후 폭발
        }
    }

    private void Explode()
    {
        if (player != null)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
            {
                p.TakeDamage(damage);
            }
        }

        // 💥 폭발 이펙트 생성 후 1.5초 뒤 제거
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1.5f); // 1.5초 후 제거
        }

        // 🔊 폭발 사운드
        SoundManager.Instance.Play("HIT");

        Destroy(gameObject);
    }

    private IEnumerator SelfDestructAfterTime()
    {
        yield return new WaitForSeconds(chaseDuration);
        Explode();
    }
    
    private IEnumerator ExplodeDelayed(float delay)
    {
        if (isExploding) yield break;
        isExploding = true;

        yield return new WaitForSeconds(delay);
        Explode();
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
#endif
    
}
