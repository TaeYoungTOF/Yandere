using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss1_Pattern3_Projectile01 : MonoBehaviour
{

    [Header("이펙트 설정")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject explsotionWarningEffectPrefab;

    [Header("수류탄 설정")]
    [SerializeField] private float smokeTickDamage = 10f;
    [SerializeField] private float smokeDamageperTickTime = 1f;
    [SerializeField] private float smokeRadius = 3f;
    [SerializeField] private float smokeDuration = 5f; 
    
    private Vector3 targetPos;
    private float moveDuration;
    private float arcHeight;
    
    public void Init(Vector3 target, float height, float duration)
    {
        targetPos = target;
        arcHeight = height;
        moveDuration = duration;

        // ✅ 도착 지점에 경고 이펙트 생성
        if (explsotionWarningEffectPrefab != null)
        {
            GameObject war = Instantiate(explsotionWarningEffectPrefab, targetPos, Quaternion.identity);
            Destroy(war, 0.8f);
        }

        StartCoroutine(MoveToTarget());
    }


    private IEnumerator MoveToTarget()
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            float heightOffset = arcHeight * Mathf.Sin(t * Mathf.PI); // 포물선

            transform.position = Vector3.Lerp(startPos, targetPos, t) + new Vector3(0, heightOffset, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Explode();
    }
    
    private void Explode()
    {
        // ✅ 폭발 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(explosionEffectPrefab, targetPos, Quaternion.identity);
            Destroy(effect, smokeDuration); // 연막 지속 시간 후 제거
        }

        // ✅ 데미지 주는 영역 생성 (코루틴 시작)
        StartCoroutine(DamageOverTime());
    }
    
    private IEnumerator DamageOverTime()
    {
        float timer = 0f;

        while (timer < smokeDuration)
        {
            // 반지름 내 플레이어 찾기
            Collider2D[] hits = Physics2D.OverlapCircleAll(targetPos, smokeRadius, LayerMask.GetMask("Player"));

            foreach (var hit in hits)
            {
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    StageManager.Instance.Player.TakeDamage(smokeTickDamage);
                    Debug.Log($"[연막데미지] {smokeTickDamage} 피해 적용");
                }
            }

            yield return new WaitForSeconds(smokeDamageperTickTime);
            timer += smokeDamageperTickTime;
        }

        Destroy(gameObject); // 끝나면 수류탄 오브젝트도 제거
    }

}
